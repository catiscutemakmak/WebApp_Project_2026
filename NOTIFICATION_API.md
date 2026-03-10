## Notification System - API Usage Guide

### Overview
ระบบ notification ได้รับการปรับปรุงให้ทำงานกับ room status updates ใน matches

### ไฟล์ที่เพิ่ม/แก้ไข
1. **NoticeController.cs** - เพิ่ม API endpoints
2. **NotificationService.cs** - ตรรมชาติใหม่ (Services folder)
3. **Program.cs** - ลงทะเบียน NotificationService
4. **RoomHub.cs** - เพิ่ม notification groups

---

## API Endpoints (ใน NoticeController)

### 1. ดึงจำนวน notification ที่ยังไม่อ่าน
```
GET /api/notifications/unread-count
```

**Response:**
```json
{
  "unreadCount": 5
}
```

---

### 2. ดึงรายการ notification ที่ยังไม่อ่าน
```
GET /api/notifications/unread?limit=10
```

**Response:**
```json
[
  {
    "id": 1,
    "message": "ยินดีด้วย! คุณถูกยอมรับเข้าห้อง 'Room1' (LOL)",
    "roomId": 123,
    "isRead": false,
    "createdAt": "2026-03-09T10:30:00",
    "roomName": "Room1",
    "gameName": "LOL"
  },
  {
    "id": 2,
    "message": "'Player123' ขอเข้าห้อง 'Room2' (Dota2)",
    "roomId": 124,
    "isRead": false,
    "createdAt": "2026-03-09T10:25:00",
    "roomName": "Room2",
    "gameName": "Dota2"
  }
]
```

---

### 3. ดึงรายการ notification ทั้งหมด (รวมอ่านแล้ว)
```
GET /api/notifications/all?limit=50
```

**Response:** เหมือน unread แต่รวม notifications ที่อ่านแล้วด้วย

---

### 4. ท่า notification เป็นอ่านแล้ว
```
POST /api/notifications/{notificationId}/read
```

**Response:**
```json
{
  "message": "Notification marked as read"
}
```

---

### 5. ลบ notification
```
DELETE /api/notifications/{notificationId}
```

**Response:**
```json
{
  "message": "Notification deleted"
}
```

---

### 6. ท่าการอ่าน notification ทั้งหมด
```
POST /api/notifications/mark-all-read
```

**Response:**
```json
{
  "message": "Marked 5 notifications as read"
}
```

---

## Frontend Integration

### JavaScript - SignalR Real-time Notifications

```javascript
// 1. Initialize SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/roomhub")
    .withAutomaticReconnect()
    .build();

connection.start().catch(err => console.error(err));

// 2. Join user's notification group
const userId = document.querySelector('[data-user-id]').getAttribute('data-user-id');
connection.invoke("JoinNotificationGroup", userId)
    .catch(err => console.error(err));

// 3. Listen for real-time notifications
connection.on("NotificationReceived", (notification) => {
    console.log("Notification received:", notification);
    // Update UI, show toast, etc.
});

// 4. Fetch unread notifications via REST API
fetch('/api/notifications/unread')
    .then(r => r.json())
    .then(notifications => {
        console.log(notifications);
        // Update notification badge/list
    });

// 5. Mark notification as read
fetch(`/api/notifications/${notificationId}/read`, { method: 'POST' })
    .then(r => r.json())
    .then(data => console.log(data));
```

---

## Notification Messages

### 1. Player Join Queue (Private Room)
```
Player receives: "คุณถูกเพิ่มลงในคิวห้อง 'RoomName' (GameName) แล้ว รอให้เจ้าของห้องตรวจสอบคำขอของคุณ"
Owner receives: "'PlayerNickname' ขอเข้าห้อง 'RoomName' (GameName)"
```

### 2. Queue Accepted
```
Player receives: "ยินดีด้วย! คุณถูกยอมรับเข้าห้อง 'RoomName' (GameName)"
```

### 3. Queue Rejected
```
Player receives: "ขออภัย ร้องขอของคุณเข้าห้อง 'RoomName' (GameName) ถูกปฏิเสธแล้ว"
```

---

## How to Send Notifications Programmatically

```csharp
// Inject NotificationService in your controller
private readonly NotificationService _notificationService;

// Send notification to queue player
await _notificationService.NotifyPlayerAddedToQueueAsync(
    playerUserId: userProfile.Id,
    roomId: room.Id,
    roomName: room.RoomName,
    gameName: room.Game.GameName
);

// Send notification to room owner
await _notificationService.NotifyRoomOwnerNewQueueAsync(
    ownerUserId: room.OwnerId,
    roomId: room.Id,
    playerNickname: userProfile.Nickname,
    roomName: room.RoomName,
    gameName: room.Game.GameName
);

// Send accepted notification
await _notificationService.NotifyPlayerAcceptedAsync(
    playerUserId: queuePlayer.UserId,
    roomId: room.Id,
    roomName: room.RoomName,
    gameName: room.Game.GameName
);

// Send rejected notification
await _notificationService.NotifyPlayerRejectedAsync(
    playerUserId: queuePlayer.UserId,
    roomId: room.Id,
    roomName: room.RoomName,
    gameName: room.Game.GameName
);
```

---

## Database Schema (Existing)

```sql
CREATE TABLE Notifications (
    Id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    UserProfileId INT NOT NULL,
    Message VARCHAR(255) NOT NULL,
    RoomId INT,
    IsRead BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserProfileId) REFERENCES UserProfiles(Id),
    FOREIGN KEY (RoomId) REFERENCES Rooms(Id)
);
```

---

## Notes
- ทุก notification จะถูก broadcast via SignalR ผ่าน group `notification-{userId}`
- ทุก notification จะถูกบันทึกลงฐานข้อมูล
- Notifications มี soft-delete (ลบได้แต่ยังเก็บไว้)
- สามารถท่า notifications เป็นอ่านแล้วเพื่ออัปเดตสถานะ
