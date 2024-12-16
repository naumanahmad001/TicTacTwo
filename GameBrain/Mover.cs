using Components;

namespace GameBrain

{
    public class Mover
    {
        public static void MovePiece(char player, char[,] board, Grid grid, Dictionary<(int, int), char> pieces)
        {
            Console.Write("Enter the piece position to move (row, col): ");
            var fromPos = Console.ReadLine().Split(',');

            if (fromPos.Length == 2 &&
                int.TryParse(fromPos[0], out var fromX) &&
                int.TryParse(fromPos[1], out var fromY))
            {
                if (pieces.ContainsKey((fromX, fromY)))
                {
                    if (pieces[(fromX, fromY)] == player)
                    {
                        Console.Write("Enter the new position for the piece (row, col): ");
                        var toPos = Console.ReadLine().Split(',');

                        if (toPos.Length == 2 &&
                            int.TryParse(toPos[0], out var toX) &&
                            int.TryParse(toPos[1], out var toY) &&
                            MoveVerifier.IsValidMove(board, grid, pieces, toX, toY))
                        {
                            pieces.Remove((fromX, fromY));
                            pieces[(toX, toY)] = player;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move. Try again.");
                            Board.PrintBoard(board, grid, pieces);
                            MovePiece(player, board, grid, pieces); 
                        }
                    }
                    else
                    {
                        Console.WriteLine("You can only move your own pieces!");
                        Board.PrintBoard(board, grid, pieces);
                        MovePiece(player, board, grid, pieces); 
                    }
                }
                else
                {
                    Console.WriteLine("No piece exists at the specified position. Try again.");
                    Board.PrintBoard(board, grid, pieces);
                    MovePiece(player, board, grid, pieces); 
                }
            }
            else
            {
                Console.WriteLine("Invalid input format. Please enter valid row and column numbers.");
                Board.PrintBoard(board, grid, pieces);
                MovePiece(player, board, grid, pieces); 
            }
        }

        public static void MoveGridInput(Grid grid, char[,] board)
        {
            Console.Write("Enter the direction to move the grid (up/down/left/right): ");
            var direction = Console.ReadLine().Trim().ToLower();

            if (MoveVerifier.IsValidGridMove(grid, direction, board.GetLength(0)))
            {
                MoveGrid(grid, direction);
            }
            else
            {
                Console.WriteLine("Unable to move the grid, the border is in the way");
                MoveGridInput(grid, board);
            }
        }

        
        public static void MoveGrid(Grid grid, string direction)
        {
            if (direction == "up") grid.TopLeft = (grid.TopLeft.Item1 - 1, grid.TopLeft.Item2);
            if (direction == "down") grid.TopLeft = (grid.TopLeft.Item1 + 1, grid.TopLeft.Item2);
            if (direction == "left") grid.TopLeft = (grid.TopLeft.Item1, grid.TopLeft.Item2 - 1);
            if (direction == "right") grid.TopLeft = (grid.TopLeft.Item1, grid.TopLeft.Item2 + 1);
        }
    }
}