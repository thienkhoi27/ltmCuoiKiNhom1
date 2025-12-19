using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace CuoiKiLTM
{
    public sealed class FramingClient : IDisposable
    {
        private TcpClient? _tcp;
        private SslStream? _ssl;
        private CancellationTokenSource? _cts;

        public event Action? OnConnected;
        public event Action<string>? OnDisconnected;
        public event Action<string>? OnError;
        public event Action<JsonObject>? OnMessage;

        public bool IsConnected => _ssl != null;

        public async Task ConnectAsync(string host, int port, bool acceptAnyCert = true)
        {
            try
            {
                Dispose();

                _tcp = new TcpClient();
                await _tcp.ConnectAsync(host, port);

                _ssl = new SslStream(
                    _tcp.GetStream(),
                    false,
                    (sender, cert, chain, errors) => acceptAnyCert || errors == SslPolicyErrors.None
                );

                _ssl.AuthenticateAsClient(host, null, SslProtocols.Tls12 | SslProtocols.Tls13, false);

                _cts = new CancellationTokenSource();
                _ = Task.Run(() => RecvLoop(_cts.Token));

                OnConnected?.Invoke();
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
                Dispose();
                throw;
            }
        }

        public async Task SendAsync(JsonObject obj)
        {
            if (_ssl == null) throw new InvalidOperationException("Not connected.");

            string json = obj.ToJsonString();
            byte[] payload = Encoding.UTF8.GetBytes(json);
            int len = payload.Length;

            byte[] header = new byte[4];
            header[0] = (byte)((len >> 24) & 0xFF);
            header[1] = (byte)((len >> 16) & 0xFF);
            header[2] = (byte)((len >> 8) & 0xFF);
            header[3] = (byte)(len & 0xFF);

            await _ssl.WriteAsync(header, 0, 4);
            await _ssl.WriteAsync(payload, 0, payload.Length);
            await _ssl.FlushAsync();
        }

        private async Task RecvLoop(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested && _ssl != null)
                {
                    string json = await ReadFrameAsync(_ssl, ct);
                    var node = JsonNode.Parse(json) as JsonObject;
                    if (node != null) OnMessage?.Invoke(node);
                }
            }
            catch (Exception ex)
            {
                OnDisconnected?.Invoke(ex.Message);
            }
            finally
            {
                Dispose();
            }
        }

        private static async Task<string> ReadFrameAsync(SslStream s, CancellationToken ct)
        {
            byte[] header = await ReadExactAsync(s, 4, ct);
            int len = (header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3];
            if (len <= 0 || len > 5_000_000) throw new Exception("Invalid frame length: " + len);

            byte[] payload = await ReadExactAsync(s, len, ct);
            return Encoding.UTF8.GetString(payload);
        }

        private static async Task<byte[]> ReadExactAsync(SslStream s, int n, CancellationToken ct)
        {
            byte[] buf = new byte[n];
            int read = 0;
            while (read < n)
            {
                int r = await s.ReadAsync(buf, read, n - read, ct);
                if (r == 0) throw new Exception("Remote closed.");
                read += r;
            }
            return buf;
        }

        public void Dispose()
        {
            try { _cts?.Cancel(); } catch { }
            try { _ssl?.Close(); } catch { }
            try { _tcp?.Close(); } catch { }

            _cts = null;
            _ssl = null;
            _tcp = null;
        }
    }
}
