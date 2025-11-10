using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity // as query should be run on entity as it detects signature [Id]
    {
        // Where Specification
        public Expression<Func<T, bool>> Criteria { get; set; } // Criteria is a lambda expression that checks on a property and returns a boolean value

        // Include Specification
        public List<Expression<Func<T, object>>> Includes { get; set; } // Include takes a property and returns a list of objects


        // Order Specifications
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }


        // Pagination Specifications
        public int Take { get; set; } // Take is used to limit the number of records returned by the query
        public int Skip { get; set; } // Skip is used to skip a number of records before returning the result set
        public bool IsPaginationEnabled { get; set; } // IsPaginationEnabled is used to check if pagination is enabled or not
    }
}
