using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PowerPlantAPI.DTO;
using PowerPlantAPI.Models;
using PowerPlantAPI.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Net.Mime;

namespace PowerPlantAPI.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductionPlanController : ControllerBase
    {
        private readonly ILogger<ProductionPlanController> _logger;
        private readonly IProductionPlanService _productionPlanService;
        private readonly IMapper _mapper;

        public ProductionPlanController(ILogger<ProductionPlanController> logger, IProductionPlanService productionPlanService, IMapper mapper)
        {
            _logger = logger;
            _productionPlanService = productionPlanService;
            _mapper = mapper;
        }

        [HttpPost(Name = "PostPowerPlantProduction")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<PostPowerPlantProductionResponseDTO>))]
        [SwaggerRequestExample(requestType: typeof(PostPowerPlantRequestDTO), examplesProviderType: typeof(PostPowerPlantRequestExampleProvider))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, examplesProviderType: typeof(PostPowerPlantProductionResponseExampleProvider))]
        public async Task<IActionResult> Post([FromBody] PostPowerPlantRequestDTO postPowerPlantRequestDTO)
        {
            var powerPlantRequest = _mapper.Map<Models.PowerPlantRequest>(postPowerPlantRequestDTO);
            var result = _productionPlanService.CreateProductionPlan(powerPlantRequest);
            return this.Ok(_mapper.Map<IEnumerable<PostPowerPlantProductionResponseDTO>>(result));
        }
    }
}