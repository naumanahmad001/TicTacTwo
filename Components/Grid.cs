namespace Components;

public class Grid
{
    public (int, int) TopLeft { get; set; }
    public int Size { get; set; }

    public static Grid InitGrid(int boardSize, int gridSize)
    {
        var gridStart = boardSize / 2 - gridSize / 2;
        
        return new Grid
        {
            TopLeft = (gridStart, gridStart),
            Size = gridSize
        };
    }
}