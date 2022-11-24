using HealthCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Healthcare_hc.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]

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
        public IActionResult SignUp([FromBody] UserRegistrationModel userReg)
        {
            var res = _userManager.SignUp(userReg);
            return Ok(res);
        }


        [Route("api/user/login")]
        [HttpPost]
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
