using Swashbuckle.AspNetCore.Filters;

namespace PowerPlantAPI.Models
{
    public class PostPowerPlantProductionResponseExampleProvider : IExamplesProvider<IEnumerable<PostPowerPlantProductionResponseDTO>>
    {
        public IEnumerable<PostPowerPlantProductionResponseDTO> GetExamples()
        {
            return new List<PostPowerPlantProductionResponseDTO>() {
                new PostPowerPlantProductionResponseDTO() { Name = "windpark1", Production =75},
                new PostPowerPlantProductionResponseDTO() { Name = "windpark2", Production =18},
                new PostPowerPlantProductionResponseDTO() { Name = "gasfiredbig1", Production =0},
                new PostPowerPlantProductionResponseDTO() { Name = "gasfiredbig2", Production =0},
                new PostPowerPlantProductionResponseDTO() { Name = "tj1", Production =0},
                new PostPowerPlantProductionResponseDTO() { Name = "tj2", Production =0},
            };
        }
    }
}
