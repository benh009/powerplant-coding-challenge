namespace PowerPlantAPI.Services
{
    public class PreComputedValue
    {
        public decimal Price { get; set; }
        public  PreComputedProductionPlants[] ProductionPlants { get; set; }

        public static PreComputedValue[,] InitArray(int iSize, int jSize)
        {
            var precalcul = new PreComputedValue[iSize, jSize];
            for (int i = 0; i < iSize; i++)
            {
                for (int j = 0; j < jSize; j++)
                {
                    precalcul[i, j] = new PreComputedValue() { Price = -1, ProductionPlants = new PreComputedProductionPlants[jSize] };
                    for (int k = 0; k < jSize; k++)
                    {
                        precalcul[i, j].ProductionPlants[k] = new PreComputedProductionPlants() { Power = -1 };
                    }
                }
            }
            return precalcul;
        }
    }
}
