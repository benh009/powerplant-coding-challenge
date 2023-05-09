using PowerPlantAPI.Models;

namespace PowerPlantAPI.Services
{
    public interface IProductionPlanCalculator
    {
        IEnumerable<ProductionPlan> CreateProductionPlan();
    }
}
