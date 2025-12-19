<div align="center">

<img src="https://cdn-icons-png.flaticon.com/512/927/927295.png" width="110" alt="Voting"/>

# NHÓM 1 – LẬP TRÌNH MẠNG
## HỆ THỐNG BỎ PHIẾU AN TOÀN (Secure Voting System)

Giao tiếp Client–Server qua **TLS/SSL**, dữ liệu đóng gói theo **Framing Protocol**:
**[4 bytes length – Big Endian] + [JSON payload UTF-8]**

</div>

---

## 1) Giới thiệu

Đây là ứng dụng bỏ phiếu điện tử theo mô hình **Client–Server**.
Mục tiêu chính:

-   **Bảo mật:** Đường truyền được mã hóa bằng **TLS/SSL** (chống nghe lén, giảm nguy cơ sửa đổi dữ liệu).
-   **Độ tin cậy:** Thiết kế **Framing Protocol** để xử lý bản chất "stream" của TCP (chống dính gói/cắt gói - TCP Sticking/Fragmentation).
-   **Nghiệp vụ:** Đảm bảo mỗi MSSV/Client ID chỉ được vote 1 lần, có đếm ngược thời gian server-side.
-   **Realtime:** Kết quả bỏ phiếu được cập nhật tức thời trên biểu đồ của tất cả Client.
-   **Admin Dashboard:** Công cụ quản trị mạnh mẽ để giám sát log, xem danh sách kết nối, kick client và khởi tạo cuộc bầu cử mới (Broadcast).

---

## 2) Tính năng

### 2.1 Server (Python)
-   **TLS/SSL:** Sử dụng `ssl.SSLContext` với `PROTOCOL_TLS_SERVER` và load cặp khóa `server.crt`, `server.key`.
-   **Đa luồng:** Xử lý mỗi kết nối client trên một thread riêng biệt.
-   **Custom Protocol:** Nhận/gửi dữ liệu theo định dạng Header (Length) + Payload (JSON).
-   **Quản lý Session:** Lưu trữ trạng thái phiên bầu cử (`session_id`, `topic`, `options`, `limit_time`).
-   **Broadcast System:**
    -   Khi có vote mới -> Gửi `RESULT_UPDATE` cho tất cả.
    -   Khi Admin tạo bầu cử -> Gửi `NEW_ELECTION` để reset toàn bộ hệ thống.

### 2.2 Client Vote (C# WinForms)
-   **Secure Connection:** Kết nối qua `SslStream` và thực hiện handshake `AuthenticateAsClient`.
-   **Architecture:** Tách biệt logic mạng (`NetworkManager`) và giao diện (`Form`). UI chỉ lắng nghe sự kiện (`OnMessage`, `OnResult`).
-   **Giao diện:**
    -   Hiển thị Topic, Timer đồng bộ từ Server.
    -   Biểu đồ cột (Column Chart) cập nhật realtime bằng thư viện **LiveCharts**.

### 2.3 Admin Dashboard (C# WinForms)
-   **Quyền quản trị:** Đăng nhập bằng `admin_key`.
-   **Giám sát:**
    -   Nhận Log hệ thống realtime.
    -   Xem danh sách Client đang kết nối (IP, Port, ID).
-   **Điều khiển:**
    -   **Kick:** Ngắt kết nối một client cụ thể.
    -   **New Election:** Tạo đề tài mới, set thời gian mới và gửi lệnh Broadcast để toàn bộ hệ thống chuyển sang phiên bầu cử mới ngay lập tức.

---

## 3) Kiến trúc & Luồng dữ liệu

### 3.1 Mô hình
-   Giao thức: `TCP` -> `TLS Layer` -> `Framing Layer` -> `JSON Messages`.
-   Server phục vụ đồng thời 2 loại client: **Voter** (người dùng) và **Admin** (quản trị).

### 3.2 Luồng hoạt động (Workflow)
1.  **Handshake:** Client kết nối TLS tới Server.
2.  **Auth:** Client gửi `HELLO` (kèm ID).
3.  **Sync:** Server trả `WELCOME` (kèm Topic, Options, Thời gian còn lại).
4.  **Action:** Client gửi `VOTE`.
5.  **Response:** Server trả `OK` (thành công) hoặc `ERR` (nếu đã vote).
6.  **Update:** Server broadcast `RESULT_UPDATE` tới toàn bộ Client đang kết nối.
7.  **Admin:** Có thể gửi lệnh `ADMIN_NEW_ELECTION` để reset toàn bộ quy trình về Bước 3 với dữ liệu mới.

---

## 4) Thiết kế giao thức (Protocol)

### 4.1 Framing Protocol
Để giải quyết vấn đề TCP Stream, mọi gói tin đều tuân thủ định dạng:

```text
[HEADER (4 bytes)] [PAYLOAD (N bytes)]
```

-   **Header:** Số nguyên 4-byte (Big Endian) biểu thị độ dài của Payload.
-   **Payload:** Chuỗi JSON (UTF-8).

### 4.2 Các loại Message (JSON Structure)

Dưới đây là cấu trúc các gói tin JSON chính:

