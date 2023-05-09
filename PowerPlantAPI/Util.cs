using PowerPlantAPI.Services;

namespace PowerPlantAPI
{
    public class Util
    {
        public static PreCalculedValue[,] InitArray(int iSize, int jSize)
        {
            var precalcul = new PreCalculedValue[iSize, jSize];
            for (int i = 0; i < iSize; i++)
            {
                for (int j = 0; j < jSize; j++)
                {
                    precalcul[i, j] = new PreCalculedValue() { Price = -1, ProductionPlants = new PreCalculedProductionPlants[jSize] };
                    for (int k = 0; k < jSize; k++)
                    {
                        precalcul[i, j].ProductionPlants[k] = new PreCalculedProductionPlants() { Power = -1 };
                    }
                }
            }
            return precalcul;
        }
    }
}
