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
