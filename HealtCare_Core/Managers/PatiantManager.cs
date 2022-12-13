using AutoMapper;
using HealtCare_Core.Managers.Interfaces;
using HealthCare_Common.Extensions;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Core.Managers
{
    public class PatiantManager : IPatiantManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;
        public PatiantManager(healthcare_hcContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public PatientAppointmentModelView CreateAppointment(UserModelView currentUser, PatientAppointmentModelView appointmentMV, int idDoctor)
        {
            var doctor = _dbContext.Users.Include(a => a.Doctor)
                                               .Include(a => a.DoctorAppointment)
                                               .Where(a => a.Id == idDoctor).FirstOrDefault();
            if(doctor.DoctorAppointment.Day.ToLower() == appointmentMV.Day.ToLower())
            {
                if (doctor.DoctorAppointment.StartTime <= appointmentMV.Time && doctor.DoctorAppointment.EndTime >= appointmentMV.Time)
                    throw new ServiceValidationException("Doctor is unAvailable, try another time ");

            }
            var appointment = _dbContext.PatientAppointments.Add(new PatientAppointment
            {
                PatientId = currentUser.Id,
                DoctorId = idDoctor,
                Day = appointmentMV.Day,
                Time = appointmentMV.Time
            }).Entity;
            _dbContext.SaveChanges();

            return _mapper.Map<PatientAppointmentModelView>(appointment);

        }

    }
}
