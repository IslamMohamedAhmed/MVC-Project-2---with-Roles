using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                var res = await userManager.Users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserName = u.UserName,
                    Roles = userManager.GetRolesAsync(u).GetAwaiter().GetResult()
                }).ToListAsync();
                return View(res);


            }

            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return View(Enumerable.Empty<UserViewModel>());
            }
            var user2 = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await userManager.GetRolesAsync(user)
            };
            return View(user2);


        }


        public async Task<IActionResult> Details(string id,string ViewName = "Details")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var user =await userManager.FindByIdAsync(id);
            if (user is not null)
            {
                var model = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await userManager.GetRolesAsync(user)
                };
                return View(ViewName,model);
            }
            return NotFound();

        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id,"Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id,UserViewModel userViewModel)
        {
            if(id != userViewModel.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            try
            {
                var user = await userManager.FindByEmailAsync(userViewModel.Email);
                if (user is not null) {
                    
                    user.FirstName = userViewModel.FirstName;
                    user.LastName = userViewModel.LastName;
                    await userManager.UpdateAsync(user);
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
            }
            return View(userViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

 

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            try
            {
                var user = await userManager.FindByEmailAsync(userViewModel.Email);
                if (user is not null)
                {

                    await userManager.DeleteAsync(user);
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(userViewModel);
        }
    }
}
