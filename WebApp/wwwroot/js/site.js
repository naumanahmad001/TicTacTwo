$(document).ready(function () {
    const gridSize = 6;
    const matrixSize = 3;
    const playerSymbols = ['X', 'O'];
    let currentPlayer = 0; // Start with player 1
    let matrixStartRow = 0;
    let matrixStartCol = 0;

    // Initialize the grid
    const $grid = $('#grid');
    for (let i = 0; i < gridSize; i++) {
        for (let j = 0; j < gridSize; j++) {
            const $cell = $('<div class="cell" data-row="' + i + '" data-col="' + j + '"></div>');
            $grid.append($cell);
        }
    }

    // Highlight the active matrix
    function highlightMatrix() {
        $('.cell').removeClass('active');
        for (let i = matrixStartRow; i < matrixStartRow + matrixSize; i++) {
            for (let j = matrixStartCol; j < matrixStartCol + matrixSize; j++) {
                $('[data-row="' + i + '"][data-col="' + j + '"]').addClass('active');
            }
        }
    }

    // Check if the move is valid
    function isValidMove(rowOffset, colOffset) {
        const newRowStart = matrixStartRow + rowOffset;
        const newColStart = matrixStartCol + colOffset;
        return (
            newRowStart >= 0 &&
            newColStart >= 0 &&
            newRowStart + matrixSize <= gridSize &&
            newColStart + matrixSize <= gridSize
        );
    }

    // Move the matrix
    function moveMatrix(rowOffset, colOffset) {
        if (isValidMove(rowOffset, colOffset)) {
            matrixStartRow += rowOffset;
            matrixStartCol += colOffset;
            highlightMatrix();
        }
    }

    // Handle cell click
    $('.cell').on('click', function () {
        const $cell = $(this);
        if ($cell.hasClass('active') && !$cell.hasClass('taken')) {
            $cell.text(playerSymbols[currentPlayer]);
            $cell.addClass('taken');

            // Check for win within the active matrix
            if (checkWin(playerSymbols[currentPlayer])) {
                alert('Player ' + (currentPlayer + 1) + ' (' + playerSymbols[currentPlayer] + ') wins!');
                $('.cell').off('click'); // Disable further moves
                return;
            }

            // Switch players
            currentPlayer = 1 - currentPlayer;
        }
    });

    // Reset the grid
    $('#reset').on('click', function () {
        $('.cell').removeClass('taken').text('');
        matrixStartRow = 0;
        matrixStartCol = 0;
        highlightMatrix();
        currentPlayer = 0; // Reset to player 1
        $('.cell').off('click').on('click', function () {
            const $cell = $(this);
            if ($cell.hasClass('active') && !$cell.hasClass('taken')) {
                $cell.text(playerSymbols[currentPlayer]);
                $cell.addClass('taken');
                if (checkWin(playerSymbols[currentPlayer])) {
                    alert('Player ' + (currentPlayer + 1) + ' (' + playerSymbols[currentPlayer] + ') wins!');
                    $('.cell').off('click');
                    return;
                }
                currentPlayer = 1 - currentPlayer;
            }
        });
    });

    // Movement controls
    $('#move-up').on('click', function () {
        moveMatrix(-1, 0);
    });
    $('#move-down').on('click', function () {
        moveMatrix(1, 0);
    });
    $('#move-left').on('click', function () {
        moveMatrix(0, -1);
    });
    $('#move-right').on('click', function () {
        moveMatrix(0, 1);
    });

    // Function to check for win
    function checkWin(symbol) {
        const cells = $('.cell');
        const rows = [[], [], []];
        const cols = [[], [], []];
        const diag1 = [];
        const diag2 = [];

        cells.each(function () {
            const row = $(this).data('row');
            const col = $(this).data('col');
            const value = $(this).text();

            if (
                row >= matrixStartRow &&
                row < matrixStartRow + matrixSize &&
                col >= matrixStartCol &&
                col < matrixStartCol + matrixSize &&
                value === symbol
            ) {
                rows[row - matrixStartRow].push(true);
                cols[col - matrixStartCol].push(true);
                if (row - matrixStartRow === col - matrixStartCol) diag1.push(true);
                if (row - matrixStartRow + col - matrixStartCol === matrixSize - 1) diag2.push(true);
            }
        });

        return (
            rows.some(row => row.length === matrixSize) ||
            cols.some(col => col.length === matrixSize) ||
            diag1.length === matrixSize ||
            diag2.length === matrixSize
        );
    }

    // Initial matrix highlight
    highlightMatrix();
});