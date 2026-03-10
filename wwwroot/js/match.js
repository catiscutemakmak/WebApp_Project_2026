const amongUsAvatars = [
"/images/amongus/Banana.webp",
"/images/amongus/Blue.webp",
"/images/amongus/Brown.webp",
"/images/amongus/Coral.webp",
"/images/amongus/Cyan.webp",
"/images/amongus/Gray.webp",
"/images/amongus/Green.webp",
"/images/amongus/Lime.webp",
"/images/amongus/Maroon.webp",
"/images/amongus/Orange.webp",
"/images/amongus/Pink.webp",
"/images/amongus/Purple.webp",
"/images/amongus/Red.webp",
"/images/amongus/Tan.webp",
"/images/amongus/White.webp",
"/images/amongus/Yellow.webp"
];
let currentPage = 1;
const roomsPerPage =5;
let selectedAvatar = null;
let selectedRoomId = null;
let selectedRankId = null;
let selectedRoleName = null;
async function reloadRooms() {

  try {

    const res = await fetch(`/game/${gameName}/rooms`);

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

async function reloadRank() {

  try {

    const res = await fetch(`/api/get/rank/${gameName}`);

    if (!res.ok) {
      throw new Error("Reload rank failed");
    }

    ranks = await res.json();
    

  } catch (err) {

    console.error("Reload rank error:", err);

  }

}

/* ================================
   SIGNALR
================================ */

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/roomhub")
  .withAutomaticReconnect()
  .build();

connection.on("RoomCreated", async (updatedGameName) => {

  if (updatedGameName !== gameName) return;

  try {
    await reloadRooms();
  } catch (err) {
    console.error("Realtime update error:", err);
  }

});

connection.on("PlayerJoinedRoom", async (updatedGameName) => {

  if (updatedGameName !== gameName) return;

  try {
    await reloadRooms();
  } catch (err) {
    console.error("Realtime update error:", err);
  }

});

/* ================================
   GLOBAL STATE
================================ */
let rooms = [];
let roles = [];
let ranks = [];

/* ================================
   INIT
================================ */
async function init() {

  try {

    showLoading();

    const rolesRes = await fetch(`/game/${gameName}/roles`);
    roles = await rolesRes.json();
    await reloadRank();
    await reloadRooms();
    

  } catch(err){

    console.error(err);

  } finally {

    hideLoading();

  }

}

async function startRealtime(){

  try {

    await connection.start();

    await connection.invoke("JoinGameGroup", gameName);

    console.log("SignalR connected");

  } catch(err) {

    console.error("SignalR error:", err);

  }

}

init();
startRealtime();

/* ================================
   NORMALIZE ROOM
================================ */
function normalizeRoom(room) {

  return {
    ...room,
    players: room.players ?? [],
    roomSetting: room.roomSetting ?? {},
  };

}



/* ================================
   RENDER ROOMS
================================ */
function renderRooms(allRooms) {

  const container = document.getElementById("MatchContainer");
  container.innerHTML = "";

  const start = (currentPage - 1) * roomsPerPage;
  const end = start + roomsPerPage;

  const paginatedRooms = allRooms.slice(start, end);

  paginatedRooms.forEach(room => {
    const safeRoom = normalizeRoom(room);
    container.appendChild(renderRoom(safeRoom));
  });

  renderPagination(allRooms.length);
}

/* ================================
   RENDER ROOM
================================ */
function renderRoom(room) {

  const wrapper = document.createElement("div");
  wrapper.classList.add("player-room");

  /* ROOM NAME */

  const roomName = document.createElement("div");
  roomName.classList.add("room-name");
  roomName.innerText = room.roomName;
  wrapper.appendChild(roomName);

  /* PLAYERS */

  room.players.forEach(player => {
    wrapper.appendChild(PlayerCard(player, room.ownerId));
  });

  /* EMPTY SLOTS */

  const playerCount = room.players.length;
  const maxPlayer = room.roomSetting.maxPlayer ?? 0;
  const emptyCount = maxPlayer - playerCount;

  const roleForm = createRoleForm(room);
  const joinButton = createJoinButton(room.roomId);

  for (let i = 0; i < emptyCount; i++) {
    wrapper.appendChild(EmptySlot(roleForm, joinButton));
  }

  /* REQUIREMENT BAR */

  wrapper.appendChild(createRequirementBar(room));

  if (roleForm) wrapper.appendChild(roleForm);
  wrapper.appendChild(joinButton);

  return wrapper;

}

/* ================================
   PLAYER CARD
================================ */
function PlayerCard(player, OwnerId) {

  const div = document.createElement("div");
  div.classList.add("player-dev");
  div.style.cursor = "pointer";

  div.addEventListener("click", () => {
    window.location.href = `/Profile/ViewProfile/${player.userId}`;
  });

    let rankImg;

  if (gameName === "Among Us") {

    rankImg = player.avatar
      ? player.avatar
      : "/images/amongus/Lime.webp";
  } else {
    rankImg = player.rankName
      ? player.rankName
      : "/images/default-rank.png";
  }

  div.innerHTML = `
    <img class="player-profile"
      src="${player.userProfile ?? 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQj5K_Hlzgq-p_0Xfv_vykmcOtuXhBI7VFBxg&s'}">

    ${player.userId === OwnerId
      ? "<span class='empty-crown'>👑</span>"
      : "<span class='empty-crown'>🎮</span>"}

    <p>${player.username}</p>

    <img class="rankImg" src="${rankImg}">

    <p class="rank-p">${player.roleName ?? "No Role"}</p>
  `;

  return div;

}

/* ================================
   EMPTY SLOT
================================ */
function EmptySlot(roleForm, joinButton) {

  const btn = document.createElement("button");

  btn.type = "button";
  btn.classList.add("empty-dev");

  btn.innerHTML = `<span class="empty-crown">/ᐠ • ˕ •マ ?</span>`;

  btn.addEventListener("click", () => {

    if (roleForm) roleForm.classList.toggle("hide");
    if (joinButton) joinButton.classList.toggle("hide");

  });

  return btn;

}

/* ================================
   ROLE FORM
================================ */
function createRoleForm(room) {

  if (!roles || roles.length === 0) return null;

  const takenRoles = room.players
    .map(p => p.roleName)
    .filter(Boolean);

  const form = document.createElement("form");
  form.classList.add("role-form", "hide");

  roles.forEach(role => {

    const label = document.createElement("label");

    const input = document.createElement("input");
    input.type = "radio";
    input.classList.add("role-input");

    input.name = `role-${room.roomId}`;
    input.value = role.roleName;

    const span = document.createElement("span");
    span.innerText = role.roleName;

    if (!room.roomSetting?.allowDuplicateRole &&
        takenRoles.includes(role.roleName)) {

      input.disabled = true;
      span.classList.add("role-disabled");

    }

    label.appendChild(input);
    label.appendChild(span);

    form.appendChild(label);

  });

  return form;

}

/* ================================
   JOIN BUTTON
================================ */
function createJoinButton(roomId) {

  const btn = document.createElement("button");

  btn.type = "button";
  btn.innerText = "Join";
  btn.classList.add("join-btn", "hide");

  btn.addEventListener("click", async () => {

    selectedRoomId = roomId;

    const selectedRole = document.querySelector(
      `input[name="role-${roomId}"]:checked`
    );

    if (!selectedRole && roles.length !== 0) {
      alert("Please select a role.");
      return;
    }

    const roleName = selectedRole ? selectedRole.value : null;
    selectedRoleName = roleName;

    // Among Us → Avatar Picker
    if (gameName === "Among Us") {
      openAvatarPicker();
      return;
    }

    // Game ที่มี Rank
    if (ranks && ranks.length > 0) {
      const room = rooms.find(r => r.roomId === roomId);
      openRankPicker(room);
      return;
    }

    // Game ที่ไม่มี Rank / Avatar → join ตรง
    try {

      const response = await fetch(
        `/game/${gameName}/JoinRoom/${roomId}`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ roleName })
        }
      );

      if (response.ok) {

        const data = await response.json();

        if (data.roomUrl) {
          window.location.href = data.roomUrl;
        } else {

          const dismissed = JSON.parse(localStorage.getItem("dismissedQueues") || "[]");
          const updated = dismissed.filter(id => id !== data.roomId);

          localStorage.setItem("dismissedQueues", JSON.stringify(updated));

          if (connection.state === "Connected") {
            connection.invoke("AcceptRejectQueue", String(data.roomId));
          }

          if (typeof initFloatingQueue === "function") {
            initFloatingQueue();
          }

        }

      } else {

        const errorText = await response.text();
        alert(errorText);

      }

    } catch (err) {
      console.error(err);
    }

  });

  return btn;
}
/* ================================
   REQUIREMENT BAR
================================ */
function createRequirementBar(room) {

  const bar = document.createElement("div");
  bar.classList.add("req-bar");

  const s = room.roomSetting ?? {};

  const items = [
    room.roomStatus === 0 ? "🔵 Waiting Room" : null,
    room.roomStatus === 1 ? "🟠 Room Full" : null,
    room.roomStatus === 2 ? "🔴 Room Start" : null,
 
    room.myStatus === "Active" ? "🟢 In Room" : null,
    room.myStatus === "Queue" ? "🟡 In Queue" : null,

    s.minRank ? `Min Rank: ${s.minRank}` : null,
    s.maxRank ? `Max Rank: ${s.maxRank}` : null,
    s.isPrivate ? "🔒 Private Room" : "🌐 Public Room",
    s.allowDuplicateRole
      ? "♻ Duplicate Role Allowed"
      : "🚫 No Duplicate Role"

  ];

  items.forEach(text => {

    if (!text) return;

    const div = document.createElement("div");
    div.classList.add("req-text");

    if (text.includes("In Room")) {
      div.classList.add("status-active");
    }

    if (text.includes("In Queue")) {
      div.classList.add("status-queue");
    }

    
    if (text.includes("Waiting Room")) {
      div.classList.add("status-waiting");
    }

    if (text.includes("Room Full")) {
      div.classList.add("status-full");
    }

    if (text.includes("Room Start")) {
      div.classList.add("status-start");
    }
    div.innerText = text;

    bar.appendChild(div);

  });

  return bar;

}
function renderPagination(totalRooms){

  const totalPages = Math.ceil(totalRooms / roomsPerPage);

  const nav = document.createElement("div");
  nav.classList.add("pagination");

  const prev = document.createElement("button");
  prev.innerText = "<< Prev";
  prev.disabled = currentPage === 1;

  prev.onclick = () => {
    currentPage--;
    renderRooms(rooms);
  };

  const pageInfo = document.createElement("span");
  pageInfo.innerText = `Page ${currentPage} / ${totalPages}`;

  const next = document.createElement("button");
  next.innerText = "Next >>";
  next.disabled = currentPage === totalPages;

  next.onclick = () => {
    currentPage++;
    renderRooms(rooms);
  };

  nav.appendChild(prev);
  nav.appendChild(pageInfo);
  nav.appendChild(next);

  document.getElementById("MatchContainer").appendChild(nav);
}

