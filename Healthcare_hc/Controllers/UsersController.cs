using HealthCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Healthcare_hc.Controllers
{
    public class UsersController : Controller
    {
        private IUserManager _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger,
                              IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        [Route("api/user/signUp")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] UserRegistrationModel userReg)
        {
            var res = _userManager.SignUp(userReg);
            return Ok(res);
        }


        [Route("api//user/login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModelView userLogin)
        {
            var res = _userManager.Login(userLogin);
            return Ok(res);
        }

        //[Route("api/user/UpdateProfile")]
        //[HttpPut]
        //[Authorize]
        //public IActionResult UpdateMyProfile(UserUpdatedModel request)
        //{
        //    var user = _userManager.UpdateProfile(LoggedInUser, request);
        //    return Ok(user);
        //}
    }
}
