using AutoMapper;
using HealthCare_Common.Extensions;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using System.Linq;

namespace HealtCare_Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<City, CityModelView>().ReverseMap();

            CreateMap<User, LoginUserResponse>().ReverseMap();

            CreateMap<UserResult, User>().ReverseMap();

            CreateMap<UserModelView, User>().ReverseMap();

            CreateMap<BlogModelView, Blog>().ReverseMap();
            CreateMap<DoctorResponse, Doctor>().ReverseMap();
            
             CreateMap<BlogModelView, Blog>().ReverseMap();
            CreateMap<DoctorResponse, DoctorModelView>().ReverseMap();

            CreateMap<PagedResult<BlogModelView>, PagedResult<Blog>>().ReverseMap();

            CreateMap<User, UserModelView>().ReverseMap();

            CreateMap<DoctorModelView, Doctor>().ReverseMap();

            CreateMap<DoctorRegistrationModel, Doctor>().ReverseMap();
            CreateMap<PatientAppointmentModelView, PatientAppointment>().ReverseMap();
            
            CreateMap<GetDoctorAppointmentModelView, DoctorAppointment>().ReverseMap();
            CreateMap<StateModelView, State>().ReverseMap();

            CreateMap<DepartmentModelView, Department>().ReverseMap();
            CreateMap<DoctorAppointmentModelView, DoctorAppointment>().ReverseMap();

        }


    }
}
