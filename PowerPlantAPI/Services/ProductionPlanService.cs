using PowerPlantAPI.Models;

namespace PowerPlantAPI.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public IEnumerable<ProductionPlan> CreateProductionPlan(PowerPlantRequest powerPlantRequest)
        {
            var powerPlants = InitPowerPlant(powerPlantRequest);
            decimal step = 0.1M;
            var algo = new AlgorithmDynamic(powerPlants.ToList(), powerPlantRequest.Load, step);
            return algo.CreateProductionPlan() ;
        }

        private IEnumerable<PowerPlant> InitPowerPlant(PowerPlantRequest powerPlantRequest)
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
            return powerPlants;
        }
    }
}
