namespace PowerPlantAPI.Models
{
    public class PowerPlantRequest
    {
        public decimal Load { get; set; }
        public EnergyPrice Fuels { get; set; }
        public IEnumerable<PowerPlant> PowerPlants { get; set; }
    }
}
