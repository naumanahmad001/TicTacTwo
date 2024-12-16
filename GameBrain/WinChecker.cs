namespace GameBrain;
using Components;

public class WinChecker
{
    public static char CheckWin(Dictionary<(int, int), char> pieces, Grid grid)
    {
        (var gridX, var gridY) = grid.TopLeft;
        var gridSize = grid.Size;

        for (var i = 0; i < gridSize; i++)
        {
            for (var j = 0; j < gridSize; j++)
            {
                var x = gridX + i;
                var y = gridY + j;
                if (pieces.ContainsKey((x, y)))
                {
                    var player = pieces[(x, y)];

                    if (CheckLineWin(x, y, 1, 0, player, pieces, grid) || 
                        CheckLineWin(x, y, 0, 1, player, pieces, grid) || 
                        CheckLineWin(x, y, 1, 1, player, pieces, grid) || 
                        CheckLineWin(x, y, 1, -1, player, pieces, grid)) 
                    {
                        return player;
                    }
                }
            }
        }

        return ' ';
    }
    
    public static bool CheckLineWin(int startX, int startY, int dx, int dy, char player, Dictionary<(int, int), char> pieces, Grid grid)
    {
        (var gridX, var gridY) = grid.TopLeft;
        var gridSize = grid.Size;

        for (var k = 0; k < 3; k++)
        {
            var x = startX + k * dx;
            var y = startY + k * dy;

            if (x < gridX || x >= gridX + gridSize || y < gridY || y >= gridY + gridSize || !pieces.ContainsKey((x, y)) || pieces[(x, y)] != player)
                return false;
        }

        Console.Beep();
        Console.Beep();
        Console.Beep();

        return true;
    }
}