function showLoading(){
  document.body.classList.add("loading");
}

function hideLoading(){
  document.body.classList.remove("loading");
}
function openAvatarPicker(){
  const room = rooms.find(r => r.roomId === selectedRoomId);

  const takenAvatars = room.players
    .map(p => p.avatar)
    .filter(a => a);

  const picker = document.getElementById("avatarPicker");
  const grid = document.getElementById("avatarGrid");
  selectedAvatar = null;
  const confirmBtn = document.getElementById("avatarConfirm");
  picker.addEventListener("click", (e) => {

  const box = document.querySelector(".avatar-box");

  if (!box.contains(e.target)) {
    picker.classList.add("hide");
  }

});
  grid.innerHTML = "";

  amongUsAvatars.forEach(src => {

    const img = document.createElement("img");
    img.src = src;

    if (takenAvatars.includes(src)) {
      img.classList.add("taken");
      img.style.opacity = "0.3";
      img.style.pointerEvents = "none";
    }

    img.onclick = () => {

      document.querySelectorAll("#avatarGrid img")
        .forEach(i => i.classList.remove("selected"));

      img.classList.add("selected");

      selectedAvatar = src;
      confirmBtn.disabled = false;

    };

    grid.appendChild(img);

  });

  picker.classList.remove("hide");

}
document.getElementById("avatarConfirm").onclick = async () => {

  document.getElementById("avatarPicker").classList.add("hide");

  if(gameName === "Among Us"){

    if(!selectedAvatar) return;
    await joinRoomWithAvatar(selectedAvatar);

  }else{

    if(!selectedRankId) return;
    await joinRoomWithRank(selectedRankId);

  }

};

