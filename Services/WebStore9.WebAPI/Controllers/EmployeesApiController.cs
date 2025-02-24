using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities;

namespace WebStore9.WebAPI.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeesData _employeesData;

        public EmployeesApiController(IEmployeesData employeesData) => _employeesData = employeesData;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employees = await _employeesData.GetAll();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeesData.GetByIdAsync(id);
            if (employee is null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Employee employee)
        {
            await _employeesData.UpdateAsync(employee);

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Employee employee)
        {
            var id = await _employeesData.Add(employee);

            return CreatedAtAction(nameof(GetById), new { id }, employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeesData.DeleteAsync(id);

            return result ? Ok(true) : NotFound(false);
        }

    }
}
