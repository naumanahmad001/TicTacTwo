using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Domain
{
    public class TempGameState : BaseEntity
    {
        public string GridTopLeft { get; set; } = default!;
        
        public string PositionsJson { get; set; } = default!;
        
        [NotMapped]
        public Dictionary<string, char>? Positions
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, char>>(PositionsJson);
            set => PositionsJson = JsonConvert.SerializeObject(value);
        }
        
        public int MoveNumber { get; set; }
        
        public string GameId { get; set; }
    }
}