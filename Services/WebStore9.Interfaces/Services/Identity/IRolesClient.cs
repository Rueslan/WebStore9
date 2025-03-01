using Microsoft.AspNetCore.Identity;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.Interfaces.Services.Identity;

public interface IRolesClient: IRoleStore<Role>
{

}