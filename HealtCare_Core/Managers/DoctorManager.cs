using AutoMapper;
using HealtCare_Core.Managers.Interfaces;
using HealthCare_Common.Extensions;
using HealthCare_Common.Extinsions;
using HealthCare_EmailService;
using Healthcare_hc.Models;
using HealthCare_Helper;
using HealthCare_Implementation;
using HealthCare_infrastructure;
using HealthCare_Models.Static;
using HealthCare_ModelView;
using HealthCare_ModelView.Enums;
using HealthCare_Notifications;
using HHealthCare_Common.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Core.Managers
{
    public class DoctorManager : IDoctorManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly IEmailSender _emailSender;
        public DoctorManager(healthcare_hcContext dbContext, IMapper mapper, IConfigurationSettings configurationSettings, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configurationSettings = configurationSettings;
            _emailSender = emailSender;


        }
        #region Public
        public LoginUserResponse SignUpBaseData(DoctorRegistrationModel DoctorReg)
        {
            if (_dbContext.Users
                       .Any(a => a.Email.Equals(DoctorReg.Email,
                                 StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ServiceValidationException("User already exist");
            }

            var hashedPassword = HashPassword(DoctorReg.Password);

            var user = _dbContext.Users.Add(new User
            {
                FirstName = DoctorReg.FirstName,
                LastName = DoctorReg.LastName,
                Email = DoctorReg.Email.ToLower(),
                Password = hashedPassword,
                ConfirmPassword = hashedPassword,
                Phone = DoctorReg.Phone,
                StateId = DoctorReg.StateId,
                CityId = DoctorReg.CityId,
                IsDoctor = true,
                IsSuperAdmin = true,

                ConfirmationLink = Guid.NewGuid().ToString().Replace("-", "").ToString()

            }).Entity;

            _dbContext.SaveChanges();


            var res = _mapper.Map<LoginUserResponse>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";

            return res;
        }

        public DoctorDataViewModel SignUpDoctorData(UserModelView user, DoctorDataViewModel DoctorReg)
        {
            var url = "";

            if (!string.IsNullOrWhiteSpace(DoctorReg.ImageString))
            {
                url = Helper.SaveImage(DoctorReg.ImageString, "profileimages");
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44309/";
                DoctorReg.Image = @$"{baseURL}/api/v1/user/fileretrive/profilepic?filename={url}";
                
            }
            var doctor = _dbContext.Doctors.Add(new Doctor
            {
         

            DoctorId = user.Id,
                ClinicName = DoctorReg.ClinicName,
                Address = DoctorReg.Address,
                Image = DoctorReg.Image,
                DepartmentId = DoctorReg.DepartmentId

            }).Entity;

            _dbContext.SaveChanges();
            var res = _mapper.Map<DoctorDataViewModel>(doctor);
            return res;

        }

        public DoctorAppointmentModelView SignUpAppointment(UserModelView currentuser,string day, DateTime start, DateTime end ,int numberOfPatients)
        {
            var Appointment = _dbContext.DoctorAppointments.Add(new DoctorAppointment
            {
                DoctorId = currentuser.Id,
                Day = day,
                StartTime=start,
                EndTime=end,
                NumberOfPatients = numberOfPatients

            }).Entity;
            _dbContext.SaveChanges();
            return _mapper.Map<DoctorAppointmentModelView>(Appointment); 
        }

        //public List<DoctorAppointmentModelView> SignUpAppointment(List<AppointmentDataViewModel> DoctorReg, AppointmentDataViewModel currentuser)
        //{
        //    var doctorList = new List<DoctorAppointment>();
        //    foreach (var doctorApp in DoctorReg)
        //    {
        //        doctorList.Add(new DoctorAppointment
        //        {
        //            DoctorId = currentuser.Id,
        //            Day = doctorApp.Day,
        //            StartTime = doctorApp.StartTime,
        //            EndTime = doctorApp.EndTime,
        //            NumberOfPatients = doctorApp.NumberOfPatients

        //        });
        //    }
        //    _dbContext.AddRange(doctorList);

        //    _dbContext.SaveChanges();
        //    var res = _mapper.Map<List<DoctorAppointmentModelView>>(doctorList);
        //    return res;
        //}

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

        public string UpdateProfile(UserModelView currentUser, DoctorModelView request)
        {
            var user = _dbContext.Users
                .Include(a => a.Doctor)
                .Include(a => a.DoctorAppointment)
                    .FirstOrDefault(a => a.Id == currentUser.Id)
                    ?? throw new ServiceValidationException("User not found");
            var hashedPassword = HashPassword(request.Password);
            var url = "";

            if (!string.IsNullOrWhiteSpace(request.ImageString))
            {
                url = Helper.SaveImage(request.ImageString, "profileimages");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Password = hashedPassword;
            user.Doctor.Address = request.Address;
            user.Phone = request.Phone;
            user.StateId = request.StateId;
            user.CityId = request.CityId;
            user.Doctor.ClinicName = request.ClinicName;
            user.Doctor.DepartmentId = request.DepartmentId;


            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44309/";
                user.Doctor.Image = @$"{baseURL}/api/v1/user/fileretrive/profilepic?filename={url}";
            }

            _dbContext.SaveChanges();
            return "DONE";
        }




        public List<Department> GetDepartments()
        {
            var res = _dbContext.Departments.ToList();

            return res;
        }


        public PagedResult<DoctorModelView> GetSingleDoctorById(int idDoctor, int page = 1, int pageSize = 10)
        {
            var queryRes = _dbContext.Users
                .Include(a => a.Doctor)
                .Where(a => a.IsDoctor == true)
                .Where(a => a.Id == idDoctor)
                .Select(x => new DoctorModelView
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Password = x.Password,
                    DoctorId = x.Doctor.DoctorId,
                    DepartmentId = x.Doctor.DepartmentId,
                    ClinicName = x.Doctor.ClinicName,
                    Image = x.Doctor.Image,
                    Address = x.Doctor.Address,
                    StateId = x.StateId,
                    CityId = x.CityId,
                    ConfirmPassword = x.ConfirmPassword,
                    Email = x.Email,
                    Phone = x.Phone

                }).GetPaged(page, pageSize); ;


            return queryRes;
        }
        public PagedResult<DoctorModelView> GetDoctors( int page = 1, int pageSize = 10)
        {
            var queryRes = _dbContext.Users
                .Include(a => a.Doctor)
                .Where(a => a.IsDoctor == true)
                .Select(x => new DoctorModelView
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Password = x.Password,
                    DoctorId = x.Doctor.DoctorId,
                    DepartmentId = x.Doctor.DepartmentId,
                    ClinicName = x.Doctor.ClinicName,
                    Image = x.Doctor.Image,
                    Address = x.Doctor.Address,
                    StateId = x.StateId,
                    CityId = x.CityId,
                    ConfirmPassword = x.ConfirmPassword,
                    Email = x.Email,
                    Phone = x.Phone

                }).GetPaged(page, pageSize);


            return queryRes;
        }
        public PagedResult<DoctorModelView> GetDoctorByDepartmentId(int departmentId, int page = 1, int pageSize = 10)
        {
            var queryRes = _dbContext.Users
                .Include(a => a.Doctor)
                .Where(a => a.IsDoctor == true)
                .Where(a => (a.Doctor.DepartmentId == departmentId))
                .Select(x => new DoctorModelView
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Password = x.Password,
                    DoctorId = x.Doctor.DoctorId,
                    DepartmentId = x.Doctor.DepartmentId,
                    ClinicName = x.Doctor.ClinicName,
                    Image = x.Doctor.Image,
                    Address = x.Doctor.Address,
                    StateId = x.StateId,
                    CityId = x.CityId,
                    ConfirmPassword = x.ConfirmPassword,
                    Email = x.Email,
                    Phone = x.Phone

                }).GetPaged(page, pageSize);


            return queryRes;
        }
        public PagedResult<GetDoctorAppointmentModelView> GetDoctorAppointment(UserModelView currentUser, int page = 1, int pageSize = 10)
        {
            var queryRes = _dbContext.PatientAppointments
                  .Include(a => a.User)
                .Where(a => a.DoctorId == currentUser.Id)
                .Select(x => new GetDoctorAppointmentModelView
                {
                    
                    PatientName = x.User.FirstName+ " " + x.User.LastName,
                    Day = x.Day,
                    time = x.Time,

                }).GetPaged(page, pageSize);
            return queryRes;
        }

        //public PagedResult<GetDoctorAppointmentModelView>  GetDoctorAppointment(UserModelView currentUser, int departmentId, int page = 1, int pageSize = 10)
        //{
        //    var queryRes = _dbContext.Users
        //                                .Include(a => a.Doctor)
        //                                .Include(a => a.PatientAppointment)
        //                                   .Where(a => (a.Doctor.Id == currentUser.Id))
        //                                      .Select(x => new GetDoctorAppointmentModelView
        //                                           {
        //                                           PatientName = currentUser.FirstName + " " + currentUser.LastName,
        //                                           Day =x.PatientAppointment.Day,
        //                                           time =x.PatientAppointment.Time,

        //                                       }).GetPaged(page, pageSize);

        //    return queryRes;

        //}


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
        }
    }

