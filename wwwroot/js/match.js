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
    const [roomsRes, rolesRes] = await Promise.all([
      fetch(`/game/${gameName}/rooms`),
      fetch(`/game/${gameName}/roles`)
    ]);

    if (!roomsRes.ok) throw new Error("Rooms API error: " + roomsRes.status);
    if (!rolesRes.ok) throw new Error("Roles API error: " + rolesRes.status);

    rooms = await roomsRes.json();
    roles = await rolesRes.json();

    renderRooms(rooms, roles);

  } catch (err) {
    console.error("Init error:", err);
  }
}

init();

/* ================================
   RENDER ROOMS
================================ */
function renderRooms(rooms, roles) {
  const container = document.getElementById("MatchContainer");
  container.innerHTML = "";

  rooms.forEach(room => {
    container.appendChild(renderRoom(normalizeRoom(room), roles));
  });
}

/* ================================
   NORMALIZE (กัน undefined)
================================ */
function normalizeRoom(room) {
  return {
    ...room,
    players: room.players || [],
    roomSetting: room.roomSetting || {}
  };
}

/* ================================
   RENDER ROOM
================================ */
function renderRoom(room, roles) {

  const wrapper = document.createElement("div");
  wrapper.classList.add("player-room");

  // Room Name
  const roomName = document.createElement("div");
  roomName.classList.add("room-name");
  roomName.innerText = room.roomName;
  wrapper.appendChild(roomName);

  // Players
  room.players.forEach(player => {
    wrapper.appendChild(PlayerCard(player, room.ownerUsername));
  });

  // Empty Slots
  const playerCount = room.players.length;
  const maxPlayer = room.roomSetting.maxPlayer || 0;
  const emptyCount = maxPlayer - playerCount;

  const roleForm = createRoleForm(room, roles);
  const joinButton = createJoinButton(room.roomId);

  for (let i = 0; i < emptyCount; i++) {
    wrapper.appendChild(EmptySlot(roleForm, joinButton));
  }

  // Requirement Bar
  wrapper.appendChild(createRequirementBar(room));
  wrapper.appendChild(roleForm);
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

  div.innerHTML = `
    <img class="player-profile"
         src="${player.userProfile ?? 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQj5K_Hlzgq-p_0Xfv_vykmcOtuXhBI7VFBxg&s'}">

    ${player.username === ownerUsername
      ? "<span class='empty-crown'>👑</span>"
      : "<span class='empty-crown'>🎮</span>"}

    <p>${player.username}</p>

    <img class="rankImg"
         src="${player.rankName ?? "/images/rank/epic.webp"}">

    <p class="rank-p">${player.roleName || "??"}</p>
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
    roleForm.classList.toggle("hide");
    joinButton.classList.toggle("hide");
  });

  return btn;
}

/* ================================
   ROLE FORM
================================ */
function createRoleForm(room, roles) {

  if (!roles || roles.length === 0) return null;

  const takenRoles = room.players
    ?.map(p => p.roleName)
    .filter(Boolean) || [];

  const form = document.createElement("form");
  form.classList.add("role-form", "hide");

  roles.forEach(role => {

    const label = document.createElement("label");

    const input = document.createElement("input");
    input.classList.add("role-input");
    input.type = "radio";
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
  btn.innerText = "Joining...";
  btn.classList.add("join-btn", "hide");

  btn.addEventListener("click", async () => {


    const selectedRole = document.querySelector(
      `input[name="role-${roomId}"]:checked`
    );

    if (!selectedRole) {
      alert("Please select a role first.");
      return;
    }

    const roleName = selectedRole.value;

    try {
      const response = await fetch(
        `/game/${gameName}/JoinRoom/${roomId}`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify({ roleName: roleName })
        }
      );

      if (response.ok) {
      const data = await response.json();

      // บันทึกห้องลง sessionStorage สำหรับ floating card
      const joinedRooms = JSON.parse(sessionStorage.getItem("joinedRooms") || "[]");
      const alreadySaved = joinedRooms.some(r => r.roomId === data.roomId);
      if (!alreadySaved && data.roomUrl) {
        joinedRooms.push({
          roomId: data.roomId,
          roomName: data.roomName,
          gameName: data.gameName,
          roomUrl: data.roomUrl
        });
        sessionStorage.setItem("joinedRooms", JSON.stringify(joinedRooms));
      }

      if (data.roomUrl) {
        window.location.href = data.roomUrl;
      } else {
        alert(data.message); // private room → added to queue
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

  const settings = room.roomSetting;

  const items = [
    `Min Rank: ${settings.minRank}`,
    `Max Rank: ${settings.maxRank}`,
    settings.isPrivate ? "Private Room" : "Public Room",
    settings.allowDuplicateRole
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