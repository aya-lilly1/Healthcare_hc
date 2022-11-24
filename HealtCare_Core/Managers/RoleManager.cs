using AutoMapper;
using HealthCare_Common.Extensions;
using HealthCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Core.Managers
{
    public class RoleManager : IRoleManager
    {

        private healthcare_hcContext _dbContext;
        private IMapper _mapper;

        public RoleManager(healthcare_hcContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool CheckAccess(UserModelView userModel, List<string> persmissions)
        {
            var userTest = _dbContext.Users
                                        .FirstOrDefault(a => a.Id == userModel.Id)
                                        ?? throw new ServiceValidationException("Invalid user id");

            if (userTest.IsSuperAdmin)
            {
                return true;
            }

            var userPermissions = _dbContext.UserPermissionView.Where(a => a.UserId == userTest.Id).ToList();

            return userPermissions.Any(r => persmissions.Contains(r.Code));
        }
    }
}
