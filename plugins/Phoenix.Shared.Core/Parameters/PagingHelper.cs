using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Phoenix.Shared.Core.Parameters
{
    public class PagingHelper
    {
        public IQueryable<T> sortir<T>(IQueryable<T> query,string defsort,string sortBy, string sortDir)
        {
            bool abortsort = false;
            if (defsort != null)
            {
                if (typeof(T).GetProperty(defsort) == null)
                {
                    abortsort = true;
                }
            }
            if (sortBy != null)
            {
                if (typeof(T).GetProperty(sortBy) != null)
                {
                    abortsort = false;
                }
            }


            if (!abortsort)
            {
                if (sortDir != null)
                {
                    var sortmode = "asc";
                    if (sortDir.ToLower() == "desc")
                    {
                        sortmode = "desc";
                    }
                    query = query.OrderBy(sortBy + " " + sortmode);
                }
                
            }
            

            return query;
        }
        public IQueryable<T> limit<T>(IQueryable<T> query, int take, int skip)
        {
            query = query.Skip(skip);
            query = query.Take(take);
            return query;
        }
        public IQueryable<T> filterContains<T>(IQueryable<T> query, string field, string search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field+".Contains(@0)", search);
                }
                   
            }
            return query;
        }

       

        public IQueryable<T> filterEquals<T>(IQueryable<T> query, string field, string search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "=@0", search);
                }

            }
            return query;
        }
        public IQueryable<T> filterEquals<T>(IQueryable<T> query, string field, double? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "=@0", search);
                }

            }
            return query;
        }
        public IQueryable<T> filterEquals<T>(IQueryable<T> query, string field, decimal? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "=@0", search);
                }

            }
            return query;
        }
        public IQueryable<T> filterEquals<T>(IQueryable<T> query, string field, bool? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "=@0", search);
                }

            }
            return query;
        }
        public IQueryable<T> filterEquals<T>(IQueryable<T> query, string field, DateTime? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "=@0", search.Value);
                }

            }
            return query;
        }
        public IQueryable<T> filterEquals<T>(IQueryable<T> query, string field, long? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "=@0", search.Value);
                }

            }
            return query;
        }
        public IQueryable<T> filterEquals<T>(IQueryable<T> query, string field, int? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "=@0", search.Value);
                }

            }
            return query;
        }
        public IQueryable<T> filterLess<T>(IQueryable<T> query, string field, string search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "<@0", search);
                }

            }
            return query;
        }
        public IQueryable<T> filterLessEquals<T>(IQueryable<T> query, string field, string search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "<=@0", search);
                }

            }
            return query;
        }
        public IQueryable<T> filterLess<T>(IQueryable<T> query, string field, DateTime? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "<@0", search.Value);
                }

            }
            return query;
        }
        public IQueryable<T> filterLessEquals<T>(IQueryable<T> query, string field, DateTime? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "<=@0", search.Value);
                }

            }
            return query;
        }
        public IQueryable<T> filterGreater<T>(IQueryable<T> query, string field, string search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + ">@0", search);
                }

            }
            return query;
        }
        public IQueryable<T> filterGreaterEquals<T>(IQueryable<T> query, string field, string search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + ">=@0", search);
                }

            }
            return query;
        }
        public IQueryable<T> filterGreater<T>(IQueryable<T> query, string field, DateTime? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + ">@0", search.Value);
                }

            }
            return query;
        }
        public IQueryable<T> filterGreaterEquals<T>(IQueryable<T> query, string field, DateTime? search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + ">=@0", search.Value);
                }

            }
            return query;
        }
        public IQueryable<T> filterNotEquals<T>(IQueryable<T> query, string field, string search)
        {
            if (search != null)
            {
                if (typeof(T).GetProperty(field) != null)
                {
                    query = query.Where(field + "<> @0", search);
                }

            }
            return query;
        }
        public GeneralResponseList<T> response<T>(GeneralResponseList<T> resp, IQueryable<T> query)
        {
            resp.RecordsTotal = query.Count() ;


            return resp;
        }
    }

}
