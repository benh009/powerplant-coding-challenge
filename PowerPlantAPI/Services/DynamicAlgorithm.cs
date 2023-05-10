using PowerPlantAPI.Models;

namespace PowerPlantAPI.Services
{

    public class DynamicAlgorithm : IProductionPlanCalculator
    {
        private PreComputedValue[,] PreComputedValues { get; set; }

        private decimal Step { get; set; }

        private List<PowerPlant> PowerPlantList { get; set; }

        private decimal Load { get; set; }

        private int NumberOfPowerPlant => PowerPlantList.Count();

        // The documentation is here https://www.eeeguide.com/optimal-unit-commitment-uc/
        public DynamicAlgorithm(List<PowerPlant> powerPlantList, decimal load, decimal step)
        {
            PowerPlantList = powerPlantList;
            Step = step;
            Load = load;
            PreComputedValues = PreComputedValue.InitArray((int)(Load / Step), NumberOfPowerPlant);
        }

        public IEnumerable<ProductionPlan> CreateProductionPlan()
        {
            RunAlgorithm(Load, NumberOfPowerPlant);
            FillPowerInPowerPlant();
            return PowerPlantList.Select(e => new ProductionPlan() { Name = e.Name, Production = e.Power });
        }

        private IEnumerable<PowerPlant> FillPowerInPowerPlant()
        {
            int powerPlantN = 1;           
            foreach (var powerPlant in PowerPlantList)
            {
                if (PreComputedPowerPlantPowerHasValue(PreComputedValues.GetLength(0) * Step, PreComputedValues.GetLength(1),powerPlantN))
                {
                    powerPlant.Power = GetPreComputedPowerPlantPower(PreComputedValues.GetLength(0) *Step, PreComputedValues.GetLength(1) , powerPlantN);
                }
                else
                {
                    powerPlant.Power = 0;
                }
                powerPlantN++;
            }
            return PowerPlantList;
        }

        private PowerPlant GetPowerPlant(int n)
        {
            return PowerPlantList[n - 1];
        }

        private decimal? RunAlgorithmBaseCase(decimal load, int n)
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
                    SetPreComputedPrice(load, n, price);
                    return price;
                }
                else
                {
                    SetPreComputedPowerPlant(load, n, n, load);
                    var price = GetPriceForUnitN(n, load);
                    SetPreComputedPrice(load, n, price);
                    return price;
                }
            }
            return null;
        }

        private decimal RunAlgorithm(decimal load, int n)
        {
            var price = RunAlgorithmBaseCase(load, n);
            if (price != null)
            {
                return price.Value;
            }
            if (IsAlreadyPreComputedMinPrice(load, n))
            {
                return GetPreComputedMinPrice(load, n);
            }
            return CalculMinPrice(load, n);
        }
        private decimal CalculMinPrice(decimal load, int n)
        {
            var powerMin = GetMinPower(load, n);
            var price = GetMinimumPriceForTheProductionByTheNUnits(load, n, powerMin);
            SetPreComputedProductionN(load, n, powerMin);
            SetPreComputedPrice(load, n, price);
            return price;
        }

        private void SetPreComputedProductionN(decimal load, int n, decimal powerMin)
        {   
            int powerPlantN = 1;
            foreach (var d in GetPreComputedPowerPlant(load - powerMin, n - 1))
            {
                SetPreComputedPowerPlant(load, n, powerPlantN, d.Power);
                powerPlantN++;
            }
            SetPreComputedPowerPlant(load, n, n, powerMin);
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

                //include 0 power
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
            var SubPrice = RunAlgorithm(load - power, n - 1);
            decimal price;
            if (SubPrice == decimal.MaxValue)
            {
                price = decimal.MaxValue;
            }
            else
            {
                //fn(y) + FN-1 (x – y)
                price = GetPriceForUnitN(n, power) + SubPrice;
            }
            return price;
        }

        //fN (y) = cost of generating y MW by the Nth unit
        private decimal GetPriceForUnitN(int powerPlantN, decimal power)
        {
            return GetPowerPlant(powerPlantN).PricePerMWH * power;
        }

        private void SetPreComputedPowerPlant(decimal load, int n, int powerPlantN, decimal power)
        {
            GetPreComputedValue(load, n).ProductionPlants[powerPlantN-1].Power = power;
        }

        private PreComputedProductionPlants[] GetPreComputedPowerPlant(decimal load, int n)
        {
            return GetPreComputedValue(load, n).ProductionPlants;
        }

        private bool PreComputedPowerPlantPowerHasValue(decimal load, int n, int powerPlantN)
        {
            return GetPreComputedPowerPlantPower(load, n, powerPlantN) != -1;
        }

        private Decimal GetPreComputedPowerPlantPower(decimal load, int n,int powerPlantN)
        {
            return GetPreComputedValue(load, n).ProductionPlants[powerPlantN-1].Power;
        }

        private bool IsAlreadyPreComputedMinPrice(decimal load, int n)
        {
            return GetPreComputedValue(load, n).Price != -1;
        }

        private decimal GetPreComputedMinPrice(decimal load, int n)
        {
            return GetPreComputedValue(load, n).Price;
        }

        private void SetPreComputedPrice(decimal load, int n, decimal price)
        {
            GetPreComputedValue(load, n).Price = price;
        }

        private PreComputedValue GetPreComputedValue(decimal load, int n)
        {
            return PreComputedValues[(int)(load / Step) - 1, n - 1];
        }
    }
}
