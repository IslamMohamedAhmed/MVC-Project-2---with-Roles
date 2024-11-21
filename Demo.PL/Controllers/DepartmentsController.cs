using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DepartmentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string? Name)
        {
            var departments = await unitOfWork.Departments.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(Name))
            {
                departments = await unitOfWork.Departments.GetDepartmentWithNameAsync(Name);
            }
            var departments2 = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(departments2);
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {

            if (!ModelState.IsValid)
            {
                return View(department);
            }
            else
            {
                var department2 = mapper.Map<DepartmentViewModel, Department>(department);
                await unitOfWork.Departments.CreateAsync(department2);

                if (await unitOfWork.CompleteAsync() > 0) TempData["CreateDepartment"] = "Department was added successfully!";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue) return BadRequest();
            var department = await unitOfWork.Departments.GetAsync(id.Value);
            if (department is null) return NotFound();
            var department2 = mapper.Map<Department, DepartmentViewModel>(department);
            return View(ViewName, department2);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel department)
        {
            if (id != department.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var department2 = mapper.Map<DepartmentViewModel, Department>(department);
                    unitOfWork.Departments.Update(department2);
                    if (await unitOfWork.CompleteAsync() > 0) TempData["EditDepartment"] = "Department was updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel department)
        {
            if (id != department.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var department2 = mapper.Map<DepartmentViewModel, Department>(department);
                    unitOfWork.Departments.Delete(department2);
                    if (await unitOfWork.CompleteAsync() > 0) TempData["DeleteDepartment"] = "Department was removed successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }



    }
}
