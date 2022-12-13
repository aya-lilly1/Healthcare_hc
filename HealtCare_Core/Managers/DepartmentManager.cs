using AutoMapper;
using HealthCare_Common.Extensions;
using HealthCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Core.Managers
{
    public class DepartmentManager : IDepartmentManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;
        public DepartmentManager(healthcare_hcContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<Department> GetDepartments()
        {
            var res = _dbContext.Departments.ToList();
            return res;
        }

        public DepartmentModelView CreateDepartment(DepartmentModelView departmentMV)
        {
            if (_dbContext.Departments
                          .Any(a => a.Name.Equals(departmentMV.Name,
                                    StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ServiceValidationException("Department already exist");
            }

         
            var department = _dbContext.Departments.Add(new Department
            {
                Name = departmentMV.Name
            }).Entity;

            _dbContext.SaveChanges();

            var res = _mapper.Map<DepartmentModelView>(department);

            return res;
        }

        public DepartmentModelView UpdateDepartment(DepartmentModelView currentDepartment)
        {
            var department = _dbContext.Departments
                        .FirstOrDefault(a => a.Id == currentDepartment.Id)
                        ?? throw new ServiceValidationException("Department not found");

            department.Name = currentDepartment.Name;

            _dbContext.SaveChanges();
            return _mapper.Map<DepartmentModelView>(department);
        }
        public DepartmentModelView DeleteDepartment(DepartmentModelView currentDepartment)
        {
            var department = _dbContext.Departments
                        .FirstOrDefault(a => a.Id == currentDepartment.Id)
                        ?? throw new ServiceValidationException("Department not exist");

            department.Archived = true;

            _dbContext.SaveChanges();
            return _mapper.Map<DepartmentModelView>(department);
        }

    }
}
