// notifications จะถูกส่งมาจาก view ผ่าน JSON
// const notice_mock = [...]; // Removed - using real data from server


function getDateLabel(createdAt) {
    const today = new Date();
    const yesterday = new Date();

    yesterday.setDate(today.getDate() - 1);

    today.setHours(0,0,0,0);
    yesterday.setHours(0,0,0,0);

    const notice = new Date(createdAt);
    notice.setHours(0,0,0,0);
    
    if (notice.getTime() === today.getTime()) return "TODAY";
    if (notice.getTime() === yesterday.getTime()) return "Yesterday";
    
    return notice.toLocaleDateString("th-TH");
}

function getThaiTime(createdAt) {
    return new Date(createdAt).toLocaleTimeString("th-TH", {
        timeZone: "Asia/Bangkok",
        hour: "2-digit",
        minute: "2-digit",
        hour12: false
    });
}

const noticeContainer = document.getElementById("NoticeContainer");
let current_label = null;
let noticeDev = null;

// แสดงข้อความว่าง ถ้าไม่มี notification
if (!notifications || notifications.length === 0) {
    const emptyContainer = document.createElement("div");
    emptyContainer.classList.add("empty-notice-container");
    
    const emptyText = document.createElement("p");
    emptyText.classList.add("empty-notice-text");
    emptyText.innerHTML = "It's quite lonely right now";
    emptyContainer.appendChild(emptyText);
    noticeContainer.appendChild(emptyContainer);
} else {
    notifications.forEach(notice => {
        const label = getDateLabel(notice.createdAt);

        if (current_label !== label) {
            if (noticeDev) {
                noticeContainer.appendChild(noticeDev);
            }

            noticeDev = document.createElement("div");

            const date_header = document.createElement("h3");
            date_header.classList.add("date-header");
            date_header.innerText = label;
            noticeDev.appendChild(date_header);

            current_label = label;
        }
        
        const noticedev = document.createElement("div")
        noticedev.classList.add("notice-dev")
        
        // แสดง Actor Label ถ้ามี
        if (notice.actorUserName) {
            const actorLabel = document.createElement("div");
            actorLabel.classList.add("actor-label");
            
            if (notice.actorProfileImage) {
                const actorImg = document.createElement("img");
                actorImg.src = notice.actorProfileImage;
                actorImg.alt = notice.actorUserName;
                actorImg.classList.add("actor-avatar");
                actorLabel.appendChild(actorImg);
            }
            
            const actorName = document.createElement("span");
            actorName.classList.add("actor-name");
            actorName.innerText = notice.actorUserName;
            actorLabel.appendChild(actorName);
            
            noticedev.appendChild(actorLabel);
        }
        
        const messagebox = document.createElement("div");
        const messagechat = document.createElement("p")
        messagebox.classList.add("message-dev");
        messagechat.innerText = notice.message;
        
        const messagedate = document.createElement("p")
        messagechat.classList.add("message-text");
        messagedate.classList.add("message-time");
        messagedate.innerText = getThaiTime(notice.createdAt);
        messagebox.appendChild(messagechat);
        messagebox.appendChild(messagedate)

        const detailbox = document.createElement("div");
        const detailchat = document.createElement("p")
        detailbox.classList.add("detail-dev");
        detailchat.innerText = notice.message;
        
        const detailgame = document.createElement("p")
        detailchat.classList.add("detail-text");
        detailgame.classList.add("detail-game");
        detailgame.innerText = notice.gameName || "Unknown Game";
        detailbox.appendChild(detailchat);
        detailbox.appendChild(detailgame)
        
        noticedev.appendChild(messagebox);
        noticedev.appendChild(detailbox);
        noticeDev.appendChild(noticedev);
    });
    
    if (noticeDev) {
        noticeContainer.appendChild(noticeDev);
    }
}
