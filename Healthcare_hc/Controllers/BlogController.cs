using HealthCare_Core.Managers;
using HealthCare_Core.Managers.Interfaces;
using Healthcare_hc.Attributes;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Healthcare_hc.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    public class BlogController : ApiBaseController
    {
        private IBlogManager _blogManager;
        private readonly ILogger<BlogController> _logger;
        public BlogController(ILogger<BlogController> logger,
                             IBlogManager blogManager)
        {
            _logger = logger;
            _blogManager = blogManager;
        }

        [Route("api/v{version:apiVersion}/blog/CreateBlog")]
        [HttpPost]
        [MapToApiVersion("1")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HealthCareAuthrize(Permissions = "blog_create")]
        public IActionResult CreateBlog(BlogRequest blogMV)
        {
            var res = _blogManager.CreateBlog(LoggedInUser, blogMV);
            return Ok(res);
        }
        // GET: api/<BlogController>
        [Route("api/v{version:apiVersion}/blog/Getblogs")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetAllBlogs(int page = 1,
                                      int pageSize = 5,
                                      StatusEnum statusEnum = StatusEnum.All,
                                      string sortColumn = "",
                                      string sortDirection = "ascending",
                                      string searchText = "")
        {
            var result = _blogManager.GetBlogs(page, pageSize, statusEnum, sortColumn, sortDirection, searchText);
            return Ok(result);
        }

        [Route("api/v{version:apiVersion}/blog/{id}")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetBlogId(int id, int page = 1, int pageSize = 10)
        {
            var result = _blogManager.GetBlog(id, page, pageSize);
            return Ok(result);
        }


        [Route("api/v{version:apiVersion}/blog/{id}/Delete")]
        [HttpDelete]
        [MapToApiVersion("1")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HealthCareAuthrize(Permissions = "blog_delete")]
        public IActionResult ArchiveBlog(int id)
        {
            _blogManager.ArchiveBlog(LoggedInUser, id);
            return Ok("done");
        }

        [Route("api/v{version:apiVersion}/blog/update")]
        [HttpPut]
        [MapToApiVersion("1")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HealthCareAuthrize(Permissions = "blog_create")]

        public IActionResult PutBlog(BlogRequest blogRequest)
        {
            var result = _blogManager.PutBlog(LoggedInUser, blogRequest);
            return Ok(result);
        }
    }
}
