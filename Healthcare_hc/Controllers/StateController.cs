using HealtCare_Core.Managers.Interfaces;
using HealthCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Healthcare_hc.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    public class StateController : ApiBaseController
    {
        private IStateManager _stateManager;
        private readonly ILogger<StateController> _logger;

        public StateController(ILogger<StateController> logger,
                              IStateManager stateManager)
        {
            _logger = logger;
            _stateManager = stateManager;
        }
        // GET: api/<StateController>
        [Route("api/v{version:apiVersion}/state/GetState")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetStateByCity(int CityId)
        {
            var res = _stateManager.GetStateByCity(CityId);
            return Ok(res);
        }


        // POST api/<StateController>
        [Route("api/v{version:apiVersion}/state/CreateState")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult AddState([FromBody] StateModelView StateMV)
        {
            var res = _stateManager.AddState(StateMV);
            return Ok(res);
        }

        //PUT api/<StateController>/5
        [Route("api/v{version:apiVersion}/state/updatestate/{id}")]
        [HttpPut]
        [MapToApiVersion("1")]
        public IActionResult UpdateState([FromBody] StateModelView currentState)
        {
            var res = _stateManager.UpdateState(currentState);
            return Ok(res);
        }

        // DELETE api/<StateController>/5
        [Route("api/v{version:apiVersion}/state/deletestate/{id}")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public IActionResult DeleteState(StateModelView stateMV)
        {
            var res = _stateManager.DeleteState(stateMV);
            return Ok(res);
        }
    }
}
