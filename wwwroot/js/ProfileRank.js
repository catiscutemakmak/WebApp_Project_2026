document.querySelector(".submit-btn").addEventListener("click", () => {
  alert("Profile Submitted!");
});

const rankMap = {
    mlbb: ["Warrior", "Elite", "Master", "Grandmaster", "Epic", "Legend", "Mythic"],
    rov: ["Bronze", "Silver", "Gold", "Platinum", "Diamond", "Conqueror"],
    valorant: ["Iron", "Bronze", "Silver", "Gold", "Platinum", "Diamond", "Ascendant", "Immortal", "Radiant"],
    csgo: ["Silver", "Gold Nova", "Master Guardian", "Legendary Eagle", "Global Elite"],
    overwatch: ["Bronze", "Silver", "Gold", "Platinum", "Diamond", "Master", "Grandmaster"],
    pubg: ["Bronze", "Silver", "Gold", "Platinum", "Diamond", "Crown", "Ace"],
    amongus: ["None"],
    lol: ["Iron", "Bronze", "Silver", "Gold", "Platinum", "Diamond", "Master", "Grandmaster", "Challenger"]
};

document.querySelectorAll(".game-select").forEach(gameSelect => {
    gameSelect.addEventListener("change", () => {
        const rankSelect = document.getElementById(gameSelect.dataset.rank);
        const ranks = rankMap[gameSelect.value];

        rankSelect.innerHTML =
            `<option value="" disabled selected hidden>Choose Rank</option>`;

        ranks.forEach(rank => {
            const option = document.createElement("option");
            option.value = rank;
            option.textContent = rank;
            rankSelect.appendChild(option);
        });

        gameSelect.style.color = "#fff";

    });
});

// PHONE

const phoneInput = document.getElementById('phoneInput');

phoneInput.addEventListener('input', (e) => {
    let value = e.target.value.replace(/\D/g, '');
    
    if (value.length > 10) {
        value = value.slice(0, 10);
    }

    let formattedValue = '';
    
    if (value.length > 0) {
        if (value.length <= 3) {
            formattedValue = value;
        } else if (value.length <= 6) {

            formattedValue = value.slice(0, 3) + '-' + value.slice(3);
        } else {
     
            formattedValue = value.slice(0, 3) + '-' + value.slice(3, 6) + '-' + value.slice(6);
        }
    }
    e.target.value = formattedValue;
});