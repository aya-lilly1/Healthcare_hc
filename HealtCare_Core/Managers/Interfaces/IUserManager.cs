
using HealthCare_ModelView;

namespace HealthCare_Core.Managers.Interfaces
{
    public interface IUserManager : IManager
    {

        UserModelView UpdateProfile(UserModelView currentUser, UserModelView request);

        LoginUserResponse Login(LoginModelView userReg);

        LoginUserResponse SignUp(UserRegistrationModel userReg);

         void DeleteUser(UserModelView currentUser, int id);

        UserModelView Confirmation(string ConfirmationLink);
    }
}
