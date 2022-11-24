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
            if (!currentUser.IsSuperAdmin)
            {
                throw new ServiceValidationException("You don't have permission to archive blog");
            }

            var data = _dbContext.Blogs
                                    .FirstOrDefault(a => a.Id == id)
                                    ?? throw new ServiceValidationException("Invalid blog id received");
            data.Archived = true;
            _dbContext.SaveChanges();
        }

        public BlogModelView GetBlog(UserModelView currentUser, int id)
        {
            var allowedPermissions = new List<string> { "blog_all_view", "blog_view" };

            var hasAccess = currentUser.Permissions.Any(a => allowedPermissions.Contains(a.Code));

            var isAllView = currentUser.Permissions.Any(a => a.Code.Equals("blog_all_view"));

            var res = _dbContext.Blogs
                                   .Include("Creator")
                                   .FirstOrDefault(a => (currentUser.IsSuperAdmin
                                                        || (hasAccess
                                                            && (isAllView || a.CreatorId == currentUser.Id)))
                                                        && a.Id == id)
                                   ?? throw new ServiceValidationException("Invalid blog id received");

            return _mapper.Map<BlogModelView>(res);
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
                queryRes = queryRes.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn)
                && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderByDescending(sortColumn);
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

            if (blogRequest.Id > 0)
            {
                blog = _dbContext.Blogs
                                    .FirstOrDefault(a => a.Id == blogRequest.Id)
                                    ?? throw new ServiceValidationException("Invalid blog id received");

                blog.Title = blogRequest.Title;
                blog.Content = blogRequest.Content;
                blog.Status = (int)blogRequest.Status;
            }
            else
            {
                blog = _dbContext.Blogs.Add(new Blog
                {
                    Title = blogRequest.Title,
                    Content = blogRequest.Content,
                    CreatorId = currentUser.Id,
                    Status = (int)blogRequest.Status
                }).Entity;
            }

            _dbContext.SaveChanges();
            return _mapper.Map<BlogModelView>(blog);
        }

    }
}
