namespace GameBrain;
using Components;

public class MoveVerifier
{
    public static bool IsValidMove(char[,] board, Grid grid, Dictionary<(int, int), char> pieces, int x, int y)
    {
        var boardSize = board.GetLength(0);
        (var gridX, var gridY) = grid.TopLeft;
        var gridSize = grid.Size;

        if (gridX <= x && x < gridX + gridSize && gridY <= y && y < gridY + gridSize)
            return !pieces.ContainsKey((x, y));

        return false;
    }
    
    public static bool IsValidGridMove(Grid grid, string direction, int boardSize)
    {
        (var gridX, var gridY) = grid.TopLeft;
        var gridSize = grid.Size;

        if (direction == "up" && gridX > 0) return true;
        if (direction == "down" && gridX + gridSize < boardSize) return true;
        if (direction == "left" && gridY > 0) return true;
        if (direction == "right" && gridY + gridSize < boardSize) return true;

        return false;
    }
}