using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces;

namespace WebStore9.WebAPI.Controllers
{
    [Route(WebAPIAddresses.Values)]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly Dictionary<int, string> _values = Enumerable.Range(1, 10)
            .Select(i => (Id: i, Value: $"Value-{i}"))
            .ToDictionary(v => v.Id, v => v.Value);

        public ValuesController()
        {
            
        }

        [HttpGet]
        public IActionResult Get() => Ok(_values.Values);

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (!_values.TryGetValue(id, out string? value))
                return NotFound();

            return Ok(value);
        }

        [HttpGet("count")]
        public IActionResult Count() => Ok(_values.Count);

        [HttpPost]
        [HttpPost("add")]
        public IActionResult Add([FromBody]string value)
        {
            var id = _values.Count == 0 ? 1 : _values.Keys.Max() + 1;

            _values.Add(id, value);

            return CreatedAtAction(nameof(GetById), new { id = id });
        }

        [HttpPut("{id}")]
        public IActionResult Replace(int id, [FromBody] string value)
        {
            if (!_values.TryGetValue(id, out _))
                return NotFound();

            _values[id] = value;

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_values.TryGetValue(id, out _))
                return NotFound();

            _values.Remove(id);

            return Ok();
        }

    }
}
