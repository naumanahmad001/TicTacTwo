namespace Components;
public class Board
{
    public static char[,] InitBoard(int boardSize)
    {
        var board = new char[boardSize, boardSize];
        for (var i = 0; i < boardSize; i++)
        for (var j = 0; j < boardSize; j++)
            board[i, j] = '.'; 
        return board; 
    }

    public static void PrintBoard(char[,] board, Grid grid, Dictionary<(int, int), char> pieces)
    {
        var boardSize = board.GetLength(0);
        var gridSize = grid.Size;
        (var gridX, var gridY) = grid.TopLeft;

        var displayBoard = (char[,]) board.Clone();
        
        foreach (var piece in pieces)
            displayBoard[piece.Key.Item1, piece.Key.Item2] = piece.Value;
        
        for (var i = 0; i < gridSize; i++)
        for (var j = 0; j < gridSize; j++)
        {
            var x = gridX + i;
            var y = gridY + j;
            if (displayBoard[x, y] == '.')
                displayBoard[x, y] = '\u25a2';
        }
        
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++) Console.Write(displayBoard[i, j] + " ");

            Console.WriteLine();
        }
        
        Console.WriteLine();
    }
}