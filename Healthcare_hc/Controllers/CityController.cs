using HealtCare_Core.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Healthcare_hc.Controllers
{
      //[Route("api/[controller]")]
    [ApiController]
    public class CityController : Controller
    {

        private ICityManager _cityManager;
        private readonly ILogger<CityController> _logger;

        public CityController(ILogger<CityController> logger,
                              ICityManager cityManager)
        {
            _logger = logger;
            _cityManager = cityManager;
        }
        // GET: CityController
        [Route("api/city/GetCities")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCities()
        { 
            var res = _cityManager.GetCities();
            return Ok(res);

        }

 
    }
}
