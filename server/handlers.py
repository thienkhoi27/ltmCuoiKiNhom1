import threading
from typing import Dict, Tuple
from models import ClientInfo, VoteSession
from protocol import send_json

class ServerState:
    def __init__(self, session: VoteSession):
        self.session = session
        self.clients: Dict[object, ClientInfo] = {}   # sock -> ClientInfo
        self.send_locks: Dict[object, threading.Lock] = {}
        self.lock = threading.Lock()

    def add_client(self, sock, addr: Tuple[str, int]):
        with self.lock:
            self.clients[sock] = ClientInfo(addr=addr)
            self.send_locks[sock] = threading.Lock()

    def remove_client(self, sock):
        with self.lock:
            self.clients.pop(sock, None)
            self.send_locks.pop(sock, None)

    def get_send_lock(self, sock):
        with self.lock:
            return self.send_locks.get(sock)

    def broadcast(self, msg: dict, role: str = None):
        # role=None => all, role="client"/"admin" => filter
        with self.lock:
            items = list(self.clients.items())
        for s, info in items:
            if role and info.role != role:
                continue
            try:
                send_json(s, msg, self.get_send_lock(s))
            except:
                pass

    def admin_log(self, text: str):
        self.broadcast({"type": "LOG", "message": text}, role="admin")

    def broadcast_result_update(self):
        data = self.session.count_votes()
        self.broadcast({
            "type": "RESULT_UPDATE",
            "session_id": self.session.session_id,
            "remaining": self.session.remaining(),
            "data": data
        }, role="client")
        self.broadcast({
            "type": "RESULT_UPDATE",
            "session_id": self.session.session_id,
            "remaining": self.session.remaining(),
            "data": data
        }, role="admin")

    def client_list(self):
        with self.lock:
            res = []
            for _, info in self.clients.items():
                if info.role == "client":
                    res.append({
                        "client_id": info.client_id or "",
                        "addr": f"{info.addr[0]}:{info.addr[1]}"
                    })
            return res


