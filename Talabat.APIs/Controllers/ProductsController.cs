using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;

        public ProductsController(IGenericRepository<Product> productsRepo, IMapper mapper, IGenericRepository<ProductBrand> brandsRepo, IGenericRepository<ProductType> typeRepo)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
            _brandsRepo = brandsRepo;
            _typeRepo = typeRepo;
        }

            [CachedAttribute(600)]
            // Get: api/Products
            //[Authorize]
            [HttpGet]
            public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts
                ([FromQuery] ProductSpecParams productParams)
            {
                var spec = new ProductWithBrandAndTypeSpecification(productParams);

                var products = await _productsRepo.GetAllWithSpecAsync(spec);

                var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

                var countSpec = new ProductWithFiltersForCountSpecification(productParams);

                var count = await _productsRepo.GetCountAsync(countSpec);

                return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, count, Data));
            }

            // Get: api/Products/10
            [CachedAttribute(600)]

            [HttpGet("{id}")]
            public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
            {
                var spec = new ProductWithBrandAndTypeSpecification(id);
                var product = await _productsRepo.GetByIdWithSpecAsync(spec);
                if (product == null)
                    return NotFound(new ApiResponse(404));
                return Ok(_mapper.Map<Product, ProductToReturnDto>(product));

            }

            // Get: api/Products/brands
            [CachedAttribute(600)]

            [HttpGet("brands")]
            public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
            {
                var brands = await _brandsRepo.GetAllAsync();
                return Ok(brands);
            }

            // Get: api/Products/types
            [CachedAttribute(600)]

            [HttpGet("types")]
            public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
            {
                var types = await _typeRepo.GetAllAsync();
                return Ok(types);
            }

        }
    }

