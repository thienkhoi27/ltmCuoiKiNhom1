import socket
import ssl
import threading
import uuid
from models import VoteSession
from protocol import recv_json
from handlers import ServerState, handle_message

HOST = "0.0.0.0"
PORT = 8443

def load_vote_config(path: str):
    with open(path, "r", encoding="utf-8") as f:
        lines = [line.strip() for line in f.readlines()]
    lines = [l for l in lines if l]

    if len(lines) < 3:
        raise ValueError("vote.txt phải có: 1 dòng chủ đề, 1 dòng time:, và ít nhất 1 lựa chọn.")

    topic = lines[0]
    if not lines[1].lower().startswith("time:"):
        raise ValueError("Dòng thứ 2 phải có dạng: time: <số>")
    limit_time = int(lines[1].split(":", 1)[1].strip())

    options = lines[2:]
    return topic, options, limit_time


def client_thread(state: ServerState, conn, addr):
    state.add_client(conn, addr)
    state.admin_log(f"[+] Client {addr} connected")

    try:
        while True:
            msg = recv_json(conn)
            handle_message(state, conn, msg)
    except Exception as e:
        state.admin_log(f"[-] Client {addr} disconnected ({e})")
    finally:
        try:
            conn.close()
        except:
            pass
        state.remove_client(conn)
        state.broadcast({"type": "CLIENT_LIST", "clients": state.client_list()}, role="admin")


def main():
    topic, options, limit_time = load_vote_config("vote.txt")
    session = VoteSession(session_id=uuid.uuid4().hex[:10], topic=topic, options=options, limit_time=limit_time)
    state = ServerState(session)

    context = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)
    context.load_cert_chain(certfile="server.crt", keyfile="server.key")

    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
        sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        sock.bind((HOST, PORT))
        sock.listen(20)
        print(f"[SERVER] Listening on {HOST}:{PORT}")

        with context.wrap_socket(sock, server_side=True) as ssock:
            while True:
                conn, addr = ssock.accept()
                t = threading.Thread(target=client_thread, args=(state, conn, addr), daemon=True)
                t.start()


if __name__ == "__main__":
    main()
=======
votes = {}
lock = threading.Lock()


private void btnConnect_Click(object sender, EventArgs e)
{
    string host = txtServer.Text.Trim();
    int port = int.Parse(txtPort.Text);
    clientId = txtClientId.Text.Trim();

    tcpClient = new TcpClient(host, port);

    sslStream = new SslStream(
        tcpClient.GetStream(),
        false,
        (sender2, cert, chain, errors) => true
    );

    sslStream.AuthenticateAsClient(host);

    reader = new StreamReader(sslStream, Encoding.UTF8);
    writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true };

    SendLine($"HELLO|{clientId}");

    string resp = ReadLine();
    var parts = resp.Split('|');

    lblTopic.Text = parts[1];
    lstOptions.Items.Clear();
    foreach (var opt in parts[2].Split(','))
        lstOptions.Items.Add(opt);

    remainSeconds = int.Parse(parts[3]);
    voteTimer.Start();
}

def build_result_string():
    counts = {opt: 0 for opt in OPTIONS}
    for cid, idx in votes.items():
        if 1 <= idx <= len(OPTIONS):
            counts[OPTIONS[idx - 1]] += 1
    return ";".join(f"{opt}:{count}" for opt, count in counts.items())


def handle_client(conn, addr):
    print(f"[+] Client from {addr}")
    client_id = None
    f = conn.makefile("rwb")

    def send_line(s: str):
        f.write((s + "\n").encode("utf-8"))
        f.flush()

    try:
        while True:
            raw = f.readline()
            if not raw:
                break

            msg = raw.decode("utf-8-sig").strip()

            if msg.startswith("HELLO|"):
                client_id = msg.split("|", 1)[1]
                opts = ",".join(OPTIONS)
                send_line(f"WELCOME|{TOPIC}|{opts}|{LIMIT_TIME}")

            elif msg.startswith("VOTE|"):
                if client_id is None:
                    send_line("ERR|NOT_AUTH")
                    continue

                try:
                    idx = int(msg.split("|")[1])
                except:
                    send_line("ERR|INVALID_OPTION")
                    continue

                if not (1 <= idx <= len(OPTIONS)):
                    send_line("ERR|INVALID_OPTION")
                    continue

                with lock:
                    if client_id in votes:
                        send_line("ERR|ALREADY_VOTED")
                    else:
                        votes[client_id] = idx
                        send_line(f"OK|VOTED|{OPTIONS[idx-1]}")

            elif msg == "RESULT?":
                with lock:
                    send_line("RESULT|" + build_result_string())
            else:
                send_line("ERR|UNKNOWN_CMD")
    finally:
        conn.close()


