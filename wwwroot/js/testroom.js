const PlayerMain = document.getElementById("roomContainer");

let offset = 2;

const visibleSlots = 5;
const centerIndex = 2;
const maxPlayers = 8;

const roomData = {
    ownerUsername: "SKYKY",
    players: [
        {name:"SKYKY",profile:"/images/profile/0abeee40-d8ff-4e81-a46d-d01a9b610b5d.avif",player_rank:147,roleName:"Controller"},
        {name:"Alice",profile:"/images/profile/7.jpg",player_rank:100,roleName:"Duelist"},
        {name:"Bob",profile:"https://i.pinimg.com/736x/bf/6e/29/bf6e296386c67b027cd3d234e3c6efa4.jpg",player_rank:90,roleName:"Initiator"},
        {name:"Eve",profile:"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQFBo6bythwEPQHLVrQUDTLl-bVfJ4MnxRDWQ&s",player_rank:80,roleName:"Sentinel"},
        {name:"Tom",profile:"https://pbs.twimg.com/profile_images/378800000357127977/a826b950ea6dee6f691696c1e31b78b7_400x400.jpeg",player_rank:70,roleName:"Controller"},
        {name:"Ken",profile:"https://st-th-1.byteark.com/assets.punpro.com/contents/i9149/1590488436656-Image-1.jpg",player_rank:60,roleName:"Duelist"}
    ]
};

renderRooms(roomData);

function renderRooms(room) {

    PlayerMain.innerHTML = "";

    const slots = new Array(visibleSlots).fill(null);

    const owner = room.players.find(p => p.name === room.ownerUsername);
    const others = room.players.filter(p => p.name !== room.ownerUsername);

    const emptySlots = maxPlayers - room.players.length;

    const circle = [...others];

    for(let i=0;i<emptySlots;i++){
        circle.push(null);
    }

    const total = circle.length;

    // ⭐ แก้ตรงนี้
    offset = ((offset % total) + total) % total;

    slots[centerIndex] = owner;

    for(let i=1;i<=2;i++){

        const leftIndex = (offset - i + total) % total;
        const rightIndex = (offset + i - 1) % total;

        slots[centerIndex - i] = circle[leftIndex];
        slots[centerIndex + i] = circle[rightIndex];

    }

    slots.forEach(p => {
        if(p){
            PlayerMain.appendChild(PlayerCard(p, room.ownerUsername));
        }else{
            PlayerMain.appendChild(EmptySlot());
        }
    });
}

function PlayerCard(player, ownerUsername) {

    const div = document.createElement("div");
    div.classList.add("player-main");

    const card = document.createElement("div");
    card.classList.add("card");

    if(player.name === ownerUsername){
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

function EmptySlot(){

    const div = document.createElement("div");
    div.classList.add("slot");
    div.innerText = "+";

    return div;
}

document.getElementById("leftBtn").onclick = () => {

    offset--;

    renderRooms(roomData);

};

document.getElementById("rightBtn").onclick = () => {

    offset++;

    renderRooms(roomData);

};