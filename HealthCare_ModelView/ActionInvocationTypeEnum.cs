using System.ComponentModel;

namespace HealthCare_ModelView.Enums
{
    public enum ActionInvocationTypeEnum
    {
        [Description("Email Confirmation")]
        EmailConfirmation = 1,
        [Description("Reset Password")]
        ResetPassword = 2
    }
}
