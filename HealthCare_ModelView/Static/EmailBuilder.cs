using HealthCare_ModelView.Enums;
using System.Collections.Generic;

namespace HealthCare_Models.Static
{
    public class EmailBuilder
    {
        public EmailBuilder(ActionInvocationTypeEnum notificationType, Dictionary<string, string> parameterValues, string url, string assessmentURL = "")
        {
            NotificationType = notificationType;
            Values = parameterValues;
            URL = url;
        }

        public ActionInvocationTypeEnum NotificationType { get; set; }

        public Dictionary<string, string> Values { get; set; }

        public string URL { get; set; }

        public string Link { get; set; }

        public string GetTitle()
        {
            return NotificationType switch
            {
                ActionInvocationTypeEnum.EmailConfirmation => $"Email Confirmation!",
                ActionInvocationTypeEnum.ResetPassword => $"Reset Password!",
                _ => ""
            };
        }

        public string GetHeading()
        {
            return NotificationType switch
            {
                ActionInvocationTypeEnum.EmailConfirmation => $"</br>You has been registered successfully just one more step to finish. </br>",
                ActionInvocationTypeEnum.ResetPassword => $"</br> You are going to reset you password. </br>",
                _ => ""
            };
        }

        public string GetBody()
        {
            return NotificationType switch
            {
                ActionInvocationTypeEnum.EmailConfirmation => GetEmailConfirmationEmailBody(),
                ActionInvocationTypeEnum.ResetPassword => GetResetPasswordEmailBody(),
                _ => ""
            };
        }

        private string GetEmailConfirmationEmailBody()
        {
            var url = URL + $"/confirm?ConfirmaionKey={(Values.ContainsKey("Link") ? Values["Link"] : "")}";

            var emailBody = "<div width = \"100% !important\" style=\"background:#fff; width:100%!important; margin:0; padding:0; font-family:'Roboto',Helvetica,sans-serif; color:rgb(34, 58, 102); font-size:15px; line-height:1.5em\">" +
                      "<table align = \"center\" bgcolor=\"#e1f1fd\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; max-width:800px; display:table\">" +
                      "<tbody><tr><td align = \"left\" style=\"border-collapse:collapse; text-align:left; font-family:'Muli',Helvetica,sans-serif; font-size:32px; font-weight:900; color:#1B5379; padding-top:20px; letter-spacing:-1px; line-height:1em\">" +
                      "<img data-imagetype=\"External\" src=\"\" width=\"200\" style=\"width:200px\">" +
                      "<br aria-hidden=\"true\"><br aria-hidden=\"true\"><br aria-hidden=\"true\">" +
                      "<span style = \"width:80%; display:block; margin-bottom:40px\">" +
                      "</br>You has been registered successfully just one more step to finish.</br></span> </td></tr></tbody></table>" +
                      "<table align = \"center\" bgcolor= \"#ffffff\" border= \"0\" cellpadding= \"35\" cellspacing= \"0\" width= \"90%\" style= \"margin:0px auto; max-width:800px; display:table\" >" +
                      $"<tbody><tr><td align= \"left\" style= \"border-collapse:collapse; text-align:left\" > Hi {(Values.ContainsKey("AssigneeName") ? Values["AssigneeName"] : "")}," +
                      $"<br>" +
                      $"<br>" +
                      $"Please follow the link to verify you email <a data-click-track-id=\"381\" href=\"{url}\">Press here</a>. <br><br>Thanks,<br> The Health Care Team<br></td></tr></tbody></table>" +
                      "<table align=\"center\" bgcolor=\"#3599e8\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; text-align:center; max-width:800px; color:rgba(255,255,255,.5); font-size:11px; display:table\">" +
                      "<tbody>" +
                      "<tr width=\"100%\"><td width=\"100%\" style=\"border - collapse:collapse\"><img data-imagetype=\"External\" src=\"https://www.canva.com/design/DAFTKerxOG8/ai2lKzJhOcYjIYEvwULHSg/edit?utm_content=DAFTKerxOG8&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton\" alt =\"Health\" width=\"25\" style=\"width:25px; opacity:.5\">" +
                      "<br aria-hidden=\"true\"><span style=\"color:#fff; color:rgba(255,255,255,.5)\">Thank you for your cooperation</span> | <a href=\"\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\" style=\"text-decoration:none; color:#fff; color:rgba(255,255,255,.5)\" data-linkindex=\"1\">UnSubscribe</a> </td></tr></tbody>";

            return emailBody;
        }

        private string GetResetPasswordEmailBody()
        {
            var url = URL + $"/reset?resetkey={(Values.ContainsKey("Link") ? Values["Link"] : "")}";

            var emailBody = "<div width = \"100% !important\" style=\"background:#fff; width:100%!important; margin:0; padding:0; font-family:'Roboto',Helvetica,sans-serif; color:rgb(70,72,74,.9); font-size:15px; line-height:1.5em\">" +
                        "<table align = \"center\" bgcolor=\"#e1f1fd\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; max-width:800px; display:table\">" +
                        "<tbody><tr><td align = \"left\" style=\"border-collapse:collapse; text-align:left; font-family:'Muli',Helvetica,sans-serif; font-size:32px; font-weight:900; color:#1B5379; padding-top:20px; letter-spacing:-1px; line-height:1em\">" +
                        "<img data-imagetype=\"External\" src=\"\" width=\"200\" style=\"width:200px\">" +
                        "<br aria-hidden=\"true\"><br aria-hidden=\"true\"><br aria-hidden=\"true\">" +
                        "<span style = \"width:80%; display:block; margin-bottom:40px\">" +
                        "</br>Rest Your Account Password .</br></span> </td></tr></tbody></table>" +
                        "<table align = \"center\" bgcolor= \"#ffffff\" border= \"0\" cellpadding= \"35\" cellspacing= \"0\" width= \"90%\" style= \"margin:0px auto; max-width:800px; display:table\" >" +
                        $"<tbody><tr><td align= \"left\" style= \"border-collapse:collapse; text-align:left\" > Hi {(Values.ContainsKey("AssigneeName") ? Values["AssigneeName"] : "")}," +
                        $"<br>" +
                        $"<br>" +
                        $"Please follow the link to reset your account password <a data-click-track-id=\"381\" href=\"{url}\">Press here</a>. <br><br>Thanks,<br> The Health Care Team<br></td></tr></tbody></table>" +
                        "<table align=\"center\" bgcolor=\"#3599e8\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; text-align:center; max-width:800px; color:rgba(255,255,255,.5); font-size:11px; display:table\">" +
                        "<tbody>" +
                        "<tr width=\"100%\"><td width=\"100%\" style=\"border - collapse:collapse\"><img data-imagetype=\"External\" src=\"\" alt=\"Health\" width=\"25\" style=\"width:25px; opacity:.5\">" +
                        "<br aria-hidden=\"true\"><span style=\"color:#fff; color:rgba(255,255,255,.5)\">Thank you for your cooperation</span> | " +
                        "<a href=\"\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\" style=\"text-decoration:none; color:#fff; color:rgba(255,255,255,.5)\" data-linkindex=\"1\">Unsubscripted</a> </td></tr></tbody>";

            return emailBody;
        }
    }
}
