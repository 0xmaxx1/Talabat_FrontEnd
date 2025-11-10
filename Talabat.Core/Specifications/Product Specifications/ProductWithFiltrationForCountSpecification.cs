using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Product;

namespace Talabat.Core.Specifications.Product_Specifications
{
    public class ProductWithFiltrationForCountSpecification : BaseSpecification<Product>
    {
        // Apply this Ctor to inherit from Base Ctor to apply Filtration
        public ProductWithFiltrationForCountSpecification(ProductSpecParams productSpecs)
            : base(P =>
            (string.IsNullOrEmpty(productSpecs.Search) || P.Name.ToLower().Contains(productSpecs.Search)) &&
            (!productSpecs.BrandId.HasValue || P.ProductBrandId == productSpecs.BrandId) &&
            (!productSpecs.TypeId.HasValue || P.ProductTypeId == productSpecs.TypeId)
                 )
        {

        }
    }
}
