using AutoMapper;
using HealtCare_Core.Managers.Interfaces;
using HealthCare_Common.Extensions;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealtCare_Core.Managers
{
    public class StateManager : IStateManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;
        public StateManager(healthcare_hcContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<State> GetStateByCity(int CityId)
        {
            var res = _dbContext.States.Where(c => c.CityId == CityId).ToList();
            return res;
        }
        public StateModelView AddState(StateModelView stateMV)
        {
            if (_dbContext.States
                          .Any(a => a.Name.Equals(stateMV.Name,
                                    StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ServiceValidationException("State already exist");
            }


            var state = _dbContext.States.Add(new State
            {
                Name = stateMV.Name
            }).Entity;

            _dbContext.SaveChanges();

            var res = _mapper.Map<StateModelView>(state);

            return res;
        }

        public StateModelView UpdateState(StateModelView currentState)
        {
            var state = _dbContext.States
                        .FirstOrDefault(a => a.Id == currentState.Id)
                        ?? throw new ServiceValidationException("state not found");

            state.Name = currentState.Name;

            _dbContext.SaveChanges();
            return _mapper.Map<StateModelView>(state);
        }
        public StateModelView DeleteState(StateModelView currentState)
        {
            var state = _dbContext.States
                        .FirstOrDefault(a => a.Id == currentState.Id)
                        ?? throw new ServiceValidationException("State not exist");

            state.Archived = true;

            _dbContext.SaveChanges();
            return _mapper.Map<StateModelView>(state);
        }
    }

}

