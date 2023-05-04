using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PowerPlantAPI.Models;

namespace PowerPlantAPI.DTO
{
    public class PowerPlantRequestDTO
    {
        [Required]
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [Required]
        [JsonPropertyName("Type")]
        public PowerPlantType? Type { get; set; }
        [Required]
        [JsonPropertyName("Efficiency")]
        [Range(0.0, 1)]
        public decimal? Efficiency { get; set; }
        [Required]
        [JsonPropertyName("Pmin")]
        [Range(0.0, double.MaxValue)]
        public decimal? PowerMin { get; set; }
        [Required]
        [Range(0.0, double.MaxValue)]
        [JsonPropertyName("Pmax")]
        public decimal? PowerMax { get; set; }
    }
}
