// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* ================================
   FLOATING ROOM CARD
================================ */

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

    card.addEventListener("click", () => {
        window.location.href = room.roomUrl;
    });

    return card;
}

function initFloatingCards() {
    const container = document.getElementById("floating-room-container");
    if (!container) return;

    // ล้าง container ก่อนเสมอ เพื่อป้องกัน duplicate เมื่อ init ซ้ำ
    container.innerHTML = "";

    const joinedRooms = JSON.parse(sessionStorage.getItem("joinedRooms") || "[]");
    if (joinedRooms.length === 0) return;

    // สร้าง panel เก็บ cards
    const panel = document.createElement("div");
    panel.classList.add("floating-room-panel", "hidden");

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
    arrow.textContent = "\u25b2";

    tab.appendChild(label);
    tab.appendChild(count);
    tab.appendChild(arrow);

    joinedRooms.forEach(room => {
        panel.appendChild(createFloatingCard(room));
    });

    let isExpanded = false;
    tab.addEventListener("click", () => {
        isExpanded = !isExpanded;
        panel.classList.toggle("hidden", !isExpanded);
        arrow.textContent = isExpanded ? "\u25bc" : "\u25b2";
    });

    container.appendChild(panel);
    container.appendChild(tab);
}

document.addEventListener("DOMContentLoaded", initFloatingCards);
