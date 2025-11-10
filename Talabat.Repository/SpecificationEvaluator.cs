using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            IQueryable<TEntity> query = inputQuery; // query -----> dbContext.Products

            if (spec.Criteria is not null) // Criteria = P => P.Id == 1
                query = query.Where(spec.Criteria); // Where Condition

            // query -----> dbContext.Products.Where(P => P.Id == 1)


            if (spec.OrderBy is not null) // P => P.Price
                query = query.OrderBy(spec.OrderBy);
            // query -----> dbContext.Products.Where(P => P.Id == 1).OrderBy(P => P.Price)

            if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);
            // query -----> dbContext.Products.Where(P => P.Id == 1).OrderByDesc(P => P.Price)


            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            // Includes
            // 1. P => P.ProductBrand
            // 2. P => P.ProductType

            // currentQuery = query -----> dbContext.Products.Where(P => P.Id == 1).OrderBy(P => P.Price).Include(P => P.ProductBrand).ToListAsync();
            // currentQuery = query -----> dbContext.Products.Where(P => P.Id == 1).OrderBy(P => P.Price).Include(P => P.ProductBrand).Include(P => P.ProductType)

            return query;
        }
    }
}
