using HealthCare_Common.Extensions;
using HealthCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace HealtCare_Core.Managers.Interfaces
{
    public interface IDoctorManager : IManager
    {
        LoginUserResponse SignUpBaseData(DoctorRegistrationModel DoctorReg);
        DoctorDataViewModel SignUpDoctorData(UserModelView user,DoctorDataViewModel DoctorReg);
        DoctorAppointmentModelView SignUpAppointment(UserModelView currentuser, string day, DateTime start, DateTime end, int numberOfPatients);
     //    List<DoctorAppointmentModelView> SignUpAppointment(List<AppointmentDataViewModel> DoctorReg, AppointmentDataViewModel currentuser);
        PagedResult<DoctorModelView> GetDoctorByDepartmentId(int departmentId, int page = 1, int pageSize = 10);
        public PagedResult<DoctorModelView> GetDoctors(int page = 1, int pageSize = 10);

        PagedResult<DoctorModelView> GetSingleDoctorById(int idDoctor ,int page = 1, int pageSize = 10);
        List<Department> GetDepartments();
        UserModelView Confirmation(string ConfirmationLink);
        PagedResult<GetDoctorAppointmentModelView> GetDoctorAppointment(UserModelView currentUser, int page = 1, int pageSize = 10);

            String UpdateProfile(UserModelView currentUser, DoctorModelView request);


    }
}
