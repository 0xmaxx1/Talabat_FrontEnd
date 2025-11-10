using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; } // Where Condition


        public List<Expression<Func<T, object>>> Includes { get; set; }
            = new List<Expression<Func<T, object>>>(); // Include [Eager Loading] : To include related data


        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        // Pagination
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }



        // Ctor to set the includes [GetAll] for Includes only
        public BaseSpecification()
        {
            //Includes = new List<Expression<Func<T, object>>>(); // we replace it by initializing it in Prop
        }

        // Ctor to set the criteria [GetById] for Where only
        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression; // P => P.Id == 1 // P => (true && P.ProductTypeId == 3)
            //Includes = new List<Expression<Func<T, object>>>();// To Get the related Data
        }


        // Method to initialize OrderBy Property
        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        // Method to initialize OrderByDesc Property
        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }


        // Method to Apply Pagination
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;

            Skip = skip;
            Take = take;
        }
    }
}
