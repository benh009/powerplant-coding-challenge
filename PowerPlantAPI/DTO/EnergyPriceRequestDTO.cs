using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerPlantAPI.Models
{
    public class EnergyPriceRequestDTO
    {
        [Range(0.0, double.MaxValue)]
        [JsonPropertyName("gas(euro/MWh)")]
        public decimal? Gas { get; set; }
        [Required(AllowEmptyStrings =false)]
        [Range(0.0,double.MaxValue)]
        [JsonPropertyName("kerosine(euro/MWh)")]
        public decimal? Kerosine { get; set; }
        [Required]
        [Range(0.0, double.MaxValue )]
        [JsonPropertyName("co2(euro/ton)")]
        public decimal? CO2 { get; set; }
        [Required]
        [Range(0.0, 100)]
        [JsonPropertyName("wind(%)")]
        public decimal? Wind { get; set; }
    }
}
