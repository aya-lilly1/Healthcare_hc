using AutoMapper;
using HealtCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using HealthCare_ModelView;
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
        public CityModelView GetCities()
        {
            var res = _dbContext.Cities.ToList();
            return _mapper.Map<CityModelView>(res);
        }

      
    }
}

