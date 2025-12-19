import json
import struct

def _read_exact(sock, n: int) -> bytes:
    buf = b""
    while len(buf) < n:
        chunk = sock.recv(n - len(buf))
        if not chunk:
            raise ConnectionError("Socket closed while reading")
        buf += chunk
    return buf

def recv_json(sock) -> dict:
    header = _read_exact(sock, 4)
    (length,) = struct.unpack("!I", header)
    if length <= 0 or length > 10_000_000:
        raise ValueError(f"Invalid payload length: {length}")
    payload = _read_exact(sock, length)
    text = payload.decode("utf-8")
    return json.loads(text)

def send_json(sock, obj: dict, send_lock=None):
    data = json.dumps(obj, ensure_ascii=False).encode("utf-8")
    header = struct.pack("!I", len(data))
    if send_lock:
        with send_lock:
            sock.sendall(header + data)
    else:
        sock.sendall(header + data)
