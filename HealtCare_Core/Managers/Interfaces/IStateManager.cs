using HealthCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealtCare_Core.Managers.Interfaces
{
    public interface IStateManager : IManager
    {
        List<State> GetStateByCity(int CityId);
        StateModelView AddState(StateModelView stateMV);
        StateModelView UpdateState(StateModelView currentState);
        StateModelView DeleteState(StateModelView currentState);
    }
}
