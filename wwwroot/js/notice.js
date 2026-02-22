
const notice_mock = [
  {
    "id": 1,
    "message": "copterkeyes has applied to your MLBB annoucement as EXP.",
    "detail" : "Your post “หาคนเล่นทุกตำแหน่งครับ ผมเมนป่า ตอนนี้” ",
    "createdAt": "2026-02-19T10:30:00Z",
    "game": "MLBB"
  },
    {
    "id": 2,
    "message": "Domeza has applied to your MLBB annoucement as Gold",
    "detail" : "Your post “หาคนเล่นทุกตำแหน่งครับ ผมเมนป่า ตอนนี้” ",
    "createdAt": "2026-02-19T08:30:00Z",
    "game": "MLBB"
  },
  {
    "id": 3,
    "message": "Ichi has applied to your MLBB annoucement as Mage",
    "detail" : "Your post “หาคนเล่นทุกตำแหน่งครับ ผมเมนป่า ตอนนี้” ",
    "createdAt": "2026-02-19T06:30:00Z",
    "game": "MLBB"
  },
  {
    "id": 4,
    "message": "Match started",
    "detail" : "Your post “หาคนเล่นทุกตำแหน่งครับ ผมเมนป่า ตอนนี้” ",
    "createdAt": "2026-02-18T11:10:00Z",
    "game": "MLBB"
  },
   {
    "id": 5,
    "message": "dadwted",
    "detail" : "Your post “หาคนเล่นทุกตำแหน่งครับ ผมเมนป่า ตอนนี้” ",
    "createdAt": "2026-02-16T18:10:00Z",
    "game": "MLBB"
  }
];


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

notice_mock.forEach(notice => {
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
    detailchat.innerText = notice.detail;
    const detailgame = document.createElement("p")
    detailchat.classList.add("detail-text");
    detailgame.classList.add("detail-game");
    detailgame.innerText = notice.game
    detailbox.appendChild(detailchat);
    detailbox.appendChild(detailgame)
    
    noticedev.appendChild(messagebox);
    noticedev.appendChild(detailbox);
    noticeDev.appendChild(noticedev);
});

if (noticeDev) {
    noticeContainer.appendChild(noticeDev);
}




