using Admin_Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Talabat.APIs.Errors;
using Talabat.Core.Models.Identity;

namespace Admin_Dashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        #region Index
        // GET : User/Index
        public async Task<IActionResult> Index()
        {
            IEnumerable<AppUser> userEntities = await _userManager.Users.ToListAsync();

            List<UserViewModel> users = new List<UserViewModel>();

            foreach (var user in userEntities)
            {
                IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

                users.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles
                });
            }

            return View(users);
        }
        #endregion


        #region Details
        // Get: User/Details/Id
        public async Task<IActionResult> Details(string id, string viewName = nameof(Details))
        {
            if (id is null)
                return BadRequest(new ApiResponse(400));

            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound(new ApiResponse(404));

            var mappedUser = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return View(viewName, mappedUser);
        }
        #endregion


        #region Update
        public async Task<IActionResult> Update(string id)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);

            List<IdentityRole> allRoles = await _roleManager.Roles.ToListAsync();


            UserRolesViewModel userRoles = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(R => new RoleViewModel
                {
                    Id = R.Id,
                    Name = R.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, R.Name).Result
                }).ToList()
            };

            return View(userRoles);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id,UserRolesViewModel model)
        {
            if (id != model.UserId)
                return BadRequest(new ApiResponse(400));

            
                try
                {
                    AppUser? user = await _userManager.FindByIdAsync(id);

                    IEnumerable<string> userRoles = await _userManager.GetRolesAsync(user);

                    foreach (var role in model.Roles)
                    {
                        if (userRoles.Any(R => R == role.Name && !role.IsSelected))
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        else if (!userRoles.Any(R => R == role.Name && role.IsSelected))
                            await _userManager.AddToRoleAsync(user, role.Name);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);

                    return View(model);
                }
            
        }
        #endregion


        #region Delete
        // GET: User/Delete/Id
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, nameof(Delete));
        }

        [HttpPost,ActionName(nameof(Delete))]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            if (id is null)
                return BadRequest(new ApiResponse(400));

            try
            {
                AppUser? user = await _userManager.FindByIdAsync(id);

                await _userManager.DeleteAsync(user);

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
