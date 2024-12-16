using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Domain
{
    public class Game : BaseEntity
    {
        public string GameName { get; set; } = default!;
        
        public string? GridTopLeft { get; set; } 
        
        public string? PositionsJson { get; set; } 
        
        [NotMapped]
        public Dictionary<string, char>? Positions
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, char>>(PositionsJson);
            set => PositionsJson = JsonConvert.SerializeObject(value);
        }
        
        public int? ConfigId { get; set; }
        public Config? Config { get; set; }
        
        public string? FirstPlayerPassword { get; set; }
        
        public string? SecondPlayerPassword { get; set; }
        
        
    }
}