async function joinRoomWithAvatar(avatar){

  try {

    const response = await fetch(
      `/game/${gameName}/JoinRoom/${selectedRoomId}`,
      {
        method:"POST",
        headers:{ "Content-Type":"application/json" },
        body: JSON.stringify({
          roleName:null,
          avatar:avatar
        })
      }
    );

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || "Join room failed");
    }

    const data = await response.json();

    window.location.href = data.roomUrl;

  } catch (err) {

    console.error("Join room error:", err);
    alert(err.message);

  }

}

function openRankPicker(room){

  const picker = document.getElementById("avatarPicker");
  const grid = document.getElementById("avatarGrid");
  const showname = document.getElementById("ShowName");
  const confirmBtn = document.getElementById("avatarConfirm");

  showname.innerText = "Select Your Rank";
  selectedRankId = null;
  confirmBtn.disabled = true;

    picker.addEventListener("click", (e) => {

    const box = document.querySelector(".avatar-box");

    if (!box.contains(e.target)) {
        picker.classList.add("hide");
    }

    });
  grid.innerHTML = "";

  const minRank = room.roomSetting?.minRank;
  const maxRank = room.roomSetting?.maxRank;

  const minIndex = ranks.findIndex(r => r.rankName === minRank);
  const maxIndex = ranks.findIndex(r => r.rankName === maxRank);

  ranks.forEach((r,index) => {

    const img = document.createElement("img");
    img.src = r.rankImageUrl;

    const isBelowMin = minIndex !== -1 && index < minIndex;
    const isAboveMax = maxIndex !== -1 && index > maxIndex;

    if (isBelowMin || isAboveMax) {

      img.classList.add("disabled");
      img.style.opacity = "0.3";
      img.style.pointerEvents = "none";

    } else {

      img.onclick = () => {

        document.querySelectorAll("#avatarGrid img")
          .forEach(i => i.classList.remove("selected"));

        img.classList.add("selected");

        selectedRankId = r.id;
        confirmBtn.disabled = false;

      };

    }

    grid.appendChild(img);

  });

  picker.classList.remove("hide");

}

async function joinRoomWithRank(rankId){

  try {

    const response = await fetch(
      `/game/${gameName}/JoinRoom/${selectedRoomId}`,
      {
        method:"POST",
        headers:{ "Content-Type":"application/json" },
        body: JSON.stringify({
          roleName:selectedRoleName,
          rankId:rankId,
          avatar:null
        })
      }
    );

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || "Join room failed");
    }

    const data = await response.json();

    window.location.href = data.roomUrl;

  } catch (err) {

    console.error("Join room error:", err);
    alert(err.message);

  }

}