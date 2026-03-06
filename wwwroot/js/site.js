// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* ================================
   FLOATING ROOM CARD
================================ */

// --- Mock data: array รองรับหลายห้อง ---
// TODO: เมื่อ join logic พร้อม ให้เปลี่ยนมาอ่านจาก sessionStorage แทน
// const joinedRooms = JSON.parse(sessionStorage.getItem("joinedRooms") || "[]");
const joinedRooms = [
    { roomId: 1, roomName: "หาทีม MLBB ด่วน", gameName: "Mobile Legends", roomUrl: "/game/Mobile Legends/room/1" },
    { roomId: 2, roomName: "Ranked Only วาโลฯ", gameName: "Valorant",       roomUrl: "/game/Valorant/room/2" }
];

function createFloatingCard(room) {
    const card = document.createElement("div");
    card.classList.add("floating-room-card");
    card.dataset.roomId = room.roomId;

    card.innerHTML = `
        <div class="floating-room-info">
            <span class="floating-room-game">${room.gameName}</span>
            <span class="floating-room-name">${room.roomName}</span>
        </div>
    `;

    // กดที่ card → ไปหน้าห้อง
    card.addEventListener("click", () => {
        window.location.href = room.roomUrl;
    });

    return card;
}

function initFloatingCards() {
    const container = document.getElementById("floating-room-container");
    if (!container || joinedRooms.length === 0) return;

    // สร้าง panel เก็บ cards
    const panel = document.createElement("div");
    panel.classList.add("floating-room-panel", "hidden"); // เริ่มต้น minimized

    // สร้าง tab แถบล่าง
    const tab = document.createElement("div");
    tab.classList.add("floating-room-tab");

    const label = document.createElement("span");
    label.classList.add("floating-room-tab-label");
    label.textContent = "MY ROOMS";

    const count = document.createElement("span");
    count.classList.add("floating-room-tab-count");
    count.textContent = joinedRooms.length;

    const arrow = document.createElement("span");
    arrow.classList.add("floating-room-tab-arrow");
    arrow.textContent = "▲";

    tab.appendChild(label);
    tab.appendChild(count);
    tab.appendChild(arrow);

    // เพิ่ม cards เข้า panel
    joinedRooms.forEach(room => {
        panel.appendChild(createFloatingCard(room));
    });

    // กด tab → toggle expand/minimize
    let isExpanded = false;
    tab.addEventListener("click", () => {
        isExpanded = !isExpanded;
        panel.classList.toggle("hidden", !isExpanded);
        arrow.textContent = isExpanded ? "▼" : "▲";
    });

    container.appendChild(panel);
    container.appendChild(tab);
}

document.addEventListener("DOMContentLoaded", initFloatingCards);
