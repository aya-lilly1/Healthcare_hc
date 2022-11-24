using AutoMapper;
using HealthCare_Common.Extensions;
using HealthCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using HealthCare_Helper;
using HealthCare_ModelView;
using Microsoft.IdentityModel.Tokens;
using System;
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
     //   private readonly IEmailSender _emailSender;
     //  private readonly IConfigurationSettings _configurationSettings;


        public UserManager(healthcare_hcContext csvdbContext, IMapper mapper )
        {
            _dbContext = csvdbContext;
            _mapper = mapper;
          
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
                    Image = string.Empty
                }).Entity;

                _dbContext.SaveChanges();

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

                var url = "";

                if (!string.IsNullOrWhiteSpace(request.ImageString))
                {
                    url = Helper.SaveImage(request.ImageString, "profileimages");
                }

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.Password = request.Password;
                user.Address = request.Address;
                user.Phone = request.Phone;
                user.StateId=request.StateId;
                user.CityId=request.CityId;
                user.Birthday=request.Birthday;

                if (!string.IsNullOrWhiteSpace(url))
                {
                    var baseURL = "https://localhost:44309/";
                    user.Image = @$"{baseURL}/api/v1/user/fileretrive/profilepic?filename={url}";
                }

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
                var jwtKey = "#test.key*&^vanthis%$^&*()$%^@#$@!@#%$#^%&*%^*";
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("FirstName", user.FirstName),
                        new Claim("DateOfJoining", user.CreatedDate.ToString("yyyy-MM-dd")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                var issuer = "test.com";

                var token = new JwtSecurityToken(
                            issuer,
                            issuer,
                            claims,
                            expires: DateTime.Now.AddDays(20),
                            signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

        #endregion private
        public UserModelView Confirmation(string ConfirmationLink)
        {
            throw new NotImplementedException();
        }


      

       



    }
}
