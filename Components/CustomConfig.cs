namespace Components
{
    public class CustomConfig
    {
        // Properties for the CustomConfig class

        public string ConfigName { get; set; }
        public int BoardSize { get; set; }
        public int GridSize { get; set; }
        public int NumPieces { get; set; }
        public string PlayerOnePiece { get; set; }
        public string PlayerTwoPiece { get; set; }

        // Static method to create a new game configuration
        public static CustomConfig CreateGameConfig()
        {
            // Declare properties that will hold user input
            string playerOnePiece;
            string playerTwoPiece;

            // Prompt for Configuration Name
            string configName;
            while (true)
            {
                Console.Write("Enter a name for the configuration: ");
                configName = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(configName))
                {
                    break;
                }

                Console.WriteLine("Invalid input. Configuration name cannot be empty.");
            }

            // Prompt for Board Size
            int boardSize;
            while (true)
            {
                Console.Write("Enter the board size (N for an N x N board): ");
                if (int.TryParse(Console.ReadLine(), out boardSize) && boardSize > 0)
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a positive integer for the board size.");
            }

            // Prompt for Grid Size
            int gridSize;
            while (true)
            {
                Console.Write("Enter the grid size (M for an M x M grid): ");
                if (int.TryParse(Console.ReadLine(), out gridSize) && gridSize > 0 && gridSize <= boardSize)
                {
                    break;
                }

                Console.WriteLine("Invalid input. Grid size must be a positive integer and less than or equal to the board size.");
            }

            // Prompt for Number of Pieces
            int numPieces;
            while (true)
            {
                Console.Write("Enter the number of pieces per player: ");
                if (int.TryParse(Console.ReadLine(), out numPieces) && numPieces > 0)
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a positive integer for the number of pieces.");
            }

            // Prompt for Player One's Piece (Single character string)
            while (true)
            {
                Console.Write("Enter the name for Player One's piece (e.g., 'X'): ");
                playerOnePiece = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(playerOnePiece) && playerOnePiece.Length == 1)
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a single character piece name for Player One.");
            }

            // Prompt for Player Two's Piece (Single character string)
            while (true)
            {
                Console.Write("Enter the name for Player Two's piece (e.g., 'O'): ");
                playerTwoPiece = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(playerTwoPiece) && playerTwoPiece.Length == 1)
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a single character piece name for Player Two.");
            }

            // Return a new instance of CustomConfig with the entered values
            return new CustomConfig
            {
                ConfigName = configName, // Assign the configuration name
                BoardSize = boardSize,   // Assign the board size
                GridSize = gridSize,     // Assign the grid size
                NumPieces = numPieces,   // Assign the number of pieces
                PlayerOnePiece = playerOnePiece, // Assign Player One's piece
                PlayerTwoPiece = playerTwoPiece  // Assign Player Two's piece
            };
        }
    }
}
