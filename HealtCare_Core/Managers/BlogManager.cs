using AutoMapper;
using HealthCare_Common.Extensions;
using HealthCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using HealthCare_ModelView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using HealthCare_Common.Extinsions;
using System.Linq;
using HHealthCare_Common.Extensions;

namespace HealthCare_Core.Managers
{
    public class BlogManager : IBlogManager
    {
        private healthcare_hcContext _dbContext;
        private IMapper _mapper;

        public BlogManager(healthcare_hcContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public void ArchiveBlog(UserModelView currentUser, int id)
        {
            if (!currentUser.IsSuperAdmin && !currentUser.IsDoctor)
            {
                throw new ServiceValidationException("You don't have permission to Delete");
            }

            var data = _dbContext.Blogs
                                    .FirstOrDefault(a => a.Id == id)
                                    ?? throw new ServiceValidationException("Invalid blog id received");
            data.Archived = true;
            _dbContext.SaveChanges();
        }

        public BlogModelView CreateBlog(UserModelView currentUser, BlogRequest blogMV)
        {

            var allowedPermissions = new List<string> { "blog_create" };

            var hasAccess = currentUser.Permissions.Any(a => allowedPermissions.Contains(a.Code));

            var isAllView = currentUser.Permissions.Any(a => a.Code.Equals("blog_create"));

            if (!currentUser.IsDoctor)
            {
                throw new ServiceValidationException("You don't have permission to create");
            }
            var blog = _dbContext.Blogs.Add(new Blog
            {
                Title = blogMV.Title,
                Content = blogMV.Content,
                CreatorId = currentUser.Id,
                Status = (int)blogMV.Status,

            }).Entity;

            _dbContext.SaveChanges();
            var res = _mapper.Map<BlogModelView>(blog);

            return res;
        }
        public PagedResult<BlogModelView> GetBlog(int id, int page = 1, int pageSize = 10)
        {

            var res = _dbContext.Blogs.Where(a => a.Archived == false)
                                   .Include("Creator").Where(a => a.CreatorId == id )
                                   .Select(x => new BlogModelView
                                   {
                                       Id = x.Id,
                                       CreatorId=x.CreatorId,
                                       Title = x.Title,
                                       Content = x.Content,
                                       CreatedDate = x.CreatedDate
                                   }).GetPaged(page, pageSize);


            return res;
        }

        public BlogResponse GetBlogs(int page = 1,
                                     int pageSize = 10,
                                     StatusEnum statusEnum = StatusEnum.All,
                                     string sortColumn = "",
                                     string sortDirection = "ascending",
                                     string searchText = "")
        {
            var queryRes = _dbContext.Blogs
                                        .Where(a => (statusEnum == StatusEnum.All
                                                      || a.Status == (int)statusEnum)
                                                    && (string.IsNullOrWhiteSpace(searchText)
                                                        || (a.Title.Contains(searchText)
                                                            || a.Content.Contains(searchText))));

            if (!string.IsNullOrWhiteSpace(sortColumn)
                && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderByI(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn)
                && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderByDescendingI(sortColumn);
            }

            var res = queryRes.GetPaged(page, pageSize);

            var userIds = res.Data
                             .Select(a => a.CreatorId)
                             .Distinct()
                             .ToList();

            var users = _dbContext.Users
                                     .Where(a => userIds.Contains(a.Id))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<UserResult>(x));

            var data = new BlogResponse
            {
                Blog = _mapper.Map<PagedResult<BlogModelView>>(res),
                User = users
            };

            data.Blog.Sortable.Add("Title", "Title");
            data.Blog.Sortable.Add("CreatedDate", "Created Date");

            data.Blog.Filterable.Add("status", new FilterableKeyModel
            {
                Title = "status",
                Values = ((StatusEnum[])Enum.GetValues(typeof(StatusEnum)))
                            .Select(c => new FilterableValueModel
                            {
                                Id = (int)c,
                                Title = c.GetDescription().ToString()
                            })
                            .ToList()
            });

            return data;
        }

        public BlogModelView PutBlog(UserModelView currentUser, BlogRequest blogRequest)
        {
            Blog blog = null;

            if (!currentUser.IsSuperAdmin && !currentUser.IsDoctor)
            {
                throw new ServiceValidationException("You don't have permission to update");
            }

            blog = _dbContext.Blogs
                                .FirstOrDefault(a => a.Id == blogRequest.Id)
                                ?? throw new ServiceValidationException("Invalid blog id received");

            blog.Title = blogRequest.Title;
            blog.Content = blogRequest.Content;
            blog.Status = (int)blogRequest.Status;





            _dbContext.SaveChanges();
            return _mapper.Map<BlogModelView>(blog);
        }

        public int BolgPagesNum(int pageSize)
        {
            int blogCount = _dbContext.Blogs.Count();
            int pagesCount = blogCount / pageSize;
            return pagesCount;
        }

    }
}
