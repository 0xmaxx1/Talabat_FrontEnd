using Admin_Dashboard.Helpers;
using Admin_Dashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Models.Product;
using Talabat.Core.Specifications.Product_Specifications;

namespace Admin_Dashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Index
        // GET: /Product
        public async Task<IActionResult> Index()
        {

            ProductWithBrandAndTypeSpecifications spec = new ProductWithBrandAndTypeSpecifications();

            IReadOnlyList<Product> products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            IReadOnlyList<ProductViewModel> mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);

            return View(mappedProducts);
        }
        #endregion


        #region Create
        // GET: /Product/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Brands"] = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            ViewData["Types"] = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                    model.PictureURL = await PictureSettings.UploadFile(model.Image, "products");
                else
                    ModelState.AddModelError("Image", "Image is required");

                Product mappedProduct = _mapper.Map<ProductViewModel, Product>(model);

                await _unitOfWork.Repository<Product>().AddAsync(mappedProduct);
                int result = await _unitOfWork.Complete();

                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    ModelState.AddModelError(string.Empty, "Something went wrong, please try again later");
            }

            return View(model);
        }
        #endregion


        #region Details
        public async Task<IActionResult> Details(int id, string viewName = nameof(Details))
        {
            if (id == null)
                return BadRequest(new ApiResponse(400));

            ProductWithBrandAndTypeSpecifications spec = new ProductWithBrandAndTypeSpecifications(id);

            Product product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

            if (product is null)
                return NotFound(new ApiResponse(400));

            ProductViewModel mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(viewName, mappedProduct);
        }
        #endregion


        #region Update
        // GET: /Product/Update
        public async Task<IActionResult> Update(int id)
        {
            ViewData["Brands"] = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            ViewData["Types"] = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            return await Details(id, nameof(Update));
        }

        // POST: /Product/Update
        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product existingProduct = await _unitOfWork.Repository<Product>().GetByIdAsync(model.Id);
                if (existingProduct is null)
                    return NotFound(new ApiResponse(400));

                if (model.Image is not null)
                {
                    // Delete Old Image if it exists
                    if (!string.IsNullOrEmpty(existingProduct.PictureURL))
                    {
                        // Delete Old Image
                        await PictureSettings.DeleteFile(existingProduct.PictureURL);

                    }
                    // Upload New PictureUrl
                    model.PictureURL = await PictureSettings.UploadFile(model.Image, "products");
                }
                else
                {
                    model.PictureURL = existingProduct.PictureURL;
                }

                var mappedProduct = _mapper.Map(model, existingProduct);

                _unitOfWork.Repository<Product>().Update(mappedProduct);
                int result = await _unitOfWork.Complete();
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    ModelState.AddModelError(string.Empty, "Something went wrong, please try again later");
            }

            return View(model);
        }
        #endregion


        #region Delete
        // GET: /Product/Delete
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, nameof(Delete));
        }

        // POST: /Product/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(ProductViewModel model)
        {
            try
            {
                Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(model.Id);
                if (product.PictureURL is not null)
                    await PictureSettings.DeleteFile(product.PictureURL);

                _unitOfWork.Repository<Product>().Delete(product);
                int result = await _unitOfWork.Complete();
                if (result > 0)
                    return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, please try again later");
            }
            return View(model);
        }
        #endregion
    }
}
