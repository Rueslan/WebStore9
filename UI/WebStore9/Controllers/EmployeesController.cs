using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities;
using WebStore9Domain.Entities.Identity;
using WebStore9Domain.ViewModels;

namespace WebStore9.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
//[Route("employees/[action]/{id?}")]
[Authorize]
public class EmployeesController(IEmployeesData employeesData, ILogger<EmployeesController> logger) : Controller
{

    public IActionResult Index()
    {
        logger.LogInformation("Отображение списка сотрудников");
        return View(employeesData.GetAll());
    }

    public IActionResult Details(int id)
    {
        logger.LogInformation("Запрошены детали сотрудника с id = {id}", id);
        var employee = employeesData.GetById(id);

        if (employee is null)
        {
            logger.LogWarning("Сотрудник с id = {id} не найден", id);
            return NotFound();
        }

        return View(new EmployeeViewModel
        {
            Id = employee.Id,
            Name = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic ?? string.Empty,
            Age = employee.Age,
            Seniority = employee.Seniority
        });
    }

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Create()
    {
        logger.LogInformation("Открытие формы создания нового сотрудника");
        return View("Edit", new EmployeeViewModel());
    }

    #region DeleteAsync

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Delete(int id)
    {
        logger.LogInformation("Попытка удалить сотрудника с id = {id}", id);

        if (id < 0)
        {
            logger.LogWarning("Передан некорректный id для удаления: {id}", id);
            return BadRequest();
        }

        var employee = employeesData.GetById(id);

        if (employee is null)
        {
            logger.LogWarning("Сотрудник с id = {id} не найден для удаления", id);
            return NotFound();
        }

        employeesData.Delete(id);

        return View(new EmployeeViewModel
        {
            Id = employee.Id,
            Name = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic ?? string.Empty,
            Age = employee.Age,
            Seniority = employee.Seniority
        });
    }

    [HttpPost]
    [Authorize(Roles = Role.Administrators)]
    public IActionResult DeleteConfirmed(int id)
    {
        employeesData.Delete(id);

        logger.LogInformation("Сотрудник с id = {id} удалён", id);

        return RedirectToAction(nameof(Index));
    }

    #endregion


    #region Edit

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return View(new EmployeeViewModel());

        var employee = employeesData.GetById((int)id);

        if (employee == null) return NotFound();

        var model = new EmployeeViewModel
        {
            Id = employee.Id,
            Name = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic ?? string.Empty,
            Age = employee.Age,
            Seniority = employee.Seniority
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = Role.Administrators)]
    public IActionResult Edit(EmployeeViewModel model)
    {
        if (model.Name.Contains("бл"))
        {
            logger.LogWarning("Попытка создать/изменить сотрудника с недопустимым именем");
            ModelState.AddModelError("", "недопустимое именя!");
        }

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Модель сотрудника невалидна при сохранении");
            return View(model);
        }


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
        {
            logger.LogInformation("Добавлен новый сотрудник: {Name} {LastName}", employee.FirstName, employee.LastName);
            employeesData.Add(employee);
        }
        else
        {
            logger.LogInformation("Обновлён сотрудник с id = {Id}", employee.Id);
            employeesData.Update(employee);
        }

        return RedirectToAction(nameof(Index));
    }

    #endregion
}