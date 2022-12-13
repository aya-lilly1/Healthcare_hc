﻿using HealthCare_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Core.Managers.Interfaces
{
    public interface IRoleManager : IManager
    {
        bool CheckAccess(UserModelView userModelView, List<string> persmissions);
    }
}
