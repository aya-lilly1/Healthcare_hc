
using HealthCare_Common.Extensions;
using HealthCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;


namespace Healthcare_hc.Controllers
{
    public class ApiBaseController : Controller
    {
        private UserModelView _loggedInUser;

        protected UserModelView LoggedInUser
        {
            get
            {
                if (_loggedInUser != null)
                {
                    return _loggedInUser;
                }

                Request.Headers.TryGetValue("Authorization", out StringValues Token);

                if (string.IsNullOrWhiteSpace(Token))
                {
                    _loggedInUser = null;
                    return _loggedInUser;
                }

                var ClaimId = User.Claims.FirstOrDefault(c => c.Type == "Id");
                

                _ = int.TryParse(ClaimId.Value, out int idd);
                
                if (ClaimId == null || !int.TryParse(ClaimId.Value, out int id))
                {
                    throw new ServiceValidationException(401, "Invalid or expired token");
                } 

                var commonManager = HttpContext.RequestServices.GetService(typeof(ICommonManager)) as ICommonManager;

                _loggedInUser = commonManager.GetUserRole(new UserModelView { Id = id});

                return _loggedInUser;
            }
        }

        public ApiBaseController()
        {
        }
    }
}
