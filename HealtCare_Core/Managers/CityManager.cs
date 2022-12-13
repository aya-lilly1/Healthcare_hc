using AutoMapper;
using HealtCare_Core.Managers.Interfaces;
using HealthCare_Common.Extensions;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealtCare_Core.Managers
{
    public class CityManager : ICityManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;
        public CityManager(healthcare_hcContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<City> GetCities()
        {
            var res = _dbContext.Cities.Where(a => a.Archived == false).ToList();
            return res;
        }

        public CityModelView AddCity(CityModelView cityMV)
        {
            if (_dbContext.Cities
                          .Any(a => a.Name.Equals(cityMV.Name,
                                    StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ServiceValidationException("City already exist");
            }


            var city = _dbContext.Cities.Add(new City
            {
                Name = cityMV.Name
            }).Entity;

            _dbContext.SaveChanges();

            var res = _mapper.Map<CityModelView>(city);

            return res;
        }

        public CityModelView UpdateCity(CityModelView currentCity)
        {
            var city = _dbContext.Cities
                        .FirstOrDefault(a => a.Id == currentCity.Id)
                        ?? throw new ServiceValidationException("City not found");

            city.Name = currentCity.Name;

            _dbContext.SaveChanges();
            return _mapper.Map<CityModelView>(city);
        }
        public CityModelView DeleteCity(CityModelView currentCity)
        {
            var city = _dbContext.Cities
                        .FirstOrDefault(a => a.Id == currentCity.Id)
                        ?? throw new ServiceValidationException("City not exist");

            city.Archived=true;

            _dbContext.SaveChanges();
            return _mapper.Map<CityModelView>(city);
        }
    }


}


