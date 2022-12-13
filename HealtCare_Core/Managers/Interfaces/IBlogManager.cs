

using HealthCare_Common.Extensions;
using HealthCare_ModelView;

namespace HealthCare_Core.Managers.Interfaces
{
    public interface IBlogManager : IManager
    {
        BlogModelView CreateBlog(UserModelView currentUser, BlogRequest blogMV);


        BlogResponse GetBlogs(int page = 1,
                            int pageSize = 10,
                            StatusEnum statusEnum = StatusEnum.All,
                            string sortColumn = "",
                            string sortDirection = "ascending",
                            string searchText = "");

        PagedResult<BlogModelView> GetBlog(int id, int page = 1, int pageSize = 10);

        BlogModelView PutBlog(UserModelView currentUser, BlogRequest blogRequest);

        void ArchiveBlog(UserModelView currentUser, int id);
    }
}
