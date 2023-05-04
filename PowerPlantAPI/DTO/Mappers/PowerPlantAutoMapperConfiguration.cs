using AutoMapper;
using PowerPlantAPI.Models;

namespace PowerPlantAPI.DTO.Mappers
{
    public class PowerPlantAutoMapperConfiguration : Profile
    {
        public PowerPlantAutoMapperConfiguration()
        {
            CreateMap<PostPowerPlantRequestDTO, PowerPlantRequest>();
            CreateMap<EnergyPriceRequestDTO, EnergyPrice>();
            CreateMap<PowerPlantRequestDTO, PowerPlant>();
            CreateMap<ProductionPlan, PostPowerPlantProductionResponseDTO>();
        }
    }
}
