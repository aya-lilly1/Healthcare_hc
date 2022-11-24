using HealthCare_Core.Managers.Interfaces;
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
        StateModelView GetCities();
    }
}
