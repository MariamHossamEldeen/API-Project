using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Data
{
    public  class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> spec)
        {
            var query = inputQuery; // _Context.Set<Product>
            if(spec.Criteria != null) // P => P.Id == 10
                query = query.Where(spec.Criteria);

            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            if(spec.OrderBy != null)    
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);


            // _Context.Set<Product>.Where(P => P.Id == 10)
            query = spec.Includes.Aggregate(query, (currentQuery, includeEpression) => currentQuery.Include(includeEpression));
            // currentQuery = _Context.Set<Product>.Where(P => P.Id == 10)
            // _Context.Set<Product>.Where(P => P.Id == 10).Include( P => P.ProductBrand)
            // currentQuery = _Context.Set<Product>.Where(P => P.Id == 10).Include( P => P.ProductBrand)
            // currentQuery = _Context.Set<Product>.Where(P => P.Id == 10).Include( P => P.ProductBrand).Include( P => P.ProductType)

            return query;
        }
    }
}
