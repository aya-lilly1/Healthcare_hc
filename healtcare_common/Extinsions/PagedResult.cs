using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HealthCare_Common.Extensions
{
    public static class PagedResult
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                 int page, int pageSize, int rowCount = 0) where T : class
        {
            if (page == 0)
            {
                page = 1;
            }

            if (pageSize == 0)
            {
                pageSize = 5;
            }

            var result = new PagedResult<T>
            {
                Pagination = new PagedResultBase
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    RowCount = rowCount != 0 ? rowCount : query.Count()
                }
            };

            var pageCount = (double)result.Pagination.RowCount / pageSize;
            result.Pagination.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Data = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }

    public class PagedResultBase
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }
    }

    public class PagedResult<T> where T : class
    {
        public PagedResult()
        {
            Data = new List<T>();
            Sortable = new Dictionary<string, string>();
            Pagination = new PagedResultBase();
            Filterable = new Dictionary<string, FilterableKeyModel>();
        }

        [DataMember(Name = "pagination")]
        public PagedResultBase Pagination { get; set; }

        [DataMember(Name = "sortable")]
        public Dictionary<string, string> Sortable { get; set; }

        [DataMember(Name = "data")]
        public IList<T> Data { get; set; }

        [DataMember(Name = "filterable")]
        public Dictionary<string, FilterableKeyModel> Filterable { get; set; }
    }

    public class FilterableValueModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }

    public class FilterableKeyModel
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "values")]
        public List<FilterableValueModel> Values { get; set; }
    }
}
