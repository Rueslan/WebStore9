using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities;
using WebStore9Domain.Entities.Identity;
using WebStore9Domain.ViewModels;

namespace WebStore9.Controllers;

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
    public IActionResult Index()
    {
        return View(_EmplyeesData.GetAll());
    }

    [Route("~/employees/info-{id}")]
    public IActionResult Details(int id)
    {
        var employee = _EmplyeesData.GetById(id);

        if (employee == null)
            return NotFound();

        return View(new EmployeeViewModel
        {
            Id = employee.Id,
            Name = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
            Seniority = employee.Seniority
        });
    }

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Create()
    {
        return View("Edit", new EmployeeViewModel());
    }

    #region DeleteAsync

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Delete(int id)
    {
        if (id < 0) return BadRequest();

        var employee = _EmplyeesData.GetById(id);
        if (employee is null)
            return NotFound();

        _EmplyeesData.Delete(id);

        return View(new EmployeeViewModel
        {
            Id = employee.Id,
            Name = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
            Seniority = employee.Seniority
        });
    }

    [HttpPost]
    [Authorize(Roles = Role.Administrators)]
    public IActionResult DeleteConfirmed(int id)
    {
        _EmplyeesData.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    #endregion


    #region Edit

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return View(new EmployeeViewModel());

        var employee = _EmplyeesData.GetById((int)id);

        if (employee == null) return NotFound();

        var model = new EmployeeViewModel
        {
            Id = employee.Id,
            Name = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
            Seniority = employee.Seniority
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

        var employee = new Employee
        {
            Id = model.Id,
            LastName = model.LastName,
            FirstName = model.Name,
            Patronymic = model.Patronymic,
            Age = model.Age,
            Seniority = model.Seniority
        };

        if (employee.Id == 0)
            _EmplyeesData.Add(employee);
        else
            _EmplyeesData.Update(employee);

        return RedirectToAction(nameof(Index));
    }

    #endregion
}