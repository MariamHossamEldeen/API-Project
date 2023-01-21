using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        // This constructor is used for get all products 
        public ProductWithBrandAndTypeSpecification(ProductSpecParams productParams)
            :base(P => 
                    (string.IsNullOrEmpty(productParams.Search) || P.Name.ToLower().Contains(productParams.Search)) &&
                    (!productParams.BrandId.HasValue || P.ProductBrandId == productParams.BrandId.Value) &&
                    (!productParams.TypeId.HasValue || P.ProductTypeId == productParams.TypeId.Value)
            )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);


            // total products = 18 ~ 20
            // page size = 10 
            // page index = 2

            ApplyPagination(productParams.PageSize * (productParams.PageIndex - 1) , productParams.PageSize );
            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "PriceAsc":
                        AddOrederBy(P => P.Price);
                        break;

                    case "PriceDesc":
                        AddOrederByDescending(P => P.Price);
                        break;

                    case "NameDesc":
                        AddOrederByDescending(P => P.Name);
                        break;

                    default:
                        AddOrederBy(P => P.Name);
                        break;
                }
            }
        }
        // This constructor is used for get a specific product 
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
