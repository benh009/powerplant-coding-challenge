
namespace PowerPlantAPI.Models
{
    public class PowerPlant
    {
        public decimal EnergyPrice { get; set; }
        public string Name { get; set; }
        public PowerPlantType Type { get; set; }
        public decimal Efficiency { get; set; }
        public decimal PowerMin { get; set; }
        public decimal PowerMax { get; set; }
        public decimal PricePerMWH => EnergyPrice / Efficiency;
        public decimal Power { get; set; }
        public virtual decimal MaxProduction => PowerMax;
    }
}
