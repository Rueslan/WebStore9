using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Services.Interfaces;
using WebStore9.ViewModels;
using WebStore9Domain.Entities;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.Controllers
{
    [Route("Employees/[action]/{id?}")]
    [Route("stuff/[action]/{id?}")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmplyeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData EmplyeesData, ILogger<EmployeesController> Logger)
        {
            _EmplyeesData = EmplyeesData;
            _Logger = Logger;
        }

        [Route("~/employees/all")]
        public async Task<IActionResult> Index() => View(await _EmplyeesData.GetAll());

        [Route("~/employees/info-{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _EmplyeesData.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Seniority = employee.Seniority,
            });
        }

        #region DeleteAsync

        [Authorize(Roles = Role.Administrators)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0) return BadRequest();

            var employee = await _EmplyeesData.GetByIdAsync(id);
            if (employee is null) 
                return NotFound();

            await _EmplyeesData.DeleteAsync(id);

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Seniority = employee.Seniority,
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmplyeesData.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Edit

        [Authorize(Roles = Role.Administrators)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return View(new EmployeeViewModel());

            var employee = await _EmplyeesData.GetByIdAsync((int)id);

            if (employee == null) return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Seniority = employee.Seniority,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model.Name == "мат")
                ModelState.AddModelError("", "Низзя!");

            if (!ModelState.IsValid)
                return View(model);

            var employee = new Employee()
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.Patronymic,
                Age = model.Age,
                Seniority = model.Seniority,
            };

            if (employee.Id == 0)
                _EmplyeesData.Add(employee);
            else
                _EmplyeesData.UpdateAsync(employee);

            return RedirectToAction(nameof(Index));            
        }

        #endregion

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Create() => View("Edit", new EmployeeViewModel());
    }
}
