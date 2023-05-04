using PowerPlantAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerPlantAPI.DTO
{
    public class PostPowerPlantRequestDTO
    {
        /// <summary>
        /// The load is the amount of energy (MWh) that need to be generated during one hour..
        /// </summary>
        [Required]
        [Range(0.0, double.MaxValue)]
        [JsonPropertyName("load")]
        public decimal? Load { get; set; }

        /// <summary>
        /// Based on the cost of the fuels of each powerplant, the merit-order can be determined which is the starting point for deciding which powerplants should be switched on and how much power they will deliver.
        /// </summary>
        [Required]
        [JsonPropertyName("fuels")]
        public EnergyPriceRequestDTO Fuels { get; set; }

        /// <summary>
        /// Describes the powerplants at disposal to generate the demanded load. For each powerplant is specified
        /// </summary>
        [Required]
        [JsonPropertyName("powerplants")]
        public IEnumerable<PowerPlantRequestDTO> PowerPlants { get; set; }
    }
}
