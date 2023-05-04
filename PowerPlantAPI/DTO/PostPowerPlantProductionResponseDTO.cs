using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerPlantAPI.Models
{
    public class PostPowerPlantProductionResponseDTO
    {
        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; }
        [JsonPropertyName("p")]
        [Range(0.0, double.MaxValue)]
        [Required]
        public decimal Production { get; set; }
    }
}
