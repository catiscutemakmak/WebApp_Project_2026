
const PlayerMain = document.getElementById("roomContainer");

renderRooms({
    ownerUsername: "SKYKY",
    players: [
        {
            name:"SKYKY",
            profile:"/images/profile/0abeee40-d8ff-4e81-a46d-d01a9b610b5d.avif",
            player_rank:147,
            roleName:"Controller"
        },
        {
            name:"Alice",
            profile:"/images/profile/a4a314c2-61a7-4d43-b28d-fe277fdbae93.jpg",
            player_rank:100,
            roleName:"Duelist"
        },
        {
            name:"Bob",
            profile:"/images/profile/539021cf-a592-461e-b9a7-4ed8a132982f.jpg",
            player_rank:90,
            roleName:"Initiator"
        }
    ]
});

function renderRooms(room) {

    PlayerMain.innerHTML = "";

    const maxSlots = 5;
    const slots = new Array(maxSlots).fill(null);

    const owner = room.players.find(p => p.name === room.ownerUsername);
    const others = room.players.filter(p => p.name !== room.ownerUsername);

    // owner อยู่กลาง
    slots[2] = owner;

    let left = 1;
    let right = 3;

    others.forEach((p, i) => {

        if (i % 2 === 0) {
            slots[left] = p;
            left--;
        } else {
            slots[right] = p;
            right++;
        }

    });

    slots.forEach(p => {

        if (p) {
            PlayerMain.appendChild(PlayerCard(p, room.ownerUsername));
        } else {
            PlayerMain.appendChild(EmptySlot());
        }

    });

}

function PlayerCard(player, ownerUsername) {

    const div = document.createElement("div");
    div.classList.add("player-main");

    const card = document.createElement("div");
    card.classList.add("card");

    if (player.name === ownerUsername) {
        card.classList.add("owner");
    }

    const img = document.createElement("img");
    img.src = player.profile;

    const level = document.createElement("div");
    level.classList.add("player-level");
    level.innerText = player.player_rank;

    const name = document.createElement("div");
    name.classList.add("player-name");
    name.innerText = player.name;

    const title = document.createElement("div");
    title.classList.add("player-title");
    title.innerText = player.roleName;

    card.appendChild(img);
    card.appendChild(level);
    card.appendChild(name);
    card.appendChild(title);

    div.appendChild(card);

    return div;
}

function EmptySlot() {

    const div = document.createElement("div");
    div.classList.add("slot");
    div.innerText = "+";

    return div;
}