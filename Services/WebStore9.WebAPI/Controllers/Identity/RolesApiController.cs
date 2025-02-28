using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;
using WebStore9.Interfaces;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.WebAPI.Controllers.Identity
{
    [Route(WebAPIAddresses.Identity.Roles)]
    [ApiController]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleStore<Role> _roleStore;
        public RolesApiController(WebStore9DB db)
        {
            _roleStore = new(db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAll() => await _roleStore.Roles.ToArrayAsync();

    }
}
