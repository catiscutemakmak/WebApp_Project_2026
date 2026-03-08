let selectedAvatar = null;
let selectedRoomId = null;
let pendingRoomData = null;
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

/* =========================
   DATE + TIME PICKER
========================= */

flatpickr("#play-date", {
    disableMobile: true,
    altInput: true,
    altFormat: "d M Y",
    dateFormat: "Y-m-d",
    minDate: "today"
});

flatpickr("#play-time", {
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    time_24hr: true,
    minuteIncrement: 1
});

/* =========================
   GAME DATA
========================= */

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
    },
    "Peak": {
        modes: ["Classic"],
        servers: ["Asia"],
        ranks: ["Any"],
        role: ["Any"]
    }
};

/* =========================
   SELECT ELEMENTS
========================= */

const gameSelect = document.getElementById("game-select");
const modeSelect = document.getElementById("game-mode");
const serverSelect = document.getElementById("game-server");
const minRank = document.getElementById("min-rank");
const maxRank = document.getElementById("max-rank");
const roleSelect = document.getElementById("game-role");
const roleContainer = document.getElementById("role-container");
const rankRequirement = document.getElementById("rank-requirement");
const playerSelect = document.getElementById("player-select");

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

function combineDateTime(date,time){
    return date + "T" + time;
}

/* =========================
   GAME CHANGE
========================= */

gameSelect.addEventListener("change", () => {

    const game = gameSelect.value;
    const data = gameData[game];

    if (!data) return;

    fillSelect(modeSelect,data.modes,"Choose Mode");
    fillSelect(serverSelect,data.servers,"Choose Server");

    if (data.role.length === 1 && data.role[0] === "Any") {

        roleContainer.style.display = "none";
        roleSelect.value = "Any";

    } else {

        roleContainer.style.display = "block";
        fillSelect(roleSelect,data.role,"Choose Role");

    }

    if (data.ranks.length === 1 && data.ranks[0] === "Any") {

        rankRequirement.style.display = "none";
        minRank.value = "Any";
        maxRank.value = "Any";

    } else {

        rankRequirement.style.display = "block";
        fillSelect(minRank,data.ranks,"Min Rank");
        fillSelect(maxRank,data.ranks,"Max Rank");

    }

});

/* =========================
   RANK VALIDATION
========================= */

maxRank.addEventListener("change",()=>{

    if(minRank.selectedIndex > maxRank.selectedIndex){
        minRank.selectedIndex = maxRank.selectedIndex;
    }

});

/* =========================
   PLAYER RANGE
========================= */

gameSelect.addEventListener("change", function () {

    let gameName = this.value;

    fetch(`/CreateTeam/GetPlayerRange?gameName=${gameName}`)
    .then(res=>res.json())
    .then(data=>{

        playerSelect.innerHTML =
        '<option disabled selected hidden>Choose Player</option>';

        for(let i=data.min;i<=data.max;i++){

            let option = document.createElement("option");

            option.value = i;
            option.textContent = i + " Players";

            playerSelect.appendChild(option);

        }

    });

});

/* =========================
   FORM SUBMIT
========================= */

const form = document.getElementById("create-room-form");

form.addEventListener("submit", async function(e){

    e.preventDefault();

    const formData = new FormData(form);

    const data = {
        game: formData.get("game"),
        roomName: formData.get("roomName"),
        gameMode: formData.get("gameMode"),
        gameServer: formData.get("gameServer"),
        minRank: formData.get("minRank"),
        maxRank: formData.get("maxRank"),
        maxPlayer: formData.get("maxPlayer"),
        roomPrivacy: formData.get("RoomPrivacy")==="true",
        gameRole: formData.get("gameRole"),
        description: formData.get("description"),

        playDateTime: combineDateTime(
            formData.get("playDate"),
            formData.get("playTime")
        )
    };
    console.log(data.game);
    // ⭐ ถ้าเป็น Among Us ให้เปิด avatar picker ก่อน
    if(data.game === "Among Us"){

        pendingRoomData = data;
        openAvatarPicker();
        return;

    }

    // ⭐ เกมอื่นสร้างห้องเลย
    createRoom(data);

});

/* =========================
   AVATAR PICKER
========================= */

function openAvatarPicker(){
    console.log("open avatar picker");
    const picker = document.getElementById("avatarPicker");
    const grid = document.getElementById("avatarGrid");
    const confirmBtn = document.getElementById("avatarConfirm");

    grid.innerHTML="";
    confirmBtn.disabled = true;

    amongUsAvatars.forEach(src=>{

        const img = document.createElement("img");

        img.src = src;

        img.onclick=()=>{

            document
            .querySelectorAll("#avatarGrid img")
            .forEach(i=>i.classList.remove("selected"));

            img.classList.add("selected");

            selectedAvatar = src;

            confirmBtn.disabled = false;

        };

        grid.appendChild(img);

    });

    picker.classList.remove("hide");

}

/* =========================
   AVATAR CONFIRM
========================= */

document.getElementById("avatarConfirm").onclick = async () => {
    console.log(pendingRoomData)
    if(!selectedAvatar || !pendingRoomData) return ;

    const confirmBtn = document.getElementById("avatarConfirm");
    confirmBtn.disabled = true;

    pendingRoomData.avatar = selectedAvatar;
    createRoom(pendingRoomData);

};

async function createRoom(data){
    console.log("data");
    const res = await fetch("/CreateTeam/CreateTeam",{

        method:"POST",
        headers:{
            "Content-Type":"application/json"
        },
        body:JSON.stringify(data)

    });

    if(!res.ok){

        const error = await res.text();
        alert(error);
        return;

    }

    const roomId = await res.text();

    window.location.href = `/game/${data.game}/room/${roomId}`;
}