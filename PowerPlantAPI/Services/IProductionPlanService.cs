using PowerPlantAPI.Models;

namespace PowerPlantAPI.Services
{
    public interface IProductionPlanService
    {
        IEnumerable<ProductionPlan> CreateProductionPlan(PowerPlantRequest powerPlant);
    }
}
