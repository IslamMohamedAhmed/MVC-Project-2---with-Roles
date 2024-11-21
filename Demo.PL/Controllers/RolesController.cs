using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var res = await roleManager.Roles.Select(u => new RoleViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                }).ToListAsync();
                return View(res);


            }

            var role = await roleManager.FindByNameAsync(name);
            if (role is null)
            {
                return View(Enumerable.Empty<RoleViewModel>());
            }
            var role2 = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(role2);


        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(roleViewModel);
            }
            var role = new IdentityRole
            {
                Name = roleViewModel.Name,
            };
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(role);

        }


        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                var model = new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name

                };
                return View(ViewName, model);
            }
            return NotFound();

        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleViewModel)
        {
            if (id != roleViewModel.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(roleViewModel);
            }

            try
            {
                var role = await roleManager.FindByIdAsync(roleViewModel.Id);
                if (role is not null)
                {


                    role.Name = roleViewModel.Name;
                    await roleManager.UpdateAsync(role);
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(roleViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }



        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel roleViewModel)
        {
            if (id != roleViewModel.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(roleViewModel);
            }

            try
            {
                var role = await roleManager.FindByIdAsync(roleViewModel.Id);
                if (role is not null)
                {

                    await roleManager.DeleteAsync(role);
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(roleViewModel);
        }

        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }
            ViewBag.roleId = roleId;
            var users = await userManager.Users.ToListAsync();
            var UsersInRole = new List<UserInRoleViewModel>();
            if (users.Any())
            {
                foreach (var user in users)
                {
                    var RoleUser = new UserInRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        IsInRole = await userManager.IsInRoleAsync(user, role.Name)
                    };
                    UsersInRole.Add(RoleUser);
                }
            }
            return View(UsersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId,List<UserInRoleViewModel> userInRoleViewModels)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if(role is null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                foreach (var user in userInRoleViewModels)
                {
                    var AppUser = await userManager.FindByIdAsync(user.UserId);
                    if (AppUser is null)
                    {
                        return NotFound();
                    }
                    if (user.IsInRole != await userManager.IsInRoleAsync(AppUser, role.Name))
                    {
                        if (user.IsInRole)
                        {
                            await userManager.AddToRoleAsync(AppUser, role.Name);
                        }
                        else
                        {

                            await userManager.RemoveFromRoleAsync(AppUser, role.Name);
                        }
                    }
                }
                return RedirectToAction("Edit",new {id = roleId});
            }
            return View(userInRoleViewModels);

        }
    }
}
