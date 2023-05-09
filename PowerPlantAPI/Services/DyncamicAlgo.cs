using PowerPlantAPI.Models;
using System.Collections.Generic;

namespace PowerPlantAPI.Services
{


    public class DyncamicAlgo : IProductionPlanCalculator
    {
        private PreCalculedValue[,] PreCalculedValue { get; set; }

        private decimal Step { get; set; }

        private List<PowerPlant> PowerPlantList { get; set; }

        private decimal Load { get; set; }

        private int NumberOfPowerPlant => PowerPlantList.Count();

        public DyncamicAlgo(List<PowerPlant> powerPlantList, decimal load, decimal step)
        {
            PowerPlantList = powerPlantList;
            Step = step;
            Load = load;
            PreCalculedValue = Util.InitArray((int)(Load / Step), NumberOfPowerPlant);
        }

        public IEnumerable<ProductionPlan> CreateProductionPlan()
        {
            DynamicAlgo(Load, NumberOfPowerPlant);
            FillPowerInPowerPlant();
            return PowerPlantList.Select(e => new ProductionPlan() { Name = e.Name, Production = e.Power });
        }

        private IEnumerable<PowerPlant> FillPowerInPowerPlant()
        {
            int n = 1;           
            foreach (var powerPlant in PowerPlantList)
            {
                if (PreCalculedPowerPlantPowerHasValue(PreCalculedValue.GetLength(0) * Step, PreCalculedValue.GetLength(1),n))
                {
                    powerPlant.Power = GetPreCalculedPowerPlantPower(PreCalculedValue.GetLength(0) *Step, PreCalculedValue.GetLength(1) , n);
                }
                else
                {
                    powerPlant.Power = 0;
                }
                n++;
            }
            return PowerPlantList;
        }

        private PowerPlant GetPowerPlant(int n)
        {
            return PowerPlantList[n - 1];
        }

        private decimal? DynamicAlgoBaseCase(decimal load, int n)
        {
            if (load == 0)
            {
                return 0;
            }
            else if (n == 0)
            {
                return Decimal.MaxValue;
            }
            else if (n == 1)
            {
                //constraint not respected
                if (GetPowerPlant(n).MaxProduction < load || GetPowerPlant(n).PowerMin > load)
                {
                    var price = decimal.MaxValue;
                    SetPreCalculedPrice(load, n, price);
                    return price;
                }
                else
                {
                    SetPreCalculedPowerPlant(load, n, n, load);
                    var price = GetPriceForUnitN(n, load);
                    SetPreCalculedPrice(load, n, price);
                    return price;
                }
            }
            return null;
        }

        private decimal DynamicAlgo(decimal load, int n)
        {
            var price = DynamicAlgoBaseCase(load, n);
            if (price != null)
            {
                return price.Value;
            }
            if (IsAlreadyPreCalculedMinPrice(load, n))
            {
                return GetPreCalculedMinPrice(load, n);
            }
            return CalculMinPrice(load, n);
        }
        private decimal CalculMinPrice(decimal load, int n)
        {
            var powerMin = GetMinPower(load, n);
            var price = GetMinimumPriceForTheProductionByTheNUnits(load, n, powerMin);
            SetPreCalculedProductionN(load, n, powerMin);
            SetPreCalculedPrice(load, n, price);
            return price;
        }

        private void SetPreCalculedProductionN(decimal load, int n, decimal powerMin)
        {   
            int i = 1;
            foreach (var d in GetPreCalculedPowerPlant(load - powerMin, n - 1))
            {
                SetPreCalculedPowerPlant(load, n, i, d.Power);
                i++;
            }
            SetPreCalculedPowerPlant(load, n, n, powerMin);
        }

        //find min in the list of fN(y) + FN-1(x-y)
        private decimal GetMinPower(decimal load, int n)
        {
            decimal power = 0;
            //init Min
            decimal powerMin = power;
            decimal powerMinPrice = decimal.MaxValue;

            bool firstValue = true;
            while (power <= load && power <= GetPowerPlant(n).MaxProduction)
            {

                decimal tempMinimumPrice = GetMinimumPriceForTheProductionByTheNUnits(load, n, power);
                if (powerMinPrice > tempMinimumPrice)
                {
                    powerMin = power;
                    powerMinPrice = tempMinimumPrice;
                }

                //include 0
                if (firstValue)
                {
                    power += GetPowerPlant(n).PowerMin;
                    firstValue = false;
                }
                else
                {
                    power += Step;
                }
            }
            return powerMin;
        }

        // fn(y) + FN-1(x-y)
        private decimal GetMinimumPriceForTheProductionByTheNUnits(decimal load, int n, decimal power)
        {
            //FN-1 (x – y) = the minimum cost of generating (x – y) MW by the remaining (N – 1) units
            var tempSubPrice = DynamicAlgo(load - power, n - 1);
            decimal tempPrice;
            if (tempSubPrice == decimal.MaxValue)
            {
                tempPrice = decimal.MaxValue;
            }
            else
            {
                //fn(y) + FN-1 (x – y)
                tempPrice = GetPriceForUnitN(n, power) + tempSubPrice;
            }
            return tempPrice;
        }

        //fN (y) = cost of generating y MW by the Nth unit
        private decimal GetPriceForUnitN(int n, decimal power)
        {
            return GetPowerPlant(n).PricePerMWH * power;
        }

        private void SetPreCalculedPowerPlant(decimal load, int n, int powerPlantN, decimal power)
        {
            GetPreCalculedValue(load, n).ProductionPlants[powerPlantN-1].Power = power;
        }

        private PreCalculedProductionPlants[] GetPreCalculedPowerPlant(decimal load, int n)
        {
            return GetPreCalculedValue(load, n).ProductionPlants;
        }

        private bool PreCalculedPowerPlantPowerHasValue(decimal load, int n, int powerPlantN)
        {
            return GetPreCalculedPowerPlantPower(PreCalculedValue.GetLength(0) * Step, PreCalculedValue.GetLength(1), n) != -1;
        }

        private Decimal GetPreCalculedPowerPlantPower(decimal load, int n,int powerPlantN)
        {
            return GetPreCalculedValue(load, n).ProductionPlants[powerPlantN-1].Power;
        }

        private bool IsAlreadyPreCalculedMinPrice(decimal load, int n)
        {
            return GetPreCalculedValue(load, n).Price != -1;
        }

        private decimal GetPreCalculedMinPrice(decimal load, int n)
        {
            return GetPreCalculedValue(load, n).Price;
        }

        private void SetPreCalculedPrice(decimal load, int n, decimal price)
        {
            GetPreCalculedValue(load, n).Price = price;
        }

        private PreCalculedValue GetPreCalculedValue(decimal load, int n)
        {
            return PreCalculedValue[(int)(load / Step) - 1, n - 1];
        }
    }
}
