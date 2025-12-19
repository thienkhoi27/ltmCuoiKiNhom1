using System;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Buffers.Binary;

namespace ltmCuoiKiNhom1
{
    public sealed class NetworkManager : IDisposable
    {
        private TcpClient? _tcp;
        private SslStream? _ssl;
        private Thread? _rxThread;
        private readonly object _txLock = new object();
        private volatile bool _running;

        public event Action? OnConnected;
        public event Action<string>? OnDisconnected;
        public event Action<string>? OnError;
        public event Action<JsonObject>? OnMessage;

        public void Connect(string host, int port, bool acceptAnyCert)
        {
            _tcp = new TcpClient(host, port);
            _ssl = new SslStream(_tcp.GetStream(), false,
                (sender, cert, chain, errors) => acceptAnyCert);

            _ssl.AuthenticateAsClient(host, null,
                SslProtocols.Tls12 | SslProtocols.Tls13, false);

            _running = true;
            _rxThread = new Thread(ReadLoop) { IsBackground = true };
            _rxThread.Start();

            OnConnected?.Invoke();
        }

        public void Send(System.Text.Json.Nodes.JsonObject obj)
        {
            if (_ssl == null) throw new InvalidOperationException("Chưa kết nối");

            // Cách ổn định trên .NET 8: dùng JsonSerializer thay vì obj.ToJsonString(options)
            var options = new System.Text.Json.JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            // JsonObject implements IDictionary-like, nhưng JsonSerializer serialize ổn định nhất khi serialize JsonNode
            string json = System.Text.Json.JsonSerializer.Serialize(obj, options);

            byte[] payload = System.Text.Encoding.UTF8.GetBytes(json);

            byte[] header = new byte[4];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(header, payload.Length);

            lock (_txLock)
            {
                _ssl.Write(header, 0, 4);
                _ssl.Write(payload, 0, payload.Length);
                _ssl.Flush();
            }
        }


        private void ReadLoop()
        {
            try
            {
                if (_ssl == null) return;

                while (_running)
                {
                    byte[] header = ReadExact(_ssl, 4);
                    int len = BinaryPrimitives.ReadInt32BigEndian(header);
                    if (len <= 0 || len > 10_000_000) throw new Exception("Length không hợp lệ: " + len);

                    byte[] payload = ReadExact(_ssl, len);
                    string json = Encoding.UTF8.GetString(payload);

                    JsonNode? node = JsonNode.Parse(json);
                    if (node is JsonObject jo)
                        OnMessage?.Invoke(jo);
                }
            }
            catch (Exception ex)
            {
                OnDisconnected?.Invoke(ex.Message);
            }
        }

        private static byte[] ReadExact(SslStream s, int n)
        {
            byte[] buf = new byte[n];
            int off = 0;
            while (off < n)
            {
                int r = s.Read(buf, off, n - off);
                if (r <= 0) throw new Exception("Socket closed");
                off += r;
            }
            return buf;
        }

        public void Dispose()
        {
            _running = false;
            try { _ssl?.Close(); } catch { }
            try { _tcp?.Close(); } catch { }
        }
    }
}
