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
    public interface ICityManager : IManager
    {
        List<City> GetCities();
        CityModelView AddCity(CityModelView cityMV);
        CityModelView UpdateCity(CityModelView currentCity);
        CityModelView DeleteCity(CityModelView currentCity);
    }
}
