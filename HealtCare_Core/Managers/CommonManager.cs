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
    public class CommonManager : ICommonManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;
        private IBlogManager _blogManager;

        public CommonManager(healthcare_hcContext dbContext, IMapper mapper, IBlogManager blogManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _blogManager = blogManager;
        }

        public UserModelView GetUserRole(UserModelView user)
        {
            var dbUser = _dbContext.Users
                                      .FirstOrDefault(a => a.Id == user.Id)
                                      ?? throw new ServiceValidationException("Invalid user id received");

            var mappedUser = _mapper.Map<UserModelView>(dbUser);

            mappedUser.Permissions = _dbContext.UserPermissionView.Where(a => a.UserId == user.Id).ToList();
            return mappedUser;
        }
    }

}
