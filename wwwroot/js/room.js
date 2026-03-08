let connection = null
let connection_room = null
let connection_queue = null
let rooms = [];
let queue = [];


async function reloadRooms() {
  try {
    const res = await fetch(`/game/${gameName}/room/${roomId}/details`);

    if (!res.ok) {
      throw new Error("Reload rooms failed");
    }

    rooms = await res.json();
    console.log(rooms)
    renderRooms(rooms);

  } catch (err) {
    console.error("Reload rooms error:", err);
  }
}

async function reloadQueue() {
  try {

    const res = await fetch(`/api/rooms/${roomId}/queue`);

    if (!res.ok) {
      throw new Error("Reload queue failed");
    }

    queue = await res.json();
    console.log(queue)
    renderQueue(queue);
    
  } catch (err) {
    console.error("Reload queue error:", err);
  }
}


function registerChatEvents(){

    connection.on("ReceiveMessage", function(username, avatar, message){

        const new_message = {
            sender: username,
            avatar: avatar,
            message: message
        };

        chat_list.push(new_message);

        const chatBox = document.querySelector(".chat-dev");

        if(chatBox){
            const messageElement = CreateChatMessage(new_message);
            chatBox.appendChild(messageElement);

            chatBox.scrollTop = chatBox.scrollHeight;
        }
    });

}

function registerRoomEvents(){

    connection_room.on("RoomCreated", async (updatedGameName) => {

        if (updatedGameName !== gameName) return;

        await reloadRooms();
    });

    connection_room.on("PlayerJoinedRoom", async (updatedGameName) => {

        if (updatedGameName !== gameName) return;

        await reloadRooms();
    });

}

function registerQueueEvent(){
    connection_queue.on("QueueUpdated", async (updateroomId) => {

        if (updateroomId != roomId) return;

        await reloadQueue();
        await reloadRooms();

    });

}

async function init() {

  try {

    connection = new signalR.HubConnectionBuilder()
      .withUrl("/chathub")
      .withAutomaticReconnect()
      .build();

    connection_room = new signalR.HubConnectionBuilder()
      .withUrl("/roomhub")
      .withAutomaticReconnect()
      .build();

    connection_queue = new signalR.HubConnectionBuilder()
    .withUrl("/roomhub")
    .withAutomaticReconnect()
    .build();
    registerChatEvents();
    registerRoomEvents();
    registerQueueEvent();

    await connection.start();
    await connection_room.start();
    await connection_queue.start();
    // join chat room
    await connection.invoke("JoinRoom", roomId);

    // join realtime room group
    await connection_room.invoke("JoinGameGroup", gameName);

    // queue accept,reject
    await connection_queue.invoke("AcceptRejectQueue",roomId)
    await reloadQueue();
    await reloadRooms();
    const chatRes = await fetch(`/game/${gameName}/room/${roomId}/chat`);
chat_list = await chatRes.json();
RenderChatHistory();
    StartBtn()
    LeaveBtn()

  } catch (err) {
    console.error(err);
  }
}

init();

const chat_list = [];

const rankImageMap = {
    Warrior: "/images/rank/warrior.webp",
    Elite: "/images/rank/elite.webp",
    Master: "/images/rank/master.webp",
    Grandmaster: "/images/rank/grandmaster.webp",
    Epic: "/images/rank/epic.webp",
    Legend: "/images/rank/legend.webp",
    Mythic: "/images/rank/mythic.webp",
};

const gamerole = {
  mlbb : ["Mage", "Jungle", "EXP", "Gold", "Support"]
}

const user = {
    name:"Bow",
    avatar: "https://i2.wp.com/images.genshin-builds.com/genshin/characters/flins/image.png?strip=all&quality=100"
}

