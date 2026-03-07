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
    "Mobile Legends": {
        modes: ["Classic", "Ranked", "Brawl", "Custom"],
        servers: ["Thailand", "SEA"],
        ranks: ["Warrior", "Elite", "Master", "Grandmaster", "Epic", "Legend", "Mythic"],
        role: ["GoldLane", "ExpLane", "MidLane", "MarkMan", "Support"]
    },
    "RoV": {
        modes: ["Normal", "Ranked", "Abyssal Clash", "Custom"],
        servers: ["Thailand", "Vietnam", "Taiwan"],
        ranks: ["Bronze", "Silver", "Gold", "Platinum", "Diamond", "Commander", "Conqueror"],
        role: ["OffLane", "Jungle", "MidLane", "Carry", "Roam"]
    },
    "Valorant": {
        modes: ["Unrated", "Competitive", "Swiftplay", "Spike Rush", "Deathmatch"],
        servers: ["Hong Kong", "Singapore", "Tokyo"],
        ranks: ["Iron", "Bronze", "Silver", "Gold", "Platinum", "Diamond", "Ascendant", "Immortal", "Radiant"],
        role: ["Duelist", "Sentinel", "Controller", "Initiator"]
    },
    "CS2": {
        modes: ["Competitive", "Casual", "Deathmatch", "Wingman"],
        servers: ["Asia", "Europe", "America"],
        ranks: ["Silver", "Gold Nova", "Master Guardian", "Distinguished", "Legendary Eagle", "Supreme", "Global Elite"],
        role: ["Entry Fragger", "Support", "AWPer", "Lurker", "In-Game Leader"]
    },
    "Overwatch": {
        modes: ["Quick Play", "Competitive", "Arcade", "Custom"],
        servers: ["Asia", "America", "Europe"],
        ranks: ["Bronze", "Silver", "Gold", "Platinum", "Diamond", "Master", "Grandmaster"],
        role: ["Tank", "Damage", "Support"]
    },
    "PUBG": {
        modes: ["Normal", "Ranked", "Arcade"],
        servers: ["Asia", "SEA", "America", "Europe"],
        ranks: ["Bronze", "Silver", "Gold", "Platinum", "Crown", "Ace","AceMaster","AceDominator","Conqueror"],
        role: ["Any"]
    },
    "Among Us": {
        modes: ["Classic", "Hide & Seek"],
        servers: ["Asia", "Europe", "North America"],
        ranks: ["Any"],
        role: ["Any"]
    },
    "LoL": {
        modes: ["Normal", "Ranked Solo/Duo", "Ranked Flex", "ARAM", "Custom"],
        servers: ["TH", "SG", "JP", "KR"],
        ranks: ["Iron", "Bronze", "Silver", "Gold", "Platinum", "Diamond", "Master", "Grandmaster", "Challenger"],
        role: ["Top Lane", "Jungle", "Mid Lane", "ADC", "Support"]
    }
};

const gameSelect = document.getElementById("game-select");
const modeSelect = document.getElementById("game-mode");
const serverSelect = document.getElementById("game-server");
const minRank = document.getElementById("min-rank");
const maxRank = document.getElementById("max-rank");
const roleSelect = document.getElementById("game-role");
const roleContainer = document.getElementById("role-container");
const rankRequirement = document.getElementById("rank-requirement");

function fillSelect(select, items, placeholder) {
    select.innerHTML = `<option value="" selected hidden>${placeholder}</option>`;

    items.forEach(item => {
        const opt = document.createElement("option");
        opt.value = item;
        opt.textContent = item;
        select.appendChild(opt);
    });

    select.disabled = false;
}

function combineDateTime(date, time){
    return date + "T" + time;
}

gameSelect.addEventListener("change", () => {
    const game = gameSelect.value;
    const data = gameData[game];

    if (!data) return;

    fillSelect(modeSelect, data.modes, "Choose Mode");
    fillSelect(serverSelect, data.servers, "Choose Server");


    // ROLE LOGIC
    if (data.role.length === 1 && data.role[0] === "Any") {
        roleContainer.style.display = "none"; // ซ่อน role
        roleSelect.value = "Any";
        
    } else {
        roleContainer.style.display = "block"; // แสดง role
        fillSelect(roleSelect, data.role, "Choose Role");
    }

        // RANK LOGIC
    if (data.ranks.length === 1 && data.ranks[0] === "Any") {
        rankRequirement.style.display = "none";
            minRank.value = "Any";
            maxRank.value = "Any";
    } else {
        rankRequirement.style.display = "block";
            fillSelect(minRank, data.ranks, "Min Rank");
            fillSelect(maxRank, data.ranks, "Max Rank");

    }
});

maxRank.addEventListener("change", () => {
    if (minRank.selectedIndex > maxRank.selectedIndex) {
        minRank.selectedIndex = maxRank.selectedIndex;
    }
});

document.getElementById("create-room-form").addEventListener("submit", function () {
    document.querySelectorAll("select:disabled").forEach(el => {
        el.disabled = false;
    });
});

const form = document.getElementById("create-room-form");

form.addEventListener("submit", async function (e) {
    e.preventDefault(); // กัน reload หน้า

    const formData = new FormData(form);

    const data = {
        game: formData.get("game"),
        roomName: formData.get("roomName"),
        gameMode: formData.get("gameMode"),
        gameServer: formData.get("gameServer"),
        minRank: formData.get("minRank"),
        maxRank: formData.get("maxRank"),
        maxPlayer: formData.get("maxPlayer"),
        roomPrivacy: formData.get("RoomPrivacy") === "true",
        gameRole: formData.get("gameRole"),
        description: formData.get("description"),

        playDateTime: combineDateTime(
            formData.get("playDate"),
            formData.get("playTime")
        )
    };
    console.log("Submitting data:", data);
    const res = await fetch("/CreateTeam/CreateTeam", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });

    if (res.ok) {
        const roomId = await res.text();

        // บันทึกห้องลง sessionStorage สำหรับ floating card
        const joinedRooms = JSON.parse(sessionStorage.getItem("joinedRooms") || "[]");
        joinedRooms.push({
            roomId: parseInt(roomId),
            roomName: data.roomName,
            gameName: data.game,
            roomUrl: `/game/${data.game}/room/${roomId}`
        });
        sessionStorage.setItem("joinedRooms", JSON.stringify(joinedRooms));

        window.location.href = `/game/${data.game}/room/${roomId}`;
    }
    if (!res.ok) {
    const error = await res.text();
    alert("Error: " + error);
    }
});