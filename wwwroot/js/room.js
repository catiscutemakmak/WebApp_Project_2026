const room_player = { 
        game: "mlbb",
        room_name: "Hello MLBB",
        room_id: "1001",
        maximum_player : 5,
        owner : "Sky",
        player: [
            {
                name: "Sky",
                player_rank: "Mythic",
                role: "Mage",
                status: "Ready",
                profile: "https://tx.audubon.org/sites/default/files/styles/bean_wysiwyg_full_width/public/cbcpressroom_tuftedtitmouse-judyhowle.jpg?itok=VMtDnqip",
                
            },
            {
                name: "Dome",
                player_rank: "Legend",
                role: "Jungle",
                status: "Ready",
                profile: "https://s.france24.com/media/display/544355b0-45df-11f0-9098-005056a97e36/w:980/Part-GTY-GYI0061951038-1-1-0.jpg"
            },
            {
              name: "Bow",
              player_rank: "Grandmaster",
              role: "EXP",
              status: "Not Ready",
              profile: "https://i2.wp.com/images.genshin-builds.com/genshin/characters/flins/image.png?strip=all&quality=100"
            }
        ],
                room_queue : [
            {
                name:"Egg",
                player_rank:"Legend",
                role:"Gold",
                profile:"https://play-lh.googleusercontent.com/PCpXdqvUWfCW1mXhH1Y_98yBpgsWxuTSTofy3NGMo9yBTATDyzVkqU580bfSln50bFU=w600-h300-pc0xffffff-pd"
            },
            {
                name:"Egga",
                player_rank:"Mythic",
                role:"Roam",
                profile:"https://s.france24.com/media/display/544355b0-45df-11f0-9098-005056a97e36/w:980/Part-GTY-GYI0061951038-1-1-0.jpg"
            }
        ]
    }

const chat_list = [
  {
    sender: "Bow",
    avatar: "https://i2.wp.com/images.genshin-builds.com/genshin/characters/flins/image.png?strip=all&quality=100",
    message: "Hello Everyone!",
  },
  {
    sender: "Sky",
    avatar: "https://tx.audubon.org/sites/default/files/styles/bean_wysiwyg_full_width/public/cbcpressroom_tuftedtitmouse-judyhowle.jpg?itok=VMtDnqip",
    message: "Hi Bow",
  },
  {
    sender: "Dome",
    avatar: "https://s.france24.com/media/display/544355b0-45df-11f0-9098-005056a97e36/w:980/Part-GTY-GYI0061951038-1-1-0.jpg",
    message: "Lets play MLBB Bro!!!",
  },
  {
    sender: "Dome",
    avatar: "https://s.france24.com/media/display/544355b0-45df-11f0-9098-005056a97e36/w:980/Part-GTY-GYI0061951038-1-1-0.jpg",
    message: "Hello ans me TT",
  },
    {
    sender: "Bow",
    avatar: "https://i2.wp.com/images.genshin-builds.com/genshin/characters/flins/image.png?strip=all&quality=100",
    message: "U all noob bye bye",
  }
];

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
    playerRoom.classList.add("player-room");

    room.player.forEach(p => {
        playerRoom.appendChild(PlayerCard(p, room.owner));
    });

    const emptyCount = room.maximum_player - room.player.length;
    
    for (let i = 0; i < emptyCount; i++) {
        playerRoom.appendChild(EmptySlot());
    }

    const chatbox = CreateChatBox();
    playerRoom.appendChild(chatbox);

    const sent_box = CreateSentBox();
    playerRoom.appendChild(sent_box)
    
    renderQueue(room.room_queue);
    return playerRoom;
}

function PlayerCard(player, owner) {
    const div = document.createElement("div");
    div.classList.add("player-dev");

    div.innerHTML = `
        ${player.profile != "None"
          ? `<img class="player-profile" src="${player.profile}">`
          : `<img class="player-profile"
              src="https://as1.ftcdn.net/jpg/02/57/42/72/1000_F_257427286_Lp7c9XdPnvN46TyFKqUaZpPADJ77ZzUk.jpg">`
        }
        ${player.name === owner 
        ? "<span class='empty-crown'>👑</span>" 
        : "<span class='empty-crown'>🎮</span>"}
        
        <p class="${player.status === "Ready" ? "yellow" : ""}">
        ${player.name}
        </p>
        

        <img class="rankImg"
             src="${rankImageMap[player.player_rank] || "/images/rank/default.png"}">

        <p class="rank-p">${player.role}</p>
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

    const new_message = {
    sender: user.name,
    avatar: user.avatar,
    message: input.value,

  }
    chat_list.push(new_message); //place holder//
    input.value = "";
    });
    sent_box.appendChild(input);
    sent_box.appendChild(button);

    return sent_box;
}

function renderQueue(queue) {
    const queueBox = document.getElementById("queueBox");
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
        imgDiv.src = p.profile;

        const roleDiv = document.createElement("p");
        roleDiv.classList.add("queue-role");
        roleDiv.textContent = `${p.player_rank}/${p.role}`;

        // 🔹 สร้างปุ่มรับ
        const BtnDiv = document.createElement("div");
        BtnDiv.classList.add("queue-btn-div");
        const acceptBtn = document.createElement("button");
        acceptBtn.innerHTML = "✓";
        acceptBtn.classList.add("queue-circle-btn", "accept-btn");
        acceptBtn.addEventListener("click", () => {
            console.log("Accepted:", p.name);
        });

        // 🔹 สร้างปุ่มปฏิเสธ
        const rejectBtn = document.createElement("button");
        rejectBtn.innerHTML = "✕";
        rejectBtn.classList.add("queue-circle-btn", "reject-btn");
        rejectBtn.addEventListener("click", () => {
            console.log("Rejected:", p.name);
        });

        BtnDiv.appendChild(acceptBtn);
        BtnDiv.appendChild(rejectBtn);

        const botdiv = document.createElement("div");
        botdiv.classList.add("queue-bot");

        const nameDiv = document.createElement("p");
        nameDiv.classList.add("queue-name");
        nameDiv.textContent = p.name;
        topdiv.appendChild(imgDiv);
        topdiv.appendChild(nameDiv);
        botdiv.appendChild(roleDiv);
        botdiv.appendChild(BtnDiv);
        div.appendChild(topdiv);
        div.appendChild(botdiv);

        queueList.appendChild(div);
        queueBox.appendChild(queueList);
        DragQueue()
    });
}

function DragQueue(){
const box = document.querySelector(".queueBox");

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
const roomId = "@Model";
renderRooms(room_player);