function renderRooms(room) {

    const playerRoom = document.getElementById("roomContainer");
    playerRoom.innerHTML = "";
    playerRoom.classList.add("player-room");

    // render players
    room.players.forEach(p => {
        playerRoom.appendChild(PlayerCard(p, room.ownerId));
    });

    // calculate empty slot
    const maxPlayer = room.roomSetting.maxPlayer;
    const currentPlayer = room.players.length;
    const emptyCount = maxPlayer - currentPlayer;

    for (let i = 0; i < emptyCount; i++) {
        playerRoom.appendChild(EmptySlot());
    }

    // chat
    const chatbox = CreateChatBox();
    playerRoom.appendChild(chatbox);

    const sent_box = CreateSentBox();
    playerRoom.appendChild(sent_box);

    // queue
    const queueBox = document.getElementById("queueBox");
    
    renderQueue(queue);
    
    
    return playerRoom;
}

function PlayerCard(player, OwnerId) {

    const div = document.createElement("div");
    div.classList.add("player-dev");

    div.style.cursor = "pointer";
    div.addEventListener("click", () => {
    window.location.href = `/Profile/View/${player.userId}`;
    });

    div.innerHTML = `
        <img class="player-profile"
             src="${player.userProfile ?? 'https://as1.ftcdn.net/jpg/02/57/42/72/1000_F_257427286_Lp7c9XdPnvN46TyFKqUaZpPADJ77ZzUk.jpg'}">

        ${player.userId === OwnerId
        ? "<span class='empty-crown'>👑</span>"
        : "<span class='empty-crown'>🎮</span>"}

        <p class="${player.status === "Ready" ? "yellow" : "white"}">
            ${player.username}
        </p>

        <img class="rankImg"
             src="${player.rankName ?? "/images/rank/default.png"}">

        <p class="rank-p">${player.roleName}</p>
    `;

    return div;
}

function EmptySlot() {
    const div = document.createElement("div");
    div.innerHTML = `<span class="empty-crown">/ᐠ • ˕ •マ ?</span>`;
    div.classList.add("empty-dev");
    return div;
}

function CreateChatBox() {
    const chatdiv = document.createElement("div");
    chatdiv.classList.add("chat-dev");
    let user_before = null;

    chat_list.forEach(chat => {

    const current_chat = document.createElement("div");
    current_chat.classList.add("chat-format");

    const avatar = document.createElement("div");
    avatar.classList.add("chat-avatar");

    if (user_before !== chat.sender) {
        avatar.style.backgroundImage = `url(${chat.avatar})`;
        avatar.style.backgroundSize = "cover";
        avatar.style.backgroundPosition = "center";
    } else {
        avatar.classList.add("avatar-hidden");
    }

    current_chat.appendChild(avatar);

    const textBox = document.createElement("div");
    textBox.classList.add("chat-textbox");

    const message = document.createElement("p");
    message.classList.add("chat-message");
    message.innerText = chat.message;

    textBox.appendChild(message);
    current_chat.appendChild(textBox);

    chatdiv.appendChild(current_chat);

    user_before = chat.sender;
});

    return chatdiv;
}

function CreateSentBox() {
    const sent_box = document.createElement("form");
    sent_box.classList.add("sent-box");


    const input = document.createElement("input");
    input.placeholder = "Let's greet your teammate";
    input.classList.add("chat-input");

    const button = document.createElement("button");
    button.type = "submit";
    button.classList.add("chat-send-btn");
    button.innerHTML = `
<svg width="20" height="20" viewBox="0 0 32 32" fill="none">
<path d="M4.95 4.06L28.95 16.6L4.95 27.93L8 16L4.95 4.06Z"
stroke="#EB55FF" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
</svg>
`;
    sent_box.addEventListener("submit", function(e) {

        e.preventDefault();

        if(input.value.trim() === "") return;

        connection.invoke(
            "SendMessage",
            roomId,
            user.name,
            user.avatar,
            input.value
        ).catch(err => console.error(err));

        input.value = "";
    });

    sent_box.appendChild(input);
    sent_box.appendChild(button);

    return sent_box;
}

