using Components;

namespace GameBrain

{
    public class Placer
    {
        public static void PlacePiece(char player, char[,] board, Grid grid, Dictionary<(int, int), char> pieces, int numPieces)
        {
            if (CountPlayerPieces(pieces, player) >= numPieces)
            {
                Console.WriteLine("You have placed all your pieces. Choose another option.");
                return;
            }

            Console.Write("Enter position to place piece (row, col): ");
            var pos = Console.ReadLine().Split(',');
            var x = int.Parse(pos[0]);
            
            var y = int.Parse(pos[1]);

            if (MoveVerifier.IsValidMove(board, grid, pieces, x, y))
            {
                
                pieces[(x, y)] = player;
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
                PlacePiece(player, board, grid, pieces, numPieces); 
            }
        }

        private static int CountPlayerPieces(Dictionary<(int, int), char> pieces, char player)
        {
            var count = 0;
            foreach (var piece in pieces.Values)
                if (piece == player)
                    count++;

            return count;
        }
    }
}