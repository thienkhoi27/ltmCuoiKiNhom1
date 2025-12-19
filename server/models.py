from dataclasses import dataclass, field
from typing import Dict, List, Optional
import time
import uuid

@dataclass
class ClientInfo:
    addr: tuple
    role: str = "client"          # "client" | "admin"
    client_id: Optional[str] = None
    connected_at: float = field(default_factory=time.time)

@dataclass
class VoteSession:
    session_id: str
    topic: str
    options: List[str]
    limit_time: int
    start_time: float = field(default_factory=time.time)
    votes: Dict[str, int] = field(default_factory=dict)  # client_id -> index (1-based)

    def remaining(self) -> int:
        elapsed = int(time.time() - self.start_time)
        r = self.limit_time - elapsed
        return r if r > 0 else 0

    def reset(self, topic: str, options: List[str], limit_time: int):
        self.session_id = uuid.uuid4().hex[:10]
        self.topic = topic
        self.options = options
        self.limit_time = limit_time
        self.start_time = time.time()
        self.votes.clear()

    def count_votes(self) -> Dict[str, int]:
        counts = {opt: 0 for opt in self.options}
        for _, idx in self.votes.items():
            if 1 <= idx <= len(self.options):
                counts[self.options[idx - 1]] += 1
        return counts
