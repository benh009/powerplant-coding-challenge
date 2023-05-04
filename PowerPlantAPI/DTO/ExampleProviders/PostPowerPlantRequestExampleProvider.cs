using PowerPlantAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace PowerPlantAPI.DTO
{
    public class PostPowerPlantRequestExampleProvider : IMultipleExamplesProvider<PostPowerPlantRequestDTO>
    {
        public IEnumerable<SwaggerExample<PostPowerPlantRequestDTO>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Example1",
                new PostPowerPlantRequestDTO()
                {
                    Load = 480,
                    Fuels = new Models.EnergyPriceRequestDTO()
                    {
                        Gas = 13.4M,
                        Kerosine = 50.8M,
                        CO2 = 20,
                        Wind = 60

                    },
                    PowerPlants = new List<PowerPlantRequestDTO>(){
                        new PowerPlantRequestDTO()
                        {
                            Name = "gasfiredbig1",
                            Type = PowerPlantType.GasFired,
                            Efficiency = 0.53M,
                            PowerMin = 100M,
                            PowerMax = 460M
                        },
                        new PowerPlantRequestDTO()
                        {
                            Name = "gasfiredbig2",
                            Type = PowerPlantType.GasFired,
                            Efficiency = 0.53M,
                            PowerMin = 100M,
                            PowerMax = 460M,
                        },
                        new PowerPlantRequestDTO()
                        {
                            Name = "gasfiredsomewhatsmaller",
                            Type = PowerPlantType.GasFired,
                            Efficiency = 0.37M,
                            PowerMin = 40M,
                            PowerMax = 210M
                        },
                        new PowerPlantRequestDTO()
                        {
                            Name = "tj1",
                            Type = PowerPlantType.Turbojet,
                            Efficiency = 0.3M,
                            PowerMin = 0M,
                            PowerMax = 16M

                        },
                        new PowerPlantRequestDTO()
                        {
                            Name = "windpark1",
                            Type = PowerPlantType.WindTurbine,
                            Efficiency = 1,
                            PowerMin = 0M,
                            PowerMax = 150M
                        },
                        new PowerPlantRequestDTO()
                        {
                            Name = "windpark2",
                            Type = PowerPlantType.WindTurbine,
                            Efficiency = 1,
                            PowerMin = 0M,
                            PowerMax = 36M
                        },
                    }
                });
        }
    }
}

