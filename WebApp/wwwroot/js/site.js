"use strict";
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub")
    .build();
hubConnection.start();

let playerName = "";
let gridSize = 6; // Default grid size
let isPlayerTurn = false;
let matrixStartRow = 0;                  // Initial matrix top-left position
let matrixStartCol = 0;
// Initialize Grid
function initializeGrid() {
    const grid = document.getElementById("grid");
    grid.style.gridTemplateColumns = `repeat(${gridSize}, 1fr)`;

    grid.innerHTML = ""; // Clear existing grid
    for (let i = 0; i < gridSize; i++) {
        for (let j = 0; j < gridSize; j++) {
            const cell = document.createElement("div");
            cell.classList.add("cell");
            cell.dataset.row = i;
            cell.dataset.col = j;

            cell.addEventListener("click", () => makeMove(cell));
            grid.appendChild(cell);
        }
    }
}

function saveGameStateToServer() {
    const positions = {};
    $('.cell').each(function () {
        const row = $(this).data('row');
        const col = $(this).data('col');
        const text = $(this).text();

        if (text) {
            positions[`${row},${col}`] = text;
        }
    });

    const payload = {
        gameSaveName: document.getElementById("gameName").value,
        positions: positions,
        grid: {
            topLeft: `${matrixStartRow},${matrixStartCol}`
        }
    };

    $.ajax({
        url: '/api/Game/SaveTempGameState',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function () {
            console.log("Game state saved successfully");
            hubConnection.invoke("NotifyMove", `${playerName} has made a move.`);
        },
        error: function (err) {
            console.error("Failed to save game state:", err);
        }
    });
}
// Handle joining the game
document.getElementById("join-game").addEventListener("click", () => {
    playerName = prompt("Enter your name:");
    if (playerName) {
        hubConnection.invoke("JoinGame", playerName);
    }
});

// Make a move
function makeMove(cell) {
    if (!isPlayerTurn || cell.classList.contains("taken")) return;

    cell.innerText = playerName === "Player X" ? "X" : "O";
    cell.classList.add("taken");
    isPlayerTurn = false;

    updateState();
}

// Update and send the current grid state
function updateState() {
    const cells = document.querySelectorAll(".cell");
    const gameState = Array.from(cells).map(cell => ({
        row: cell.dataset.row,
        col: cell.dataset.col,
        value: cell.innerText
    }));

    hubConnection.invoke("UpdateState", JSON.stringify(gameState));
    hubConnection.invoke("NotifyMove", `${playerName} has made a move.`);
}

// Receive updated state from other players
hubConnection.on("ReceiveState", (gameState) => {
    const state = JSON.parse(gameState);

    state.forEach(cellState => {
        const cell = document.querySelector(`[data-row='${cellState.row}'][data-col='${cellState.col}']`);
        if (cellState.value) {
            cell.innerText = cellState.value;
            cell.classList.add("taken");
        }
    });

    isPlayerTurn = true;
    document.getElementById("turn-notification").innerText = "It's your turn!";
});

// Notification for moves
hubConnection.on("MoveNotification", (message) => {
    document.getElementById("status").innerText = message;
    window.location = window.location.href;
});

// Player joined notification
hubConnection.on("PlayerJoined", (name) => {
    document.getElementById("status").innerText = `${name} joined the game!`;
    if (!isPlayerTurn) isPlayerTurn = true;
});

// Start connection and initialize grid
hubConnection.start().then(() => {
    initializeGrid();
    console.log("Connection established.");
}).catch(err => console.error(err));