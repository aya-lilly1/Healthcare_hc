using HealtCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Healthcare_hc.Controllers
{
      
    [ApiController]
    [ApiVersion("1")]
    public class CityController : ApiBaseController
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
        [Route("api/v{version:apiVersion}/city/GetCities")]
        [HttpGet]
        [MapToApiVersion("1")]
        [AllowAnonymous]

        public IActionResult GetCities()
        { 
            var res = _cityManager.GetCities();
            return Ok(res);

        }
        // POST api/<CityController>
        [Route("api/v{version:apiVersion}/city/AddCity")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult AddCity([FromBody] CityModelView cityMV)
        {
            var res = _cityManager.AddCity(cityMV);
            return Ok(res);
        }

        //PUT api/<CityController>/5
        [Route("api/v{version:apiVersion}/city/UpdateCity/{id}")]
        [HttpPut]
        [MapToApiVersion("1")]
        public IActionResult UpdateCity([FromBody] CityModelView currentCity)
        {
            var res = _cityManager.UpdateCity(currentCity);
            return Ok(res);
        }

        // DELETE api/<CityController>/5
        [Route("api/v{version:apiVersion}/city/DeleteCity/{id}")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public IActionResult DeleteCity(CityModelView cityMV)
        {
            var res = _cityManager.DeleteCity(cityMV);
            return Ok(res);

        }

    }
}
