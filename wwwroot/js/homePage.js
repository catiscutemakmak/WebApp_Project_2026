const gameImg = {
    csgo: "/images/bg - counter.png",
    valorant: "/images/bg - valo.png",
    ow: "/images/bg - ow2.png",
    lol: "/images/bg - lol.png",
    mlbb: "/images/bg - mlbb.png",
    rov: "/images/bg - rov.png",
    amongus: "/images/bg - among us.png",
    peak: "/images/bg - peak.png",
    pubg: "/images/bg - pubg.png",
};

const gameList = {
    csgo: "Counter-Strike",
    valorant: "Valorant",
    ow: "Overwatch",
    lol: "League of Legends",
    mlbb: "Mobile Legends",
    rov: "RoV",
    amongus: "Among Us",
    peak: "Peak",
    pubg: "PUBG",
};

var currentGameKey = "";

function GameCard(key) {
    const div = document.createElement("div");
    div.classList.add("game-card");
    div.innerHTML = `
        <img class="game-card-img" src="${gameImg[key]}">
        <div class="game-card-name">${gameList[key]}</div>
    `;
    div.addEventListener("click", () => openModal(key));
    return div;
}

function renderGameCards() {
    const container = document.getElementById("game-cards-container");
    Object.keys(gameList).forEach(key => {
        container.appendChild(GameCard(key));
    });
}

function openModal(key) {
    currentGameKey = key;
    document.getElementById("modal-game-title").textContent = gameList[key];
    document.getElementById("modal-game-img").src = gameImg[key];
    document.getElementById("game-id-input").value = "";
    document.getElementById("input-error").style.display = "none";
    document.getElementById("game-modal").style.display = "flex";
}

function closeModal() {
    document.getElementById("game-modal").style.display = "none";
}

function submitGame() {
    var gameID = document.getElementById("game-id-input").value.trim();
    var error = document.getElementById("input-error");
    if (!gameID) {
        error.style.display = "block";
        return;
    }
    error.style.display = "none";

    var form = document.createElement("form");
    form.method = "POST";
    form.action = "/Match/Index";

    var inputGame = document.createElement("input");
    inputGame.type = "hidden";
    inputGame.name = "gameKey";
    inputGame.value = currentGameKey;

    var inputID = document.createElement("input");
    inputID.type = "hidden";
    inputID.name = "gameID";
    inputID.value = gameID;

    form.appendChild(inputGame);
    form.appendChild(inputID);
    document.body.appendChild(form);
    form.submit();
}

window.addEventListener("click", function(e) {
    if (e.target.id === "game-modal") {
        closeModal();
    }
});

renderGameCards();