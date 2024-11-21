using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Models;
using Demo.PL.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeesController(IUnitOfWork unitOfWork, IMapper mapper)
        {

            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? Name)
        {

            var employees = await unitOfWork.Employees.GetAllWithDepartmentAsync();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                employees = await unitOfWork.Employees.GetAllByNameAsync(Name);
            }

            var employeesVM = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(employeesVM);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await unitOfWork.Departments.GetAllAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            ViewBag.Departments = await unitOfWork.Departments.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            else
            {
                if (employee.Image is not null)
                {
                    employee.ImageName = DocumentSettings.UploadFile(employee.Image, "Images");
                }
                var employee2 = mapper.Map<EmployeeViewModel, Employee>(employee);
                await unitOfWork.Employees.CreateAsync(employee2);
                if (await unitOfWork.CompleteAsync() > 0) TempData["CreateEmployee"] = "Employee was added successfully!";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            ViewBag.Departments =await unitOfWork.Departments.GetAllAsync();
            if (!id.HasValue) return BadRequest();
            var employee =await unitOfWork.Employees.GetAsync(id.Value);
            if (employee is null) return NotFound();
            var employee2 = mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, employee2);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employee)
        {
            if (id != employee.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (employee.Image is not null)
                    {
                        employee.ImageName = DocumentSettings.UploadFile(employee.Image, "Images");
                    }
                    var employee2 = mapper.Map<EmployeeViewModel, Employee>(employee);
                    unitOfWork.Employees.Update(employee2);
                  
                    if (await unitOfWork.CompleteAsync() > 0)
                    {
                        TempData["EditEmployee"] = "Employee was updated successfully!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employee);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employee)
        {
            if (id != employee.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (employee.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile("Images",employee.ImageName);
                    }
                    var employee2 = mapper.Map<EmployeeViewModel, Employee>(employee);
                    unitOfWork.Employees.Delete(employee2);
                    if (await unitOfWork.CompleteAsync() > 0) TempData["DeleteEmployee"] = "Employee was removed successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employee);
        }



    }
}

