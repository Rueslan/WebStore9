using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;
using WebStore9.Interfaces;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.WebAPI.Controllers.Identity
{
    [Route(WebAPIAddresses.Identity.Users)]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly UserStore<User, Role, WebStore9DB> _userStore;

        public UsersApiController(WebStore9DB db)
        {
            _userStore = new (db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAll() => await _userStore.Users.ToArrayAsync();
    }
}