#### A. Nhóm xác thực & Khởi tạo

**Client gửi HELLO:**
```json
{ "type": "HELLO", "client_id": "MSSV001" }
```

**Server trả WELCOME:**
```json
{
    "type": "WELCOME",
    "session_id": "abc12345",
    "topic": "Bầu lớp trưởng",
    "options": ["Nguyễn Văn A", "Trần Thị B"],
    "limit_time": 60,
    "remaining": 58
}
```

#### B. Nhóm Bỏ phiếu

**Client gửi VOTE:**
```json
{ "type": "VOTE", "option_index": 1 }
```

**Server trả OK:**
```json
{ "type": "OK", "result": "VOTED", "option": "Trần Thị B" }
```

**Server trả ERR:**
```json
{ "type": "ERR", "code": "ALREADY_VOTED", "message": "Bạn đã bỏ phiếu rồi!" }
```

**Server Broadcast Kết quả (Realtime):**
```json
{
    "type": "RESULT_UPDATE",
    "counts": { "Nguyễn Văn A": 5, "Trần Thị B": 3 },
    "remaining": 45
}
```

#### C. Nhóm Admin

**Admin gửi lệnh tạo bầu cử mới:**
```json
{
    "type": "ADMIN_NEW_ELECTION",
    "topic": "Bầu Bí thư",
    "options": ["Ứng viên X", "Ứng viên Y"],
    "limit_time": 120
}
```

**Server Broadcast New Election (tới tất cả):**
```json
{
    "type": "NEW_ELECTION",
    "topic": "Bầu Bí thư",
    "options": ["..."],
    "limit_time": 120,
    "remaining": 120
}
```

---

## 5) Cấu trúc thư mục

```text
Repo/
├── Server/
│   ├── server.py           # Main Entry: Socket loop + Threading
│   ├── models.py           # Classes: VoteSession, ClientInfo
│   ├── protocol.py         # Hàm đóng gói/giải gói (Framing)
│   ├── handlers.py         # Logic xử lý tin nhắn JSON
│   ├── vote.txt            # File cấu hình mặc định
│   ├── server.crt          # SSL Certificate
│   └── server.key          # SSL Private Key
│
└── Client/
    ├── NetworkManager.cs   # Xử lý TLS, Framing, Events
    ├── VoteChart.cs        # Điều khiển LiveCharts
    ├── MainMenuForm.cs     # Màn hình chọn chế độ
    ├── VoteForm.cs         # Giao diện bỏ phiếu
    └── AdminForm.cs        # Giao diện quản trị
```

---

## 6) Hướng dẫn Cài đặt & Chạy

### 6.1 Yêu cầu hệ thống
-   **Server:** Python 3.9 trở lên.
-   **Client:** Windows OS, Visual Studio 2022, .NET Framework / .NET 6+.
-   **Thư viện:** `LiveCharts` (Cài qua NuGet cho Client).

### 6.2 Tạo chứng chỉ SSL (Quan trọng)
Tại thư mục `Server/`, chạy lệnh OpenSSL để tạo Self-signed Certificate:

```bash
openssl req -new -newkey rsa:2048 -days 365 -nodes -x509 -keyout server.key -out server.crt
```

### 6.3 Cấu hình file `vote.txt`
Tạo file `vote.txt` cùng thư mục `server.py` với nội dung mẫu (UTF-8):

```text
Bầu chọn Gương mặt đại diện
time: 120
Nguyễn Văn A
Lê Thị B
Trần Văn C
```

### 6.4 Chạy Server
Mở terminal tại thư mục Server và chạy:

```bash
python server.py
```

*Server sẽ lắng nghe tại `0.0.0.0:8443`.*

### 6.5 Chạy Client
1.  Mở Solution bằng Visual Studio.
2.  Chuột phải vào Project -> **Manage NuGet Packages** -> Cài đặt `LiveCharts` và `LiveCharts.WinForms`.
3.  Nhấn **Start** để chạy.
4.  Tại màn hình chính:
    -   Chọn **Tham gia Vote**: Nhập IP, Port, MSSV -> Kết nối -> Bỏ phiếu.
    -   Chọn **Admin**: Nhập IP, Port, Admin Key (mặc định: `admin`) -> Quản lý.

---

## 7) Thành viên nhóm & Phân công

| STT | Thành viên | Vai trò |
| :--- | :--- | :--- |
| **1** | **Nguyễn Thu Hương** | **Team Leader** – Server Core, TLS/SSL, Framing Protocol |
| **2** | **Hoàng Thị Kiều Diễm** | **Documentation** – Viết tài liệu, Test Case, Hỗ trợ UI |
| **3** | **Lê Thiện Khôi** | **Client Dev** – WinForms, Tích hợp LiveCharts, NetworkManager |
| **4** | **Nguyễn Tuấn Kiệt** | **Protocol Design** – Thiết kế JSON Schema, Testing, Edge cases |
| **5** | **Hoàng Thanh Hải** | **Security QA** – Kiểm thử bảo mật, Admin Dashboard testing |

---

<div align="center">
  <sub>Developed by Group 1 - Network Programming Class</sub>
</div>
