using Healthcare_hc.Models;
using HealthCare_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Core.Managers.Interfaces
{
    public interface IDepartmentManager
    {
        List<Department> GetDepartments();
        DepartmentModelView CreateDepartment(DepartmentModelView departmentMV);
        DepartmentModelView UpdateDepartment(DepartmentModelView currentDepartment);
        DepartmentModelView DeleteDepartment(DepartmentModelView currentDepartment);
    }
}
