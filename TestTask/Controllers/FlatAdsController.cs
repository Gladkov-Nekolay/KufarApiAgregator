using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatAdsController : ControllerBase
    {
        private IFlatAdsService _flatAdsService;
        private CancellationToken _cancellationToken => HttpContext.RequestAborted;

        public FlatAdsController(IFlatAdsService flatAdsService)
        {
            _flatAdsService = flatAdsService;
        }

        [HttpGet("correlation/{priceThreshold}")]
        public async Task<IActionResult> GetCorrelationAsync(double priceThreshold)
        {
            return Ok(await _flatAdsService.GetCorrelationsAsync(priceThreshold, _cancellationToken));
        }
        [HttpGet("area/{areaName}")]
        public async Task<IActionResult> GetFlatsRentInAreaAsync(string areaName)
        {
            return Ok(await _flatAdsService.GetFlatRentAdsAsync(areaName, _cancellationToken));
        }
    }
}
