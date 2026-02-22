flatpickr("#play-date", {
    disableMobile: true,
    altInput: true,          // แสดง input ใหม่สำหรับผู้ใช้
    altFormat: "d M Y",      // รูปแบบที่ผู้ใช้เห็น → 24 Feb 2026
    dateFormat: "Y-m-d"      // รูปแบบที่ส่ง backend → 2026-02-24
});

document.querySelector('.player-select').addEventListener('change', function () {
    console.log("Players:", this.value);
});


flatpickr("#play-date", {
    minDate: "today",
});

flatpickr("#play-time", {
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    time_24hr: true,
    minuteIncrement: 1
});


const gameData = {
    mlbb: {
        modes: ["Classic", "Ranked", "Brawl", "Custom"],
        servers: ["Thailand", "SEA"],
        ranks: ["Warrior", "Elite", "Master", "Grandmaster", "Epic", "Legend", "Mythic"]
    },
    rov: {
        modes: ["Normal", "Ranked", "Abyssal Clash", "Custom"],
        servers: ["Thailand", "Vietnam", "Taiwan"],
        ranks: ["Bronze", "Silver", "Gold", "Platinum", "Diamond", "Conqueror"]
    },
    valorant: {
        modes: ["Unrated", "Competitive", "Swiftplay", "Spike Rush", "Deathmatch"],
        servers: ["Hong Kong", "Singapore", "Tokyo"],
        ranks: ["Iron", "Bronze", "Silver", "Gold", "Platinum", "Diamond", "Ascendant", "Immortal", "Radiant"]
    },
    csgo: {
        modes: ["Competitive", "Casual", "Deathmatch", "Wingman"],
        servers: ["Asia", "Europe", "America"],
        ranks: ["Silver", "Gold Nova", "Master Guardian", "Distinguished", "Legendary Eagle", "Supreme", "Global Elite"]
    },
    overwatch: {
        modes: ["Quick Play", "Competitive", "Arcade", "Custom"],
        servers: ["Asia", "America", "Europe"],
        ranks: ["Bronze", "Silver", "Gold", "Platinum", "Diamond", "Master", "Grandmaster"]
    },
    pubg: {
        modes: ["Normal", "Ranked", "Arcade"],
        servers: ["Asia", "SEA", "America", "Europe"],
        ranks: ["Bronze", "Silver", "Gold", "Platinum", "Diamond", "Master"]
    },
    amongus: {
        modes: ["Classic", "Hide & Seek"],
        servers: ["Asia", "Europe", "North America"],
        ranks: ["Any"]
    },
    lol: {
        modes: ["Normal", "Ranked Solo/Duo", "Ranked Flex", "ARAM", "Custom"],
        servers: ["TH", "SG", "JP", "KR"],
        ranks: ["Iron", "Bronze", "Silver", "Gold", "Platinum", "Diamond", "Master", "Grandmaster", "Challenger"]
    }
};

const gameSelect = document.getElementById("game-select");
const modeSelect = document.getElementById("game-mode");
const serverSelect = document.getElementById("game-server");
const minRank = document.getElementById("min-rank");
const maxRank = document.getElementById("max-rank");

function fillSelect(select, items, placeholder) {
    select.innerHTML = `<option disabled selected hidden>${placeholder}</option>`;
    items.forEach(item => {
        const opt = document.createElement("option");
        opt.value = item;
        opt.textContent = item;
        select.appendChild(opt);
    });
    select.disabled = false;
}

gameSelect.addEventListener("change", () => {
    const game = gameSelect.value;
    const data = gameData[game];

    if (!data) return;

    fillSelect(modeSelect, data.modes, "Choose Mode");
    fillSelect(serverSelect, data.servers, "Choose Server");
    fillSelect(minRank, data.ranks, "Min Rank");
    fillSelect(maxRank, data.ranks, "Max Rank");
});

maxRank.addEventListener("change", () => {
    if (minRank.selectedIndex > maxRank.selectedIndex) {
        minRank.selectedIndex = maxRank.selectedIndex;
    }
});