def handle_message(state: ServerState, sock, msg: dict):
    t = msg.get("type", "")

    # ========== CLIENT FLOW ==========
    if t == "HELLO":
        client_id = (msg.get("client_id") or "").strip()
        if not client_id:
            send_json(sock, {"type": "ERR", "code": "INVALID_ID", "message": "client_id rỗng"}, state.get_send_lock(sock))
            return

        with state.lock:
            info = state.clients.get(sock)
            if info:
                info.role = "client"
                info.client_id = client_id

        state.admin_log(f"[CONNECT] client_id={client_id}")
        send_json(sock, {
            "type": "WELCOME",
            "session_id": state.session.session_id,
            "topic": state.session.topic,
            "options": state.session.options,
            "limit_time": state.session.limit_time,
            "remaining": state.session.remaining()
        }, state.get_send_lock(sock))
        return

    if t == "VOTE":
        session_id = (msg.get("session_id") or "").strip()
        index = msg.get("index", None)

        with state.lock:
            info = state.clients.get(sock)
            client_id = info.client_id if info else None

        if not client_id:
            send_json(sock, {"type": "ERR", "code": "NOT_AUTH", "message": "Chưa HELLO"}, state.get_send_lock(sock))
            return

        if session_id != state.session.session_id:
            send_json(sock, {"type": "ERR", "code": "SESSION_MISMATCH", "message": "Sai session_id"}, state.get_send_lock(sock))
            return

        if state.session.remaining() <= 0:
            send_json(sock, {"type": "ERR", "code": "TIME_EXPIRED", "message": "Hết thời gian"}, state.get_send_lock(sock))
            return

        if not isinstance(index, int):
            send_json(sock, {"type": "ERR", "code": "INVALID_OPTION", "message": "index phải là số"}, state.get_send_lock(sock))
            return

        if not (1 <= index <= len(state.session.options)):
            send_json(sock, {"type": "ERR", "code": "INVALID_OPTION", "message": "index ngoài phạm vi"}, state.get_send_lock(sock))
            return

        with state.lock:
            if client_id in state.session.votes:
                send_json(sock, {"type": "ERR", "code": "ALREADY_VOTED", "message": "Bạn đã bỏ phiếu"}, state.get_send_lock(sock))
                return
            state.session.votes[client_id] = index

        opt_text = state.session.options[index - 1]
        state.admin_log(f"[VOTE] {client_id} -> {opt_text}")
        send_json(sock, {
            "type": "OK",
            "result": "VOTED",
            "option_text": opt_text,
            "remaining": state.session.remaining()
        }, state.get_send_lock(sock))

        state.broadcast_result_update()
        return

    if t == "RESULT":
        send_json(sock, {
            "type": "RESULT",
            "session_id": state.session.session_id,
            "remaining": state.session.remaining(),
            "data": state.session.count_votes()
        }, state.get_send_lock(sock))
        return

    # ========== ADMIN FLOW ==========
    if t == "ADMIN_HELLO":
        key = (msg.get("admin_key") or "").strip()
        if key != "admin":
            send_json(sock, {"type": "ERR", "code": "ADMIN_DENIED", "message": "Sai admin_key"}, state.get_send_lock(sock))
            return

        with state.lock:
            info = state.clients.get(sock)
            if info:
                info.role = "admin"

        state.admin_log("[ADMIN] Admin connected")
        send_json(sock, {
            "type": "ADMIN_WELCOME",
            "session_id": state.session.session_id,
            "topic": state.session.topic,
            "options": state.session.options,
            "limit_time": state.session.limit_time,
            "remaining": state.session.remaining(),
            "clients": state.client_list()
        }, state.get_send_lock(sock))
        return

    if t == "ADMIN_LIST":
        send_json(sock, {"type": "CLIENT_LIST", "clients": state.client_list()}, state.get_send_lock(sock))
        return

    if t == "ADMIN_KICK":
        target_id = (msg.get("client_id") or "").strip()
        if not target_id:
            send_json(sock, {"type": "ERR", "code": "INVALID_ID", "message": "client_id rỗng"}, state.get_send_lock(sock))
            return

        kicked = False
        with state.lock:
            items = list(state.clients.items())
        for s, info in items:
            if info.role == "client" and info.client_id == target_id:
                try:
                    send_json(s, {"type": "KICKED", "message": "Bạn đã bị admin kick"}, state.get_send_lock(s))
                    s.close()
                except:
                    pass
                kicked = True
                break

        if kicked:
            state.admin_log(f"[KICK] {target_id}")
            state.broadcast({"type": "CLIENT_LIST", "clients": state.client_list()}, role="admin")
            send_json(sock, {"type": "OK", "result": "KICKED", "client_id": target_id}, state.get_send_lock(sock))
        else:
            send_json(sock, {"type": "ERR", "code": "NOT_FOUND", "message": "Không tìm thấy client"}, state.get_send_lock(sock))
        return

    if t == "ADMIN_NEW_ELECTION":
        topic = (msg.get("topic") or "").strip()
        options = msg.get("options") or []
        limit_time = msg.get("limit_time")

        if not topic or not isinstance(options, list) or len(options) < 1 or not isinstance(limit_time, int) or limit_time <= 0:
            send_json(sock, {"type": "ERR", "code": "INVALID_CONFIG", "message": "topic/options/limit_time không hợp lệ"}, state.get_send_lock(sock))
            return

        options = [str(x).strip() for x in options if str(x).strip()]
        if not options:
            send_json(sock, {"type": "ERR", "code": "INVALID_CONFIG", "message": "options rỗng"}, state.get_send_lock(sock))
            return

        state.session.reset(topic, options, limit_time)

        state.admin_log(f"[NEW_ELECTION] topic={topic} options={len(options)} limit={limit_time}s")
        msg_reset = {
            "type": "NEW_ELECTION",
            "session_id": state.session.session_id,
            "topic": state.session.topic,
            "options": state.session.options,
            "limit_time": state.session.limit_time,
            "remaining": state.session.remaining()
        }
        state.broadcast(msg_reset, role="client")
        state.broadcast(msg_reset, role="admin")
        state.broadcast_result_update()

        send_json(sock, {"type": "OK", "result": "NEW_ELECTION"}, state.get_send_lock(sock))
        return

    send_json(sock, {"type": "ERR", "code": "UNKNOWN_CMD", "message": "Lệnh không hỗ trợ"}, state.get_send_lock(sock))
