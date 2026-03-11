const PlayerMain = document.getElementById("roomContainer");

let offset = 2;

const visibleSlots = 5;
const centerIndex = 2;
let rooms = [];
let queue = [];
let chat_list = [];
let isQueueOpen = false;
let kickMode = false;
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

async function init() {

    try{
        showLoading();
        
        await reloadRooms();
        await reloadQueue();

        StartBtn()
        LeaveBtn()
        ReadyBtn()
        document.getElementById("kickbtn").addEventListener("click", kickbtn);

        
    const chatRes = await fetch(`/api/chat/${roomId}`);
    const chatHistory = await chatRes.json();

    chat_list = chatHistory.map(m => ({
    sender: m.username,
    avatar: m.avatar,
    message: m.message,
    sentAt: m.sentAt
    }));
    
     InitChat()
    RenderChatHistory();

    setInterval(fetchChatMessages,1000);
    setInterval(reloadQueue,5000);
    setInterval(reloadRooms,5000);

    }catch(err){
        console.error("INIT ERROR:", err);
        
    }
      finally {

        hideLoading();

  }
}


function renderRooms(room) {

    PlayerMain.innerHTML = "";
    
    const statusContainer = document.getElementById("statusContainer");

    statusContainer.innerHTML = "";

    const status = document.createElement("div");

    status.classList.add("room-status","cyber-status");
    status.classList.add(getStatusClass(room.roomStatus));

    status.innerText = getStatusText(room.roomStatus);

    statusContainer.appendChild(status);

const slotCount = Math.min(room.roomSetting.maxPlayer, visibleSlots);

const slots = new Array(slotCount).fill(null);

const centerIndex = Math.floor((slotCount - 1) / 2);

const owner = room.players.find(p => p.username === room.ownerUsername);
const others = room.players.filter(p => p.username !== room.ownerUsername);

const emptySlots = room.roomSetting.maxPlayer - room.players.length;

const circle = [...others];

for(let i=0;i<emptySlots;i++){
    circle.push(null);
}

const total = circle.length;

offset = ((offset % total) + total) % total;

slots[centerIndex] = owner;

for(let i=1;i<slotCount;i++){

    const leftSlot = centerIndex - i;
    const rightSlot = centerIndex + i;

    if(leftSlot >= 0){
        const leftIndex = (offset - i + total) % total;
        slots[leftSlot] = circle[leftIndex];
    }

    if(rightSlot < slotCount){
        const rightIndex = (offset + i - 1) % total;
        slots[rightSlot] = circle[rightIndex];
    }

}

slots.forEach(p => {
    if(p){
        PlayerMain.appendChild(PlayerCard(p, room.ownerUsername));
    }else{
        PlayerMain.appendChild(EmptySlot());
    }
});

}

function getStatusText(status){

switch(status){

case 0:
return "ROOM STATUS : WAITING";

case 1:
return "ROOM STATUS : FULL";


case 2:
return "ROOM STATUS : MATCH STARTING";

case 3:
return "ROOM STATUS : ROOM CLOSED";

case 4:
return "ROOM STATUS : ROOM DELETED";
}

}
function getStatusClass(status){
    switch(status){
        case 0: return "waiting";
        case 1: return "full";
        case 2: return "starting";
        case 3: return "close";
        case 4: return "delete";
    }
}

function InitChat(){
    const ChatMain = document.getElementById("chatContainer")
    const chatbox = CreateChatBox();
    ChatMain.appendChild(chatbox);

    const sent_box = CreateSentBox();
    ChatMain.appendChild(sent_box);

    RenderChatHistory();

}

function PlayerCard(player, ownerUsername) {

    const div = document.createElement("div");
    div.classList.add("player-main");

    const card = document.createElement("div");
    card.classList.add("card");

    card.dataset.playerId = player.userId;;

    card.addEventListener("click", async () => {

        if(!kickMode) return;

        const playerId = card.dataset.playerId;

        try{
            const res = await fetch(`/game/${gameName}/room/${roomId}/kick/${playerId}`,{
                method:"DELETE"
            });

            if(!res.ok){
                const err = await res.text();
                alert(err);
                return;
            }

            reloadRooms();

        }catch(err){
            console.error(err);
        }

    });
    if(player.status) {
    card.classList.add("gold")
    }
    const rank = document.createElement("img");
    if (gameName == "Among Us" || gameName == "Peak"){
        rank.src = player.avatar    
    }
    else {
    rank.src = player.rankName;
    
    }
    rank.classList.add("rank-badge");



    if(player.username === ownerUsername){
        card.classList.add("owner");
    }

    const img = document.createElement("img");
    img.src = player.userProfile ?? '/images/default-profile.png';

    img.onerror = () => {
        img.src = '/images/default-profile.png';
    };
    const name = document.createElement("div");
    name.classList.add("player-name");
    name.innerText = player.username;

    const title = document.createElement("div");
    title.classList.add("player-title");
    title.innerText = player.roleName;

    
    card.appendChild(img);
    card.appendChild(name);
    card.appendChild(title);

    div.appendChild(card);
    div.appendChild(rank);

    return div;
}

function EmptySlot(){

    const div = document.createElement("div");
    div.classList.add("slot");
    div.innerText = "+";

    return div;
}

