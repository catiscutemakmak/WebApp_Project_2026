async function reloadRooms() {
  try {
    const res = await fetch(`/game/${gameName}/rooms`);

    if (!res.ok) {
      throw new Error("Reload rooms failed");
    }

    rooms = await res.json();
    renderRooms(rooms);

  } catch (err) {
    console.error("Reload rooms error:", err);
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

/* ================================
   INIT
================================ */
async function init() {

  try {

    const rolesRes = await fetch(`/game/${gameName}/roles`);

    if (!rolesRes.ok) throw new Error("Roles API error");

    roles = await rolesRes.json();

    await reloadRooms();

  } catch (err) {
    console.error("Init error:", err);
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
function renderRooms(rooms) {

  const container = document.getElementById("MatchContainer");
  container.innerHTML = "";

  rooms.forEach(room => {

    const safeRoom = normalizeRoom(room);
    container.appendChild(renderRoom(safeRoom));

  });

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
    wrapper.appendChild(PlayerCard(player, room.ownerUsername));
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
function PlayerCard(player, ownerUsername) {

  const div = document.createElement("div");
  div.classList.add("player-dev");
  div.style.cursor = "pointer";

  div.addEventListener("click", () => {
    window.location.href = `/Profile/View/${player.userId}`;
  });

  const rankImg =
    player.rankName
      ? `${player.rankName}`
      : "/images/rank/unranked.webp";

  div.innerHTML = `
    <img class="player-profile"
      src="${player.userProfile ?? 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQj5K_Hlzgq-p_0Xfv_vykmcOtuXhBI7VFBxg&s'}">

    ${player.username === ownerUsername
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

    const selectedRole = document.querySelector(
      `input[name="role-${roomId}"]:checked`
    );

    if (!selectedRole && roles.length !== 0) {
      alert("Please select a role.");
      return;
    }

    const roleName = selectedRole ? selectedRole.value : null;

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
        window.location.href = data.roomUrl;

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

  const s = room.roomSetting;

  const items = [

    s.minRank ? `Min Rank: ${s.minRank}` : null,
    s.maxRank ? `Max Rank: ${s.maxRank}` : null,
    s.isPrivate ? "Private Room" : "Public Room",
    s.allowDuplicateRole
      ? "Duplicate Role Allowed"
      : "No Duplicate Role"

  ];

  items.forEach(text => {

    if (!text) return;

    const div = document.createElement("div");
    div.classList.add("req-text");
    div.innerText = text;

    bar.appendChild(div);

  });

  return bar;

}