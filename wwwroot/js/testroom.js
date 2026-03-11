const PlayerMain = document.getElementById("roomContainer");

let offset = 2;

const visibleSlots = 5;
const centerIndex = 2;
let rooms = [];
let queue = [];
let connection = null;
let chat_list = [];

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

        const chatBody = document.querySelector(".chat-body");

        if(chatBody){

            const empty = chatBody.querySelector(".chat-empty");
            if(empty) empty.remove();

            const messageElement = CreateChatMessage(new_message);
            chatBody.appendChild(messageElement);

            chatBody.scrollTop = chatBody.scrollHeight;
        }
    });

}
async function init() {

    try{
        showLoading();
        connection = new signalR.HubConnectionBuilder()
            .withUrl("/chathub", { withCredentials:true })
            .build();

        registerChatEvents();

        await connection.start();

        console.log("SignalR connected");

        await connection.invoke("JoinRoom", roomId);

        await reloadRooms();
        await reloadQueue();

        const chatRes = await fetch(`/game/${gameName}/room/${roomId}/chat`);
        const chatHistory = await chatRes.json();

        chat_list = chatHistory;
        StartBtn()
        LeaveBtn()
        ReadyBtn()
        RenderChatHistory();
        
    }catch(err){
        console.error("INIT ERROR:", err);
        
    }
      finally {

        hideLoading();

  }
}


function renderRooms(room) {

    PlayerMain.innerHTML = "";
    
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

// chat
const chatbox = CreateChatBox();
PlayerMain.appendChild(chatbox);

RenderChatHistory();

const sent_box = CreateSentBox();
PlayerMain.appendChild(sent_box);
}


// function renderRooms(room) {

//     PlayerMain.innerHTML = "";

//     const slots = new Array(visibleSlots).fill(null);

//     const owner = room.players.find(p => p.username === room.ownerUsername);
//     const others = room.players.filter(p => p.username !== room.ownerUsername);

//     const emptySlots = room.roomSetting.maxPlayer - room.players.length;


//     const circle = [...others];

//     for(let i=0;i<emptySlots;i++){
//         circle.push(null);
//     }

//     const total = circle.length;

//     // ⭐ แก้ตรงนี้
//     offset = ((offset % total) + total) % total;

//     slots[centerIndex] = owner;

// let currentleft = centerIndex - 1;
// let currentright = centerIndex + 1;

// let Isleft = true;

// for(let i = 0; i < circle.length; i++){

//     const c = circle[(offset + i) % total];

//     if(Isleft){

//         if(currentleft >= 0){
//             slots[currentleft] = c;
//             currentleft--;
//         }

//     }else{

//         if(currentright < visibleSlots){
//             slots[currentright] = c;
//             currentright++;
//         }

//     }

//     Isleft = !Isleft;

// }
//     console.log(slots)
// slots.forEach(p => {
//     if(p){
//         PlayerMain.appendChild(PlayerCard(p, room.ownerUsername));
//     }else{
//         PlayerMain.appendChild(EmptySlot());
//     }
// });

// // chat
// const chatbox = CreateChatBox();
// PlayerMain.appendChild(chatbox);

// RenderChatHistory();

// const sent_box = CreateSentBox();
// PlayerMain.appendChild(sent_box);
// }

function PlayerCard(player, ownerUsername) {

    const div = document.createElement("div");
    div.classList.add("player-main");

    const card = document.createElement("div");
    card.classList.add("card");

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

    const chatBody = document.querySelector(".chat-body");

    if(!chatBody) return;

    chatBody.innerHTML = "";

    if(chat_list.length === 0){

        const empty = document.createElement("p");
        empty.classList.add("chat-empty");
        empty.innerText = "No messages yet";

        chatBody.appendChild(empty);
        return;
    }

    chat_list.forEach(msg => {
        const messageElement = CreateChatMessage(msg);
        chatBody.appendChild(messageElement);
    });

    chatBody.scrollTop = chatBody.scrollHeight;
}
function CreateChatMessage(chat){

    const current_chat = document.createElement("div");
    current_chat.classList.add("chat-format");

    const avatar = document.createElement("div");
    avatar.classList.add("chat-avatar");

    avatar.style.backgroundImage = `url(${chat.avatar})`;
    avatar.style.backgroundSize = "cover";
    avatar.style.backgroundPosition = "center";

    const message = document.createElement("p");
    message.classList.add("chat-message");
    message.innerText = `${chat.sender}: ${chat.message}`;

    current_chat.appendChild(avatar);
    current_chat.appendChild(message);

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

    const label = document.createElement("span");
    label.classList.add("chat-label");
    label.innerText = "Party:";

    const input = document.createElement("input");
    input.placeholder = "Send message...";
    input.classList.add("chat-input");

    const button = document.createElement("button");
    button.type = "submit";
    button.innerText = "Send";
    button.classList.add("chat-send");

    sent_box.addEventListener("submit", function(e){

        e.preventDefault();

        if(input.value.trim() === "") return;

        connection.invoke("SendMessage", roomId, input.value);

        input.value = "";
    });

    sent_box.appendChild(label);
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

    const queuebutton = document.createElement("button");
    queuebutton.innerText = "QUEUE";
    queuebutton.classList.add("queue-btn");

    queueBox.appendChild(queuebutton);

    queuebutton.addEventListener("click", () => {
        queueList.classList.toggle("show-queue");
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

init();