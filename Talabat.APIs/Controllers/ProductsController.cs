using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Talabat.Core;
using Talabat.Core.Models.Product;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : ApiBaseController
    {
        ///private readonly IGenericRepository<Product> productRepository;
        ///private readonly IGenericRepository<ProductBrand> brandsRepository;
        ///private readonly IGenericRepository<ProductType> typesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(
            ///IGenericRepository<Product> _productRepository,
            ///IGenericRepository<ProductBrand> _brandsRepository,
            ///IGenericRepository<ProductType> _typesRepository,
            IUnitOfWork unitOfWork,
            IMapper _mapper)
        {
            ///productRepository = _productRepository;
            ///brandsRepository = _brandsRepository;
            ///typesRepository = _typesRepository;
            _unitOfWork = unitOfWork;
            mapper = _mapper;
        }




        #region Get All Products
        [HttpGet] // GET : api/products
        [CachedAttribute(600)] // built-in attribute to get data from redis database if it cached 
        //[Authorize] // Apply Default Challenge Scheme in Identity Services
        [ProducesResponseType(typeof(IReadOnlyList<ProductToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery] ProductSpecParams? productSpecs) // ActionResult : Detect the Type of Result
        {
            //IReadOnlyList<Product>? products = await productRepository.GetAllAsync();

            ProductWithBrandAndTypeSpecifications spec = new ProductWithBrandAndTypeSpecifications(productSpecs);

            // Get Products
            IReadOnlyList<Product> products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            if (products is null)
                return NotFound(new ApiResponse(404));

            // Map the products to ProductToReturnDto
            IReadOnlyList<ProductToReturnDto> mappedProducts = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);


            // Get Count of All Products
            ProductWithFiltrationForCountSpecification countSpec = new ProductWithFiltrationForCountSpecification(productSpecs);
            int countOfAllProducts = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);

            //OkObjectResult result = new OkObjectResult(products);
            //return Ok(result);

            return Ok(new Pagination<ProductToReturnDto>(productSpecs.PageIndex, productSpecs.PageSize, countOfAllProducts, mappedProducts));
        }
        #endregion


        #region Get Product
        [HttpGet("{id:int}")] // GET : BaseUrl/api/products/1
        [CachedAttribute(600)] // built-in attribute to get data from redis database if it cached 
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            //Product product = await productRepository.GetByIdAsync(id);

            ProductWithBrandAndTypeSpecifications spec = new ProductWithBrandAndTypeSpecifications(id);

            Product product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (product is null)
                return NotFound(new ApiResponse(404));

            ProductToReturnDto mappedProduct = mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(mappedProduct);
        }
        #endregion


        #region Get All Brands
        [HttpGet("brands")] // GET : BaseURL/api/products/brands
        [CachedAttribute(600)] // built-in attribute to get data from redis database if it cached 
        [ProducesResponseType(typeof(IReadOnlyList<ProductBrand>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            IReadOnlyList<ProductBrand> brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            if (brands is null)
                return NotFound(new ApiResponse(404));

            return Ok(brands);
        }
        #endregion


        #region Get All Types
        [HttpGet("types")] // GET : BaseUrl/api/products/types
        [CachedAttribute(600)] // built-in attribute to get data from redis database if it cached 
        [ProducesResponseType(typeof(IReadOnlyList<ProductType>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            IReadOnlyList<ProductType> types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            if (types is null)
                return NotFound(new ApiResponse(404));

            return Ok(types);
        }
        #endregion
    }
}
