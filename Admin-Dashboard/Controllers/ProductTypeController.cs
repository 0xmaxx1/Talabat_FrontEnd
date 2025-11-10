using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Models.Product;

namespace Admin_Dashboard.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region Index
        // GET: ProductType/Index
        public async Task<IActionResult> Index()
        {
            IReadOnlyList<ProductType> types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            if (types is null)
                return NotFound(new ApiResponse(400));

            return View(types);
        }
        #endregion


        #region Create
        // GET: ProductType/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductType model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Repository<ProductType>().AddAsync(model);

                int result = await _unitOfWork.Complete();
                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }
            else
                ModelState.AddModelError("Name", "Type Name is Required!");

            return View(model);
        }
        #endregion


        #region Details
        // GET: ProductType/Details/{Id}
        public async Task<IActionResult> Details(int id, string viewName = nameof(Details))
        {
            if (id == null)
                return BadRequest(new ApiResponse(400));

            ProductType type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);

            if (type is null)
                return NotFound(new ApiResponse(404));

            return View(viewName, type);
        }
        #endregion


        #region Update
        // GET: ProductType/Update/{Id}
        public async Task<IActionResult> Update(int id)
        {
            return await Details(id, nameof(Update));
        }

        // POST: ProductType/Update
        [HttpPost]
        public async Task<IActionResult> Update(ProductType model)
        {
            if (ModelState.IsValid)
            {
                ProductType existingType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(model.Id);

                if (existingType is null)
                    return NotFound(new ApiResponse(404));

                _unitOfWork.Repository<ProductType>().Update(existingType);

                int result = await _unitOfWork.Complete();

                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }
            else
                ModelState.AddModelError("Name", "Type Name is Required!");

            return View(model);
        }
        #endregion


        #region Delete
        // GET: ProductType/Delete/{Id}
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, nameof(Delete));
        }

        // POST: ProductType/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(ProductType model)
        {
            try
            {
                ProductType existingType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(model.Id);

                if (existingType is null)
                    return NotFound(new ApiResponse(404));

                _unitOfWork.Repository<ProductType>().Delete(existingType);

                int result = await _unitOfWork.Complete();

                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("Name", "Type Name is Required!");
            }
            return View(model);
        }
        #endregion
    }
}
