namespace Domain
{
    public class Config : BaseEntity
    {
        public string ConfigName { get; set; } = default!;
        
        public int BoardSize { get; set; }
        
        public int GridSize { get; set; }

        public int PiecesAmount { get; set; }
        
        public string PlayerOnePiece { get; set; } = default!;  
        
        public string PlayerTwoPiece { get; set; } = default!;
        
        public ICollection<Game>? Games { get; set; }
    }
}