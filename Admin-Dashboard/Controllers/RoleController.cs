using Admin_Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Talabat.APIs.Errors;

namespace Admin_Dashboard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        #region Index
        // Role/Index
        public async Task<IActionResult> Index()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }
        #endregion


        #region Create
        // Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isRoleExists = await _roleManager.RoleExistsAsync(model.Name);

                IdentityRole mappedRole = new IdentityRole(model.Name.Trim());
                if (!isRoleExists)
                {
                    await _roleManager.CreateAsync(mappedRole);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "This role is already exist!");
                }
            }
            return View(model);

        }
        #endregion


        #region Details
        // Role/Details/{Id:Guid}
        public async Task<IActionResult> Details(string id, string viewName = nameof(Details))
        {
            if (id is null)
                return BadRequest(new ApiResponse(400));

            IdentityRole? role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(new ApiResponse(404));

            var mappedRole = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(viewName, mappedRole);
        }
        #endregion


        #region Update
        // Role/Update/Id
        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, nameof(Update));
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] string id, RoleViewModel model)
        {
            if (model.Id != id)
                return BadRequest(new ApiResponse(400));

            if (ModelState.IsValid)
            {
                var isRoleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!isRoleExists)
                {

                    IdentityRole? role = await _roleManager.FindByIdAsync(model.Id);
                    role.Name = model.Name;
                    await _roleManager.UpdateAsync(role);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "This Role is already exists!");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
        #endregion


        #region Delete
        // Role/delete/Id
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, nameof(Delete));
        }

        [HttpPost, ActionName(nameof(Delete))]
        public async Task<IActionResult> ConfirmDelete([FromRoute] string id)
        {

            try
            {
                IdentityRole? mappedRole = await _roleManager.FindByIdAsync(id);

                await _roleManager.DeleteAsync(mappedRole);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
    }
}
