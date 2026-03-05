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
    csgo: "CS2",
    valorant: "Valorant",
    ow: "Overwatch",
    lol: "LoL",
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
    form.action = `/game/${encodeURIComponent(gameList[currentGameKey])}`;

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
const canvas = document.getElementById("particles");
const ctx = canvas.getContext("2d");

canvas.width = window.innerWidth;
canvas.height = window.innerHeight;

let particles = [];

for(let i=0;i<60;i++){
    particles.push({
        x:Math.random()*canvas.width,
        y:Math.random()*canvas.height,
        r:Math.random()*2+1,
        dx:(Math.random()-0.5)*0.5,
        dy:(Math.random()-0.5)*0.5
    });
}

function animateParticles(){
    ctx.clearRect(0,0,canvas.width,canvas.height);

    particles.forEach(p=>{
        p.x+=p.dx;
        p.y+=p.dy;

        ctx.beginPath();
        ctx.arc(p.x,p.y,p.r,0,Math.PI*2);
        ctx.fillStyle="rgba(170,0,193,0.7)";
        ctx.fill();

        if(p.x<0||p.x>canvas.width)p.dx*=-1;
        if(p.y<0||p.y>canvas.height)p.dy*=-1;
    });

    requestAnimationFrame(animateParticles);
}

animateParticles();
renderGameCards();