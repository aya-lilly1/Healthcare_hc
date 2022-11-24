using HealthCare_Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_ModelView
{
    public class BlogResponse
    {
        public PagedResult<BlogModelView> Blog { get; set; }

        public Dictionary<int, UserResult> User { get; set; }
    }
}
