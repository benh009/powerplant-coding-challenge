namespace PowerPlantAPI.Models
{
    public class PowerPlantTurboJet : PowerPlant
    {
        public PowerPlantTurboJet(decimal energyPrice)
        {
            this.EnergyPrice = energyPrice;
        }
    }
}
