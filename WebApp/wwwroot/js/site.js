const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub")
    .build();
//hubConnection.start();
// Start connection and initialize grid
hubConnection.start().then(() => {
    //initializeGrid();
    console.log("Connection established.");
}).catch(err => console.error(err));


hubConnection.onclose(() => {
    console.error("SignalR connection closed");
});

//hubConnection.start().then(() => {
//    console.log("SignalR connected successfully");
//}).catch(err => {
//    console.error("Error connecting to SignalR:", err);
//});

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
        gameId: document.getElementById("gameId").value,
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
            //hubConnection.invoke("NotifyMove", `${playerName} has made a move.`);
            hubConnection.invoke("NotifyMove", "Test move notification").catch(err => console.error(err));

        },
        error: function (err) {
            console.error("Failed to save game state:", err);
        }
    });
}

function saveGameStateToServerFinal() {
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
        gameId: document.getElementById("gameId").value,
        positions: positions,
        grid: {
            topLeft: `${matrixStartRow},${matrixStartCol}`
        },
        skipDeleteTempStates: false
    };

    $.ajax({
        url: '/api/Game/SaveGameState',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function () {
            console.log("Game state saved successfully");
            showAlert('Game state saved successfully!', 'success');
            const updatedUrl = removeQueryStringParameter(window.location.href, 'reload', 1);
            window.location = updatedUrl;
        },
        error: function (err) {
            console.error("Failed to save game state:", err);
        }
    });
}

function saveGameStateToServerOnWin() {
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
        gameId: document.getElementById("gameId").value,
        positions: positions,
        grid: {
            topLeft: `${matrixStartRow},${matrixStartCol}`
        },
        skipDeleteTempStates: true
    };

    $.ajax({
        url: '/api/Game/SaveGameState',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function () {
            console.log("Game state saved successfully");
            //showAlert('Game state saved successfully!', 'success');
        },
        error: function (err) {
            console.error("Failed to save game state:", err);
        }
    });
}

function showAlert(message, type) {
    // Create the alert HTML
    const alertHtml = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;

    // Append the alert to the body or a specific container
    const alertContainer = document.getElementById('alert-container') || document.body;
    const alertElement = document.createElement('div');
    alertElement.innerHTML = alertHtml;
    alertContainer.appendChild(alertElement);

    // Automatically remove the alert after 5 seconds
    setTimeout(() => {
        alertElement.querySelector('.alert').classList.remove('show');
        setTimeout(() => alertContainer.removeChild(alertElement), 150); // Wait for fade-out animation
    }, 5000);
}
// Player joined notification
hubConnection.on("PlayerJoined", (name) => {
    document.getElementById("status").innerText = `${name} joined the game!`;
    if (!isPlayerTurn) isPlayerTurn = true;
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


function updateQueryStringParameter(uri, key, value) {
    const re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    const separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    } else {
        return uri + separator + key + "=" + value;
    }
}

function removeQueryStringParameter(uri, key) {
    // Use a regular expression to find the key and its value in the query string
    const re = new RegExp("([?&])" + key + "=[^&]*(&?)", "i");
    uri = uri.replace(re, (match, p1, p2) => p1 === '?' && p2 ? '?' : p1); // Handle edge cases for '?' and '&'
    // Remove trailing '?' or '&' if present
    return uri.replace(/[?&]$/, '');
}

// Notification for moves
hubConnection.on("MoveNotification", (message) => {
    const updatedUrl = updateQueryStringParameter(window.location.href, 'reload', 1);
    window.location = updatedUrl;

});

// Notification Reload Saved Game
hubConnection.on("ReloadSavedGame", (url) => {
    window.location = removeQueryStringParameter(url, "reload");;
});
// Handle joining the game
const joinGameButton = document.getElementById("join-game");
if (joinGameButton) {
    joinGameButton.addEventListener("click", () => {
        const playerName = prompt("Enter your name:");
        if (playerName) {
            hubConnection.invoke("JoinGame", playerName)
                .catch(err => console.error("Error invoking JoinGame:", err));
        }
    });
} else {
    console.warn("join-game element not found");
}


