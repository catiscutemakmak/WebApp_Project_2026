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

    const isRejected = room.status === "Rejected";
    const statusText = isRejected ? "\u274c Request rejected." : "\u23f3 Waiting for approval...";

    card.innerHTML = `
        <div class="floating-queue-info">
            <span class="floating-queue-game">${room.gameName}</span>
            <span class="floating-queue-name">${room.roomName}</span>
            <span class="floating-queue-status">${statusText}</span>
        </div>
        <button class="floating-queue-cancel" title="Dismiss">&times;</button>
    `;

    if (isRejected) {
        card.classList.add("floating-queue-card--rejected");
    }

    card.querySelector(".floating-queue-cancel").addEventListener("click", (e) => {
        e.stopPropagation();
        // เก็บไว้ใน localStorage เพื่อคงไว้ข้าม tab/reload
        const dismissed = JSON.parse(localStorage.getItem("dismissedQueues") || "[]");
        if (!dismissed.includes(room.roomId)) {
            dismissed.push(room.roomId);
            localStorage.setItem("dismissedQueues", JSON.stringify(dismissed));
        }
        card.remove();
        const container = document.getElementById("floating-queue-container");
        const remaining = container?.querySelectorAll(".floating-queue-card");
        const countEl = container?.querySelector(".floating-queue-tab-count");
        if (countEl && remaining) countEl.textContent = remaining.length;
        if (!remaining || remaining.length === 0) {
            if (container) container.innerHTML = "";
        }
    });

    return card;
}

async function initFloatingQueue() {
    const container = document.getElementById("floating-queue-container");
    if (!container) return;

    container.innerHTML = "";

    let queuedRooms = [];
    try {
        const res = await fetch("/api/rooms/my-queue-rooms");
        if (res.ok) queuedRooms = await res.json();
    } catch { return; }

    // กรองสิ่งที่ dismiss ไว้แล้วใน localStorage
    const dismissed = JSON.parse(localStorage.getItem("dismissedQueues") || "[]");
    queuedRooms = queuedRooms.filter(r => !dismissed.includes(r.roomId));

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

async function updateQueueCard() {
    await initFloatingCards();
    await initFloatingQueue();
}

async function initQueueNotifications() {
    if (typeof signalR === "undefined") return;
    if (window._queueHandlerRegistered) return;

    let allQueueRooms = [];
    try {
        const res = await fetch("/api/rooms/my-queue-rooms");
        if (res.ok) allQueueRooms = await res.json();
    } catch { return; }

    if (allQueueRooms.length === 0) return;

    const notifConn = new signalR.HubConnectionBuilder()
        .withUrl("/roomhub")
        .withAutomaticReconnect()
        .build();

    notifConn.on("QueueUpdated", async () => {
        await initFloatingCards();
        await initFloatingQueue();
    });

    // join group ทุก room ที่ยังรอ queue (ทั้ง Queue และ Rejected)
    notifConn.start().then(() => {
        allQueueRooms.forEach(r => notifConn.invoke("AcceptRejectQueue", String(r.roomId)));
    }).catch(err => console.error("Queue notification error:", err));
}

document.addEventListener("DOMContentLoaded", initFloatingQueue);
document.addEventListener("DOMContentLoaded", initQueueNotifications);
