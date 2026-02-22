const mock_room = [
    { 
        game: "mlbb",
        room_name: "Hello MLBB",
        room_id: "1001",
        maximum_player : 5,
        owner : "Sky",
        settings: {
    mode: "5v5 Classic",
    min_rank: "Epic",
    max_rank: "Mythic",
    mic_required: true,
    room_type: "Public Room",
    Server: "NA"
  },
        player: [
            {
                name: "Sky",
                player_rank: "Mythic",
                role: "Mage",
                profile: "https://tx.audubon.org/sites/default/files/styles/bean_wysiwyg_full_width/public/cbcpressroom_tuftedtitmouse-judyhowle.jpg?itok=VMtDnqip"
            },
            {
                name: "Dome",
                player_rank: "Legend",
                role: "Jungle",
                profile: "https://s.france24.com/media/display/544355b0-45df-11f0-9098-005056a97e36/w:980/Part-GTY-GYI0061951038-1-1-0.jpg"
            },
            {
              name: "Bow",
              player_rank: "Grandmaster",
              role: "EXP",
              profile: "https://i2.wp.com/images.genshin-builds.com/genshin/characters/flins/image.png?strip=all&quality=100"
            }
        ]
    }
    ,
        {
        game: "mlbb",
        room_name: "Cambodia",
        room_id: "1002",
        maximum_player : 5,
        owner : "Skyyy",
        settings: {
    mode: "5v5 Rank",
    min_rank: "Epic",
    max_rank: "Mythic",
    mic_required: false,
    room_type: "Private Room",
    Server: "EU"
  },
        player: [
            {
                name: "Skyyy",
                player_rank: "Grandmaster",
                role: "Jungle",
                profile: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4SJO-ldC0ch4jN9kkMGl0gFVU-aRyxwmqew&s"
            },
            {
                name: "Domeeee",
                player_rank: "Grandmaster",
                role: "Mage",
                profile: "https://i2.wp.com/images.genshin-builds.com/genshin/characters/skirk/image.png?strip=all&quality=100"
            }
        ]  
    }
    ,
        {
        game: "mlbb",
        room_name: "Rank 5vs5",
        room_id: "1003",
        maximum_player : 5,
        owner : "IChi",
        settings: {
    mode: "5v5 Rank",
    min_rank: "Legend",
    max_rank: "Mythic",
    mic_required: true,
    room_type: "Public Room",
    Server: "Asia"
  },
        player: [
            {
                name: "IChi",
                player_rank: "Elite",
                role: "Mage",
                profile: "https://cdn.cdnstep.com/icd3luCKn9ladLlQ2pF1/7.webp"
            },
            {
                name: "Coptu",
                player_rank: "Epic",
                role: "Jungle",
                profile: "https://cdn.cdnstep.com/tHZASvXmWJbuYct7FBFU/18.png"
            },
            {
              name: "Bowy",
              player_rank: "Master",
              role: "EXP",
              profile: "https://api.fstik.app/file/AAMCBQADFQABaX1YV6D0-TQrs-uGZWztWXPj1wcAAs8AAxwGKw1xMMz2_6hN_gEAB20AAzgE/sticker.webp"
            },
            {
              name: "Sky92",
              player_rank: "Warrior",
              role: "Gold",
              profile: "https://cdn.cdnstep.com/Ser2JXg90h9DFB1Og6pb/1.png"
            }
        ]
    }
]

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
function renderRooms(rooms) {
    const roomContainer = document.getElementById("MatchContainer");
    

    rooms.forEach(room => {
        roomContainer.appendChild(renderRoom(room));
    });
}
function renderRoom(room) {
    const playerRoom = document.createElement("div");
    playerRoom.classList.add("player-room");
    const roomName = document.createElement("div");
    roomName.classList.add("room-name");
    roomName.innerText = room.room_name;

    playerRoom.appendChild(roomName);
    room.player.forEach(player => {
        playerRoom.appendChild(PlayerCard(player, room.owner));
    });

    const emptyCount = room.maximum_player - room.player.length;
    const roleForm = createRoleForm(room);
    roleForm.classList.add("role-form","hide");
    
    const joinButton = createJoinButton();

    
    for (let i = 0; i < emptyCount; i++) {
        playerRoom.appendChild(EmptySlot(roleForm,joinButton));
    }

    const required_bar = createRequirmentBar(room);
    playerRoom.appendChild(required_bar);
    playerRoom.appendChild(roleForm);
    playerRoom.appendChild(joinButton);
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

        <p>${player.name}</p>

        <img class="rankImg"
             src="${rankImageMap[player.player_rank] || "/images/rank/default.png"}">

        <p class="rank-p">${player.role}</p>
    `;

    return div;
}
function EmptySlot(join_dev,joinButton) {
    const btn = document.createElement("button");
    btn.type = "button"; 
    btn.classList.add("empty-dev");

    btn.addEventListener("click", () => {
        ShowJoin(join_dev);
        ShowJoin(joinButton);
    });

    btn.innerHTML = `<span class="empty-crown">/ᐠ • ˕ •マ ?</span>`;

    return btn;
}

function ShowJoin(join_dev){
    join_dev.classList.toggle("hide");
}

function createRoleForm(room) {
    const roles = gamerole[room.game];

    const takenRoles = room.player.map(p => p.role);
    const form = document.createElement("form");
    form.classList.add("role-form", "hide");
    
    roles.forEach(role => {
        
        const label = document.createElement("label");

        const input = document.createElement("input");
        input.type = "radio";
        input.name = `role-${room.room_id}`;
        input.value = role;

        const span = document.createElement("span");
        span.innerText = role;

        if (takenRoles.includes(role)) {
            input.disabled = true;
            span.classList.add("role-disabled");
        }
        label.appendChild(input);
        label.appendChild(span);
        form.appendChild(label);
    });

    return form;
}
function createJoinButton(){
    const joinButton = document.createElement("button");
    joinButton.innerText = "JOIN";
    joinButton.classList.add("join-btn","hide");
    return joinButton;
}

function createRequirmentBar(room){
  const reqBar = document.createElement("div");
  reqBar.classList.add("req-bar");

Object.entries(room.settings).forEach(([key, value]) => {

  if (!settingRenderMap[key]) return;

  const text = settingRenderMap[key](value);
  

  if (text) {
    const p = document.createElement("div");
    p.innerText = text;
    p.classList.add("req-text");
    reqBar.appendChild(p);
  }
});
  return reqBar

}

const settingRenderMap = {
  mic_required: (value) => {
    return value ? "Mic Required" : "";
  },

  min_rank: (value) => {
    return `Min Rank: ${value}`;
  },

  max_rank: (value) => {
    return `Max Rank: ${value}`;
  },

  mode: (value) => value,

  room_type: (value) => `${value}`,

  Server: (value) => `${value}`
};
renderRooms(mock_room);