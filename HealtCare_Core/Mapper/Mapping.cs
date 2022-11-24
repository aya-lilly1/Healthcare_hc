using AutoMapper;
using Healthcare_hc.Models;
using HealthCare_ModelView;


namespace HealtCare_Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<City, CityModelView>().ReverseMap();
        }


    }
}
