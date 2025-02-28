using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities;

namespace WebStore9.WebAPI.Controllers
{
    [Route(WebAPIAddresses.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeesData _employeesData;

        public EmployeesApiController(IEmployeesData employeesData) => _employeesData = employeesData;

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _employeesData.GetAll();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employee = _employeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut]
        public IActionResult Update(Employee employee)
        {
            _employeesData.Update(employee);

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            var id = _employeesData.Add(employee);

            return CreatedAtAction(nameof(GetById), new { id }, employee);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _employeesData.Delete(id);

            return result ? Ok(true) : NotFound(false);
        }

    }
}
