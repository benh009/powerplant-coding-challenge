namespace PowerPlantAPI.Models
{
    public class PowerPlantGasFired : PowerPlant
    {
        public PowerPlantGasFired(decimal energyPrice)
        {
            this.EnergyPrice = energyPrice;
        }
    }
}
