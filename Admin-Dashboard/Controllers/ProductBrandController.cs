using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Models.Product;

namespace Admin_Dashboard.Controllers
{
    public class ProductBrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductBrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            IReadOnlyList<ProductBrand> brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            if (brands is null || brands.Count == 0)
                return NotFound(new ApiResponse(400));

            return View(brands);
        }
        #endregion


        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductBrand model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Repository<ProductBrand>().AddAsync(model);
                int result = await _unitOfWork.Complete();
                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }
            else
                ModelState.AddModelError("Name", "Brand name is required");

            return View(model);
        }
        #endregion


        #region Details
        public async Task<IActionResult> Details(int id, string viewName = nameof(Details))
        {
            if (id == null)
                return BadRequest(new ApiResponse(400));

            ProductBrand brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);

            if (brand is null)
                return NotFound(new ApiResponse(404));

            return View(viewName, brand);
        }
        #endregion


        #region Update
        public async Task<IActionResult> Update(int id)
        {
            return await Details(id, nameof(Update));
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductBrand model)
        {
            if (ModelState.IsValid)
            {
                ProductBrand existingBrand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(model.Id);
                if (existingBrand is null)
                    return NotFound(new ApiResponse(404));

                _unitOfWork.Repository<ProductBrand>().Update(existingBrand);
                int result = await _unitOfWork.Complete();
                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }
            else
                ModelState.AddModelError("Name", "Brand Name is required");

            return View(model);
        }
        #endregion


        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, nameof(Delete));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductBrand model)
        {
            try
            {
                ProductBrand existingBrand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(model.Id);
                if (existingBrand is null)
                    return NotFound(new ApiResponse(404));

                _unitOfWork.Repository<ProductBrand>().Delete(existingBrand);
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
