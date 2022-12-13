using HealtCare_Core.Managers.Interfaces;
using HealthCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Healthcare_hc.Controllers
{
    
    [ApiController]
    [ApiVersion("1")]
    public class DepartmentController : ApiBaseController
    {
        private IDepartmentManager _departmentManager;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(ILogger<DepartmentController> logger,
                              IDepartmentManager departmentManager)
        {
            _logger = logger;
            _departmentManager = departmentManager;
        }
        // GET: api/<DepartmentController>
        [Route("api/v{version:apiVersion}/department/Get")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetDepartment()
        {
            var res = _departmentManager.GetDepartments();
            return Ok(res);
        }


        // POST api/<DepartmentController>
        [Route("api/v{version:apiVersion}/department/CreateDepartment")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult CreateDepartment([FromBody] DepartmentModelView departmentMV)
        {
            var res = _departmentManager.CreateDepartment(departmentMV);
            return Ok(res);
        }

        //PUT api/<DepartmentController>/5
        [Route("api/v{version:apiVersion}/department/update/{id}")]
        [HttpPut]
        [MapToApiVersion("1")]
        public IActionResult UpdateDepartment([FromBody] DepartmentModelView currentDepartment)
        {
            var res = _departmentManager.UpdateDepartment(currentDepartment);
            return Ok(res);
        }

        // DELETE api/<DepartmentController>/5
        [Route("api/v{version:apiVersion}/department/delete/{id}")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public IActionResult DeleteDepartment(DepartmentModelView departmentMV)
        {
            var res = _departmentManager.DeleteDepartment(departmentMV);
            return Ok(res);

        }
    }
}

