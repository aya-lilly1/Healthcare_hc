using AutoMapper;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealtCare_Core.Managers
{
    public class StateManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;
        public StateManager(healthcare_hcContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public StateModelView GetCities()
        {
            var res = _dbContext.Cities.ToList();
            return _mapper.Map<StateModelView>(res);
        }
    }
}
