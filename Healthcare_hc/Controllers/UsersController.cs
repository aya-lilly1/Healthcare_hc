using HealthCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Healthcare_hc.Controllers
{
    [ApiController]
    [ApiVersion("1")]
        public class UsersController : ApiBaseController
    {
        private IUserManager _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger,
                              IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        [Route("api/v{version:apiVersion}/user/signUp")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult SignUp(UserRegistrationModel userReg)
        {
            var res = _userManager.SignUp(userReg);
            return Ok(res);
        }


        [Route("api/v{version:apiVersion}/user/login")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult Login( LoginModelView userLogin)
        {
            var res = _userManager.Login(userLogin);
            return Ok(res);
        }

        [Route("api/v{version:apiVersion}/user/UpdateProfile")]
        [HttpPut]
        [MapToApiVersion("1")]
        [Authorize]
        public IActionResult UpdateMyProfile(UserModelView request)
        {
            var user = _userManager.UpdateProfile(LoggedInUser, request);
            return Ok(user);
        }

        
        [HttpDelete]
        [Route("api/v{version:apiVersion}/user/{id}")]
        [MapToApiVersion("1")]
        public IActionResult Delete(int id)
        {
            _userManager.DeleteUser(LoggedInUser, id);
            return Ok();
        }


        [Route("api/v{version:apiVersion}/user/Confirmation")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult Confirmation(string confirmationLink)
        {
            var result = _userManager.Confirmation(confirmationLink);
            return Ok(result);
        }

    }
}
