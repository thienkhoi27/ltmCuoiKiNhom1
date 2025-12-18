import socket
import ssl
import threading

HOST = "0.0.0.0"
PORT = 8443

# =========================
# Đọc cấu hình vote
# =========================
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
    if not options:
        raise ValueError("Phải có ít nhất 1 lựa chọn.")

    print("[CONFIG] Chủ đề:", topic)
    print("[CONFIG] Thời gian:", limit_time)
    for i, opt in enumerate(options, 1):
        print(f"{i}. {opt}")

    return topic, options, limit_time


TOPIC, OPTIONS, LIMIT_TIME = load_vote_config("vote.txt")

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

