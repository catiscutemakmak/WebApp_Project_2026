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

async function initFloatingCards() {
    const container = document.getElementById("floating-room-container");
    if (!container) return;

    container.innerHTML = "";

    let joinedRooms = [];
    try {
        const res = await fetch("/api/my-active-rooms");
        if (res.ok) joinedRooms = await res.json();
    } catch { return; }

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

/* ================================
   FLOATING QUEUE CARD
================================ */

function createQueueCard(room) {
    const card = document.createElement("div");
    card.classList.add("floating-queue-card");
    card.dataset.roomId = room.roomId;

    const isAccepted = !!room.accepted;
    const isRejected = !!room.rejected;

    let statusText = "Waiting for approval...";
    if (isAccepted) statusText = "\u2705 Accepted! Click to join.";
    if (isRejected) statusText = "\u274c Request rejected.";

    card.innerHTML = `
        <div class="floating-queue-info">
            <span class="floating-queue-game">${room.gameName}</span>
            <span class="floating-queue-name">${room.roomName}</span>
            <span class="floating-queue-status">${statusText}</span>
        </div>
        <button class="floating-queue-cancel" title="Dismiss">&times;</button>
    `;

    if (isAccepted && room.roomUrl) {
        card.classList.add("floating-queue-card--accepted");
        card.style.cursor = "pointer";
        card.addEventListener("click", (e) => {
            if (e.target.closest(".floating-queue-cancel")) return;
            // ย้ายจาก queuedRooms → joinedRooms
            const joinedRooms = JSON.parse(sessionStorage.getItem("joinedRooms") || "[]");
            if (!joinedRooms.some(r => r.roomId === room.roomId)) {
                joinedRooms.push({ roomId: room.roomId, roomName: room.roomName, gameName: room.gameName, roomUrl: room.roomUrl });
                sessionStorage.setItem("joinedRooms", JSON.stringify(joinedRooms));
            }
            const queuedRooms = JSON.parse(sessionStorage.getItem("queuedRooms") || "[]");
            sessionStorage.setItem("queuedRooms", JSON.stringify(queuedRooms.filter(r => r.roomId !== room.roomId)));
            window.location.href = room.roomUrl;
        });
    } else if (isRejected) {
        card.classList.add("floating-queue-card--rejected");
    }

    card.querySelector(".floating-queue-cancel").addEventListener("click", (e) => {
        e.stopPropagation();
        const queuedRooms = JSON.parse(sessionStorage.getItem("queuedRooms") || "[]");
        const updated = queuedRooms.filter(r => r.roomId !== room.roomId);
        sessionStorage.setItem("queuedRooms", JSON.stringify(updated));
        card.remove();
        const countEl = document.getElementById("floating-queue-container")?.querySelector(".floating-queue-tab-count");
        if (countEl) countEl.textContent = updated.length;
        if (updated.length === 0) {
            const container = document.getElementById("floating-queue-container");
            if (container) container.innerHTML = "";
        }
    });

    return card;
}

function initFloatingQueue() {
    const container = document.getElementById("floating-queue-container");
    if (!container) return;

    container.innerHTML = "";

    const queuedRooms = JSON.parse(sessionStorage.getItem("queuedRooms") || "[]");
    if (queuedRooms.length === 0) return;

    const panel = document.createElement("div");
    panel.classList.add("floating-queue-panel", "hidden");

    const tab = document.createElement("div");
    tab.classList.add("floating-queue-tab");

    const label = document.createElement("span");
    label.classList.add("floating-queue-tab-label");
    label.textContent = "MY QUEUE";

    const count = document.createElement("span");
    count.classList.add("floating-queue-tab-count");
    count.textContent = queuedRooms.length;

    const arrow = document.createElement("span");
    arrow.classList.add("floating-queue-tab-arrow");
    arrow.textContent = "\u25b2";

    tab.appendChild(label);
    tab.appendChild(count);
    tab.appendChild(arrow);

    queuedRooms.forEach(room => panel.appendChild(createQueueCard(room)));

    let isExpanded = false;
    tab.addEventListener("click", () => {
        isExpanded = !isExpanded;
        panel.classList.toggle("hidden", !isExpanded);
        arrow.textContent = isExpanded ? "\u25bc" : "\u25b2";
    });

    container.appendChild(panel);
    container.appendChild(tab);
}

function updateQueueCard(roomId, status, roomUrl) {
    // อัปเดต sessionStorage
    const queuedRooms = JSON.parse(sessionStorage.getItem("queuedRooms") || "[]");
    const idx = queuedRooms.findIndex(r => r.roomId === roomId);
    if (idx === -1) return;

    if (status === "Active") {
        queuedRooms[idx].accepted = true;
        queuedRooms[idx].roomUrl = roomUrl;
    } else if (status === "Rejected") {
        queuedRooms[idx].rejected = true;
    } else {
        return; // ยัง Queue อยู่ ไม่ต้องทำอะไร
    }
    sessionStorage.setItem("queuedRooms", JSON.stringify(queuedRooms));

    // อัปเดต card ที่แสดงอยู่
    const container = document.getElementById("floating-queue-container");
    const card = container?.querySelector(`[data-room-id="${roomId}"]`);
    if (!card) return;

    const statusEl = card.querySelector(".floating-queue-status");

    if (status === "Active" && roomUrl) {
        const { roomName, gameName } = queuedRooms[idx];
        card.classList.add("floating-queue-card--accepted");
        card.style.cursor = "pointer";
        if (statusEl) statusEl.textContent = "\u2705 Accepted! Click to join.";
        card.addEventListener("click", (e) => {
            if (e.target.closest(".floating-queue-cancel")) return;
            // ย้ายจาก queuedRooms → joinedRooms
            const joinedRooms = JSON.parse(sessionStorage.getItem("joinedRooms") || "[]");
            if (!joinedRooms.some(r => r.roomId === roomId)) {
                joinedRooms.push({ roomId, roomName, gameName, roomUrl });
                sessionStorage.setItem("joinedRooms", JSON.stringify(joinedRooms));
            }
            const currentQueue = JSON.parse(sessionStorage.getItem("queuedRooms") || "[]");
            sessionStorage.setItem("queuedRooms", JSON.stringify(currentQueue.filter(r => r.roomId !== roomId)));
            window.location.href = roomUrl;
        });
    } else if (status === "Rejected") {
        card.classList.add("floating-queue-card--rejected");
        card.style.cursor = "default";
        if (statusEl) statusEl.textContent = "\u274c Request rejected.";
    }
}

function initQueueNotifications() {
    if (typeof signalR === "undefined") return;
    // match.js จัดการ QueueUpdated ไว้แล้วบน connection ของตัวเอง ไม่ต้องสร้างซ้ำ
    if (window._queueHandlerRegistered) return;

    const queuedRooms = JSON.parse(sessionStorage.getItem("queuedRooms") || "[]");
    const pending = queuedRooms.filter(r => !r.accepted && !r.rejected);
    if (pending.length === 0) return;

    const notifConn = new signalR.HubConnectionBuilder()
        .withUrl("/roomhub")
        .withAutomaticReconnect()
        .build();

    notifConn.on("QueueUpdated", async (roomId) => {
        const rooms = JSON.parse(sessionStorage.getItem("queuedRooms") || "[]");
        const target = rooms.find(r => r.roomId === roomId);
        if (!target || target.accepted || target.rejected) return;

        try {
            const res = await fetch(`/api/rooms/${roomId}/my-queue-status`);
            if (!res.ok) return;
            const data = await res.json();
            updateQueueCard(roomId, data.status, data.roomUrl);
        } catch (e) {
            console.error("Queue status check error:", e);
        }
    });

    notifConn.start().then(() => {
        pending.forEach(r => notifConn.invoke("AcceptRejectQueue", String(r.roomId)));
    }).catch(err => console.error("Queue notification error:", err));
}

document.addEventListener("DOMContentLoaded", initFloatingQueue);
document.addEventListener("DOMContentLoaded", initQueueNotifications);
