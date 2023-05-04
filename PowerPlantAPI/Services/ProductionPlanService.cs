using PowerPlantAPI.Models;

namespace PowerPlantAPI.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public IEnumerable<ProductionPlan> CreateProductionPlan(PowerPlantRequest powerPlantRequest)
        {

            var powerPlants = new List<PowerPlant>();
            foreach (var powerPlant in powerPlantRequest.PowerPlants)
            {
                switch (powerPlant.Type)
                {
                    case PowerPlantType.WindTurbine:
                        powerPlants.Add(new PowerPlantWindTurbine(powerPlantRequest.Fuels.Wind)
                        {
                            Efficiency = powerPlant.Efficiency,
                            Name = powerPlant.Name,
                            PowerMax = powerPlant.PowerMax,
                            PowerMin = powerPlant.PowerMin,
                            Type = powerPlant.Type,
                        });
                        break;
                    case PowerPlantType.GasFired:
                        powerPlants.Add(new PowerPlantGasFired(powerPlantRequest.Fuels.Gas)
                        {
                            Efficiency = powerPlant.Efficiency,
                            Name = powerPlant.Name,
                            PowerMax = powerPlant.PowerMax,
                            PowerMin = powerPlant.PowerMin,
                            Type = powerPlant.Type,
                        });
                        break;
                    case PowerPlantType.Turbojet:
                        powerPlants.Add(new PowerPlantTurboJet(powerPlantRequest.Fuels.Kerosine)
                        {
                            Efficiency = powerPlant.Efficiency,
                            Name = powerPlant.Name,
                            PowerMax = powerPlant.PowerMax,
                            PowerMin = powerPlant.PowerMin,
                            Type = powerPlant.Type,
                        });
                        break;
                }
            }
            decimal step = 0.1M;
            var preCalculedPrice = InitArray((int)(powerPlantRequest.Load / step), powerPlantRequest.PowerPlants.Count());
            var preCalculedProduction = InitListOfDictionary((int)(powerPlantRequest.Load / step), powerPlantRequest.PowerPlants.Count());
            var price = DynamicAlgo(powerPlants.ToList(), powerPlantRequest.Load, powerPlantRequest.PowerPlants.Count(), step, preCalculedPrice, preCalculedProduction);
            FillPower(powerPlants.ToList(), preCalculedProduction);
            return powerPlants.Select(e => new ProductionPlan() { Name = e.Name, Production = e.Power });
        }

        private void FillPower(List<PowerPlant> powerPlantList, List<List<Dictionary<int, decimal>>> preCalculedProduction)
        {
            int i = 0;

            Dictionary<int, decimal> dico = preCalculedProduction.Last().Last();
            foreach (var powerPlant in powerPlantList)
            {
                if (dico.TryGetValue(i, out decimal value))
                {
                    powerPlant.Power = value;
                }
                else
                {
                    powerPlant.Power = 0;
                }

                i++;
            }
        }

        private List<List<Dictionary<int, decimal>>> InitListOfDictionary(int iSize, int jSize)
        {
            var listOfDictionary = new List<List<Dictionary<int, decimal>>>();
            for (int i = 0; i < iSize; i++)
            {
                var subt = new List<Dictionary<int, decimal>>();
                listOfDictionary.Add(subt);
                for (int j = 0; j < jSize; j++)
                {
                    subt.Add(new Dictionary<int, decimal>());
                }
            }
            return listOfDictionary;
        }

        private decimal[,] InitArray(int iSize, int jSize)
        {
            var precalcul = new decimal[iSize, jSize];
            for (int i = 0; i < iSize; i++)
            {
                for (int j = 0; j < jSize; j++)
                {
                    precalcul[i, j] = -1;
                }
            }
            return precalcul;
        }

        private decimal DynamicAlgo(List<PowerPlant> powerPlantList, decimal load, int i, decimal step, decimal[,] preCalculedPrice, List<List<Dictionary<int, decimal>>> preCalculedProduction)
        {
            int iIndex = i - 1;
            int jIndex = (int)(load / step) - 1;
            if (load == 0)
            {
                return 0;
            }
            if (i == 0)
            {
                return Decimal.MaxValue;
            }
            if (preCalculedPrice[jIndex, iIndex] != -1)
            {
                return preCalculedPrice[jIndex, iIndex];
            }
            if (i == 1)
            {
                if (powerPlantList[iIndex].MaxProduction < load || powerPlantList[iIndex].PowerMin > load)
                {
                    preCalculedPrice[jIndex, iIndex] = decimal.MaxValue;
                    return decimal.MaxValue;
                }
                else
                {
                    preCalculedProduction[jIndex][iIndex][iIndex] = load;
                    preCalculedPrice[jIndex, iIndex] = powerPlantList[iIndex].PricePerMWH * load;
                    return powerPlantList[iIndex].PricePerMWH * load;
                }

            }

            decimal power = 0;
            decimal powerMin = power;
            decimal powerMinPrice = decimal.MaxValue;
            bool firstValue = true;

            while (power <= load && power <= powerPlantList[iIndex].MaxProduction)
            {
                var tempSubPrice = DynamicAlgo(powerPlantList, load - power, i - 1, step, preCalculedPrice, preCalculedProduction);
                decimal tempPrice;
                if (tempSubPrice == decimal.MaxValue)
                {
                    tempPrice = decimal.MaxValue;
                }
                else
                {
                    tempPrice = powerPlantList[iIndex].PricePerMWH * power + tempSubPrice;
                }
                if (powerMinPrice > tempPrice)
                {
                    powerMin = power;
                    powerMinPrice = tempPrice;
                }
                if (firstValue)
                {
                    power += powerPlantList[iIndex].PowerMin;
                    firstValue = false;
                }
                else
                {
                    power += step;
                }

            }
            int powerMinIndex = (int)(powerMin / step);
            decimal subPrice;

            if (jIndex - powerMinIndex >= 0 && preCalculedPrice[jIndex - powerMinIndex, iIndex - 1] != -1)
            {
                subPrice = preCalculedPrice[jIndex - powerMinIndex, iIndex - 1];
            }
            else
            {
                subPrice = DynamicAlgo(powerPlantList, load - powerMin, i - 1, step, preCalculedPrice, preCalculedProduction);
            }

            decimal Price;
            if (subPrice == decimal.MaxValue)
            {
                Price = decimal.MaxValue;
            }
            else
            {
                Price = powerPlantList[iIndex].PricePerMWH * powerMin + subPrice;
            }
            preCalculedProduction[jIndex][iIndex][iIndex] = powerMin;

            if (jIndex - powerMinIndex >= 0)
            {
                foreach (var d in preCalculedProduction[jIndex - powerMinIndex][iIndex - 1])
                {
                    preCalculedProduction[jIndex][iIndex][d.Key] = d.Value;
                }
            }
            preCalculedPrice[jIndex, iIndex] = Price;
            return Price;
        }
    }
}
