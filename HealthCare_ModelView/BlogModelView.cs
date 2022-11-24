using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_ModelView
{
    public class BlogModelView
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }

        public string Title { get; set; }
        public string Image { get; set; }
        public string ImageString { get; set; }

        public string Content { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
