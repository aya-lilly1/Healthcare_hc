

using HealthCare_ModelView;

namespace HealthCare_Core.Managers.Interfaces
{
    public interface IBlogManager : IManager
    {
        BlogResponse GetBlogs(int page = 1,
                            int pageSize = 10,
                            StatusEnum statusEnum = StatusEnum.All,
                            string sortColumn = "",
                            string sortDirection = "ascending",
                            string searchText = "");

        BlogModelView GetBlog(UserModelView currentUser, int id);

        BlogModelView PutBlog(UserModelView currentUser, BlogRequest blogRequest);

        void ArchiveBlog(UserModelView currentUser, int id);
    }
}