document.getElementById("leftBtn").onclick = () => {

    offset--;

    renderRooms(rooms);

};

document.getElementById("rightBtn").onclick = () => {

    offset++;

    renderRooms(rooms);

};
function RenderChatHistory() {
    const chatBox = document.querySelector(".chat-body");
    if(!chatBox) return;

    chatBox.innerHTML = "";

    chat_list.forEach((msg, index) => {


        const messageElement = CreateChatMessage(msg);

        chatBox.appendChild(messageElement);

    });

    setTimeout(() => {
        chatBox.scrollTop = chatBox.scrollHeight;
    }, 0);
}

async function fetchChatMessages(){

    try{

        const res = await fetch(`/api/chat/${roomId}`);
        if(!res.ok) return;

        const messages = await res.json();

        const chatBox = document.querySelector(".chat-body");
        if(!chatBox) return;

        let hasNewMessage = false;

        messages.forEach(m => {

            const alreadyExists = chat_list.some(c => 
                c.sentAt === m.sentAt
            );

            if(!alreadyExists){

                const msg = {
                    sender: m.username,
                    avatar: m.avatar,
                    message: m.message,
                    sentAt: m.sentAt
                };

                

                chat_list.push(msg);

                const prev = chat_list[chat_list.length - 2];

                const messageElement = CreateChatMessage(msg, prev);
                chatBox.appendChild(messageElement);

                hasNewMessage = true;

            }

        });

        if(hasNewMessage){
            chatBox.scrollTop = chatBox.scrollHeight;
        }

    }catch(err){
        console.error("chat polling error:",err);
    }

}

function CreateChatMessage(chat){

    const current_chat = document.createElement("div");
    current_chat.classList.add("chat-format");

    const avatar = document.createElement("img");
    avatar.classList.add("chat-avatar");

    avatar.src = chat.avatar || "/images/default-profile.png";

    avatar.onerror = () => {
        avatar.src = "/images/default-profile.png";
    };

    current_chat.appendChild(avatar);
    const sender = document.createElement("p");
    sender.classList.add("chat-sender");
    sender.innerText = chat.sender + ": " + chat.message;

    current_chat.appendChild(sender);

    const textBox = document.createElement("div");
    textBox.classList.add("chat-textbox");

    current_chat.appendChild(textBox);

    return current_chat;
}
function CreateChatBox() {

    const chatdiv = document.createElement("div");
    chatdiv.classList.add("chat-dev");

    const header = document.createElement("div");
    header.classList.add("chat-header");
    header.innerText = "Chat Party";

    const chatBody = document.createElement("div");
    chatBody.classList.add("chat-body");

    const empty = document.createElement("p");
    empty.classList.add("chat-empty");
    empty.innerText = "No messages yet";

    chatBody.appendChild(empty);

    chatdiv.appendChild(header);
    chatdiv.appendChild(chatBody);

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
    sent_box.addEventListener("submit", async function(e) {

        e.preventDefault();

        if(input.value.trim() === "") return;

        const message = input.value;

        try {

            await fetch("/api/chat/send",{
                method:"POST",
                headers:{
                    "Content-Type":"application/json"
                },
                body:JSON.stringify({
                    roomId: roomId,
                    message: message
                })
            });

        } catch(err){
            console.error("send message error:",err);
        }

        input.value = "";
    });

    sent_box.appendChild(input);
    sent_box.appendChild(button);

    return sent_box;
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

function ReadyBtn(){
const startBtn = document.getElementById("Readybtn");

startBtn.addEventListener("click", async () => {

    try {

        const res = await fetch(`/game/${gameName}/room/${roomId}/ready`, {
            method: "PUT"
        });

        if (!res.ok) {

            const error = await res.text();
            alert(res.status);
            return;
        }

        alert("PlayerReady");

    } catch (err) {
        console.error(err);
        alert(err)
    }

});
}

function renderQueue(queue) {

    const queueBox = document.getElementById("queueBox");
    queueBox.innerHTML = "";

    const queueList = document.createElement("div");
    queueList.classList.add("queue-list");
    if(isQueueOpen){
        queueList.classList.add("show-queue");
    }
    const queuebutton = document.createElement("button");
    queuebutton.innerText = "QUEUE";
    queuebutton.classList.add("queue-btn");

    queueBox.appendChild(queuebutton);

    queuebutton.addEventListener("click", () => {
    isQueueOpen = !isQueueOpen;
    queueList.classList.toggle("show-queue", isQueueOpen);
    });

    // ⭐ ถ้าไม่มีคนใน queue
    if (!queue || queue.length === 0) {

        const empty = document.createElement("p");
        empty.textContent = "No Queue";
        empty.classList.add("queue-empty");

        queueList.appendChild(empty);

    } else {

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
            roleDiv.textContent = `${p.rankName || "-"}/${p.roleName || "-"}`;

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
            reloadQueue()
            reloadRooms()
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
            reloadQueue()
            reloadRooms()
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
    }

    queueBox.appendChild(queueList);
}
function showLoading(){
  document.body.classList.add("loading");
}

function hideLoading(){
  document.body.classList.remove("loading");
}


function kickbtn(){

    const container = document.getElementById("roomContainer");

    kickMode = !kickMode;

    container.classList.toggle("kick-mode");

}

init();
///