function renderQueue(queue) {

    const queueBox = document.getElementById("queueBox");
    queueBox.innerHTML = "";

    
    const queueList = document.createElement("div");
    queueList.classList.add("queue-list");

    const queuebutton = document.createElement("button");
    queuebutton.innerText = "QUEUE";
    queuebutton.classList.add("queue-btn");

    queueBox.appendChild(queuebutton);

    queuebutton.addEventListener("click", () => {
        queueList.classList.toggle("show-queue");
    });

    queue.forEach(p => {

        const div = document.createElement("div");
        div.classList.add("queue-div");

        const topdiv = document.createElement("div");
        topdiv.classList.add("queue-top");

        const imgDiv = document.createElement("img");
        imgDiv.classList.add("queue-profile");
        imgDiv.src = p.profileImagePath;

        const nameDiv = document.createElement("p");
        nameDiv.classList.add("queue-name");
        nameDiv.textContent = p.nickname;

        topdiv.appendChild(imgDiv);
        topdiv.appendChild(nameDiv);

        const botdiv = document.createElement("div");
        botdiv.classList.add("queue-bot");

        const roleDiv = document.createElement("p");
        roleDiv.classList.add("queue-role");
        roleDiv.textContent = `${p.rankName}/${p.roleName}`;

        botdiv.appendChild(roleDiv);

       // check Owner
        if (rooms.isOwner) {

            const BtnDiv = document.createElement("div");
            BtnDiv.classList.add("queue-btn-div");

            const acceptBtn = document.createElement("button");
            acceptBtn.innerHTML = "✓";
            acceptBtn.classList.add("queue-circle-btn", "accept-btn");

            acceptBtn.addEventListener("click", async () => {
        try {

        const response = await fetch(
            `/api/rooms/${roomId}/accept/${p.id}`,
            {
            method: "PUT"
            }
        );

        if (response.ok) {

            const data = await response.json();

        } else {

            const errorText = await response.text();
            alert(errorText);

        }

        } catch (err) {
        console.error(err);
        }

    });
            const rejectBtn = document.createElement("button");
            rejectBtn.innerHTML = "✕";
            rejectBtn.classList.add("queue-circle-btn", "reject-btn");
            rejectBtn.addEventListener("click", async () => {
        try {

        const response = await fetch(
            `/api/rooms/${roomId}/reject/${p.id}`,
            {
            method: "PUT"
            }
        );

        if (response.ok) {

            const data = await response.json();

        } else {

            const errorText = await response.text();
            alert(errorText);

        }

        } catch (err) {
        console.error(err);
        }

    });

            BtnDiv.appendChild(acceptBtn);
            BtnDiv.appendChild(rejectBtn);

            botdiv.appendChild(BtnDiv);
        }

        div.appendChild(topdiv);
        div.appendChild(botdiv);

        queueList.appendChild(div);
    });

    queueBox.appendChild(queueList);

    DragQueue();
}

function DragQueue(){
const box = document.getElementById("queueBox");

box.style.position = "absolute";

box.addEventListener("mousedown", function(e){
    let shiftX = e.clientX - box.getBoundingClientRect().left;
    let shiftY = e.clientY - box.getBoundingClientRect().top;

    function moveAt(pageX, pageY) {
        box.style.left = pageX - shiftX + "px";
        box.style.top = pageY - shiftY + "px";
    }

    function onMouseMove(e) {
        moveAt(e.pageX, e.pageY);
    }

    document.addEventListener("mousemove", onMouseMove);

    document.addEventListener("mouseup", function(){
        document.removeEventListener("mousemove", onMouseMove);
    }, { once: true });
});
}

function StartBtn(){
const startBtn = document.getElementById("Startbtn");

startBtn.addEventListener("click", async () => {

    try {

        const res = await fetch(`/game/${gameName}/room/${roomId}/start`, {
            method: "PUT"
        });

        if (!res.ok) {

            const error = await res.text();
            alert(error);
            return;
        }

        alert("Game starting!");

    } catch (err) {
        console.error(err);
    }

});
}
function LeaveBtn(){

const leaveBtn = document.getElementById("Leavebtn");

leaveBtn.onclick = async () => {

    try {

        const res = await fetch(`/game/${gameName}/room/${roomId}/leave`, {
            method: "PUT"
        });

        if (!res.ok) {

            const error = await res.text();
            alert(error);
            return;
        }

        window.location.href = `/game/${gameName}`;

    } catch (err) {
        console.error(err);
    }

};
}


