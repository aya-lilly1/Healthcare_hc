using AutoMapper;
using HealthCare_Common.Extensions;
using HealthCare_Core.Managers.Interfaces;
using HealthCare_EmailService;
using Healthcare_hc.Models;
using HealthCare_infrastructure;
using HealthCare_Models.Static;
using HealthCare_ModelView;
using HealthCare_ModelView.Enums;
using HealthCare_Notifications;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HealtCare_Core.Managers
{
    public class UserManager : IUserManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IConfigurationSettings _configurationSettings;


        public UserManager(healthcare_hcContext dbContext, IMapper mapper, IEmailSender emailSender, IConfigurationSettings configurationSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configurationSettings = configurationSettings;
            _emailSender = emailSender;

        }

        #region Public
            public LoginUserResponse SignUp(UserRegistrationModel userReg)
            {
                if (_dbContext.Users
                           .Any(a => a.Email.Equals(userReg.Email,
                                     StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new ServiceValidationException("User already exist");
                }

                var hashedPassword = HashPassword(userReg.Password);

                var user = _dbContext.Users.Add(new User
                {
                    FirstName = userReg.FirstName,
                    LastName = userReg.LastName,
                    Email = userReg.Email.ToLower(),
                    Password = hashedPassword,
                    ConfirmPassword = hashedPassword,
                    Phone = userReg.Phone,
                    StateId = userReg.StateId,
                    CityId = userReg.CityId,
                    ConfirmationLink = Guid.NewGuid().ToString().Replace("-", "").ToString()


                }).Entity;

                _dbContext.SaveChanges();

            var builder = new EmailBuilder(ActionInvocationTypeEnum.EmailConfirmation,
                                new Dictionary<string, string>
                                {
                                    { "AssigneeName", $"{userReg.FirstName} {userReg.LastName}" },
                                    { "Link", $"{user.ConfirmationLink}" }
                                }, "https://localhost:44309");

            var message = new Message(new string[] { user.Email }, builder.GetTitle(), builder.GetBody());
            _emailSender.SendEmail(message);

            var res = _mapper.Map<LoginUserResponse>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";

                return res;
            }

            public LoginUserResponse Login(LoginModelView userReg)
            {
                var user = _dbContext.Users
                                       .FirstOrDefault(a => a.Email
                                                               .Equals(userReg.Email,
                                                                       StringComparison.InvariantCultureIgnoreCase));

                if (user == null || !VerifyHashPassword(userReg.Password, user.Password))
                {
                    throw new ServiceValidationException(300, "Invalid user name or password received");
                }

                var res = _mapper.Map<LoginUserResponse>(user);
                res.Token = $"Bearer {GenerateJWTToken(user)}";
                return res;
            }

        public UserModelView UpdateProfile(UserModelView currentUser, UserModelView request)
        {
            var user = _dbContext.Users
                    .FirstOrDefault(a => a.Id == currentUser.Id)
                    ?? throw new ServiceValidationException("User not found");
    
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Password = request.Password;
            user.Phone = request.Phone;
            user.StateId = request.StateId;
            user.CityId = request.CityId;

            _dbContext.SaveChanges();
            return _mapper.Map<UserModelView>(user);
        }
        public void DeleteUser(UserModelView currentUser, int id)
        {
            if (currentUser.Id == id)
            {
                throw new ServiceValidationException("You have no access to delete your self");
            }

            var user = _dbContext.Users
                                    .FirstOrDefault(a => a.Id == id)
                                    ?? throw new ServiceValidationException("User not found");
            // for soft delete
            user.Archived = true;
            _dbContext.SaveChanges();

             }

        public UserModelView Confirmation(string ConfirmationLink)
        {
            var user = _dbContext.Users
                           .FirstOrDefault(a => a.ConfirmationLink
                                                    .Equals(ConfirmationLink)
                                                && !a.EmailConfirmed)
                       ?? throw new ServiceValidationException("Invalid or expired confirmation link received");

            user.EmailConfirmed = true;
            user.ConfirmationLink = string.Empty;
            _dbContext.SaveChanges();
            return _mapper.Map<UserModelView>(user);
        }

        #endregion Public

        #region private
        private static string HashPassword(string password)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                return hashedPassword;
            }

            private static bool VerifyHashPassword(string password, string HashedPasword)
            {
                return BCrypt.Net.BCrypt.Verify(password, HashedPasword);
            }

            private string GenerateJWTToken(User user)
            {
             
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationSettings.JwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("FirstName", user.FirstName),
                        //new Claim("RoleName", user.UserRoles),
                        new Claim("DateOfJoining", user.CreatedDate.ToString("yyyy-MM-dd")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                

                var token = new JwtSecurityToken(
                            _configurationSettings.Issuer,
                            _configurationSettings.Issuer,
                            claims,
                            expires: DateTime.Now.AddDays(20),
                            signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

     
        #endregion private




    }
}
