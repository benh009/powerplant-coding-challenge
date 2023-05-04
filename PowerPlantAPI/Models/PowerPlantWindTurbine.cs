namespace PowerPlantAPI.Models
{
    public class PowerPlantWindTurbine : PowerPlant
    {

        private readonly decimal WindPercentage;
        public PowerPlantWindTurbine(decimal windPercentage)
        {
            EnergyPrice = 0;
            WindPercentage = windPercentage;
        }
        public override decimal MaxProduction => PowerMax * WindPercentage / 100;
    }
}
