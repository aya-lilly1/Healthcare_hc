using HealtCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;



namespace Healthcare_hc.Controllers
{
    
    [ApiController]
    [ApiVersion("1")]
    public class DoctorController : ApiBaseController
    {
        private IDoctorManager _doctorManager;
        private readonly ILogger<DoctorController> _logger;
        public DoctorController(ILogger<DoctorController> logger,
                             IDoctorManager doctorManager)
        {
            _logger = logger;
            _doctorManager = doctorManager;
        }
        [Route("api/v{version:apiVersion}/doctor/SignUpBaseData")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult SignUpBaseData(DoctorRegistrationModel DoctorReg)
        {
            var result = _doctorManager.SignUpBaseData(DoctorReg);
            return Ok(result);
        }

        [Route("api/v{version:apiVersion}/doctor/SignUpDoctorData")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult SignUpDoctorData([FromBody]DoctorDataViewModel DoctorReg)
        {
            var result = _doctorManager.SignUpDoctorData(LoggedInUser,DoctorReg);
            return Ok(result);
        }

        [Route("api/v{version:apiVersion}/doctor/SignUpAppointment")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult SignUpAppointment( string day, DateTime start, DateTime end, int numberOfPatients)
        {
            var result = _doctorManager.SignUpAppointment(LoggedInUser, day, start, end, numberOfPatients);
            return Ok(result);
        }
        

        
        [Route("api/v{version:apiVersion}/doctor/GetDoctorByDepartmentId")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetDoctorByDepartmentId(int departmentId, int page = 1, int pageSize = 10) 
        {

            var result = _doctorManager.GetDoctorByDepartmentId(departmentId ,page ,  pageSize);
               return Ok(result);

        }

        [Route("api/v{version:apiVersion}/doctor/GetAllDoctors")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetAllDoctors( int page = 1, int pageSize = 10)
        {

            var result = _doctorManager.GetDoctors( page, pageSize);
            return Ok(result);

        }

        [Route("api/v{version:apiVersion}/doctor/GetDoctorAppointment")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetDoctorAppointment(int page = 1, int pageSize = 10)
        {
            var result = _doctorManager.GetDoctorAppointment(LoggedInUser, page, pageSize);
            return Ok(result);

        }

        [Route("api/v{version:apiVersion}/doctor/GetSingleDoctorById")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetSingleDoctorById(int idDoctor, int page = 1, int pageSize = 10)
        {

            var result = _doctorManager.GetSingleDoctorById(idDoctor, page, pageSize);
            return Ok(result);

        }

        [Route("api/v{version:apiVersion}/doctor/GetDepartments")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetDepartment()
        {
            var result = _doctorManager.GetDepartments();
            return Ok(result);

        }

        [Route("api/v{version:apiVersion}/doctor/UpdateProfile")]
        [HttpPut]
        [MapToApiVersion("1")]
        [Authorize]
        public IActionResult UpdateMyProfile(DoctorModelView request)
        {
            var user = _doctorManager.UpdateProfile(LoggedInUser, request);
            return Ok(user);
        }

        [Route("api/v{version:apiVersion}/user/fileretrive/profilepic")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult Retrive(string filename)
        {
            var folderPath = Directory.GetCurrentDirectory();
            folderPath = $@"{folderPath}\{filename}";
            var byteArray = System.IO.File.ReadAllBytes(folderPath);
            return File(byteArray, "image/jpeg", filename);
        }

        [Route("api/v{version:apiVersion}/doctor/Confirmation")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult Confirmation(string confirmationLink)
        {
            var result = _doctorManager.Confirmation(confirmationLink);
            return Ok(result);
        }
    }
}
