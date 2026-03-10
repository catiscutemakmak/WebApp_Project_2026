// notice.js — รับ notifications จาก @Html.Raw(Json.Serialize(Model)) ใน Razor view
// ตัวแปร `notifications` ถูก inject ก่อน script นี้โหลด

function getDateLabel(createdAt) {
    const today     = new Date();
    const yesterday = new Date();
    yesterday.setDate(today.getDate() - 1);

    today.setHours(0, 0, 0, 0);
    yesterday.setHours(0, 0, 0, 0);

    const notice = new Date(createdAt);
    notice.setHours(0, 0, 0, 0);

    if (notice.getTime() === today.getTime())     return "TODAY";
    if (notice.getTime() === yesterday.getTime()) return "Yesterday";

    return notice.toLocaleDateString("th-TH");
}

function getThaiTime(createdAt) {
    return new Date(createdAt).toLocaleTimeString("th-TH", {
        timeZone: "Asia/Bangkok",
        hour:     "2-digit",
        minute:   "2-digit",
        hour12:   false
    });
}

const noticeContainer = document.getElementById("NoticeContainer");
let current_label = null;
let groupDiv = null;

if (!notifications || notifications.length === 0) {
    // ── Empty state ────────────────────────────────────────────────────────
    const emptyContainer = document.createElement("div");
    emptyContainer.classList.add("empty-notice-container");

    const emptyText = document.createElement("p");
    emptyText.classList.add("empty-notice-text");
    emptyText.textContent = "It's quite lonely right now";

    emptyContainer.appendChild(emptyText);
    noticeContainer.appendChild(emptyContainer);

} else {
    notifications.forEach(notice => {
        const label = getDateLabel(notice.createdAt);   // camelCase จาก Json.Serialize

        // ── สร้าง date header ถ้าวันเปลี่ยน ──────────────────────────────────
        if (current_label !== label) {
            if (groupDiv) {
                noticeContainer.appendChild(groupDiv);
            }

            groupDiv = document.createElement("div");

            const dateHeader = document.createElement("h3");
            dateHeader.classList.add("date-header");
            dateHeader.textContent = label;
            groupDiv.appendChild(dateHeader);

            current_label = label;
        }

        // ── notice card ───────────────────────────────────────────────────────
        const noticeCard = document.createElement("div");
        noticeCard.classList.add("notice-dev");
        if (!notice.isRead) noticeCard.classList.add("unread");

        // message box (ซ้าย)
        const messageBox = document.createElement("div");
        messageBox.classList.add("message-dev");

        const messageText = document.createElement("p");
        messageText.classList.add("message-text");
        messageText.textContent = notice.message;

        const messageTime = document.createElement("p");
        messageTime.classList.add("message-time");
        messageTime.textContent = getThaiTime(notice.createdAt);

        messageBox.appendChild(messageText);
        messageBox.appendChild(messageTime);

        // detail box (ขวา)
        const detailBox = document.createElement("div");
        detailBox.classList.add("detail-dev");

        const detailText = document.createElement("p");
        detailText.classList.add("detail-text");
        detailText.textContent = notice.message;

        // ── FIX: Json.Serialize ส่ง camelCase ─────────────────────────────────
        // notice.room?.game?.gameName  (C# serialize เป็น camelCase)
        const gameLabel = notice.room?.game?.gameName ?? "Unknown Game";

        const detailGame = document.createElement("p");
        detailGame.classList.add("detail-game");
        detailGame.textContent = gameLabel;

        detailBox.appendChild(detailText);
        detailBox.appendChild(detailGame);

        noticeCard.appendChild(messageBox);
        noticeCard.appendChild(detailBox);
        groupDiv.appendChild(noticeCard);
    });

    // flush กลุ่มสุดท้าย
    if (groupDiv) {
        noticeContainer.appendChild(groupDiv);
    }
}