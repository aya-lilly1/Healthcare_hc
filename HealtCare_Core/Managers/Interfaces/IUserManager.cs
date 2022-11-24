
using HealthCare_ModelView;

namespace HealthCare_Core.Managers.Interfaces
{
    public interface IUserManager : IManager
    {
        //login
        //signup
        //update

        UserModelView UpdateProfile(UserModelView currentUser, UserModelView request);

        LoginUserResponse Login(LoginModelView userReg);

        LoginUserResponse SignUp(UserRegistrationModel userReg);

        // void DeleteUser(UserModel currentUser, int id);

        UserModelView Confirmation(string ConfirmationLink);
    }
}
