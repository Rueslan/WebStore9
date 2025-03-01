using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using WebStore9.Interfaces;
using WebStore9.Interfaces.Services.Identity;
using WebStore9.WebAPI.Clients.Base;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.WebAPI.Clients.Identity
{
    public class RolesClient : BaseClient, IRolesClient
    {
        public RolesClient(HttpClient client) : base(client, WebAPIAddresses.Identity.Roles)
        {
        }

        #region IRoleStore<Role>

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken) =>
            await (await PostAsync(Address, role, cancellationToken))
              .EnsureSuccessStatusCode()
              .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken) =>
            await (await PutAsync(Address, role, cancellationToken))
              .EnsureSuccessStatusCode()
              .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken) =>
            await (await PostAsync($"{Address}/Delete", role, cancellationToken))
              .EnsureSuccessStatusCode()
              .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken) =>
            await (await PostAsync($"{Address}/GetRoleId", role, cancellationToken))
              .EnsureSuccessStatusCode()
              .Content
               .ReadFromJsonAsync<string>(cancellationToken)
               .ConfigureAwait(false);

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken) =>
            await (await PostAsync($"{Address}/GetRoleName", role, cancellationToken))
              .EnsureSuccessStatusCode().Content
               .ReadFromJsonAsync<string>(cancellationToken)
               .ConfigureAwait(false);

        public async Task SetRoleNameAsync(Role role, string name, CancellationToken cancellationToken)
        {
            var response = await PostAsync($"{Address}/SetRoleName/{name}", role, cancellationToken);
            role.Name = await response.Content.ReadFromJsonAsync<string>(cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken) =>
            await (await PostAsync($"{Address}/GetNormalizedRoleName", role, cancellationToken))
              .EnsureSuccessStatusCode()
              .Content
               .ReadFromJsonAsync<string>(cancellationToken)
               .ConfigureAwait(false);

        public async Task SetNormalizedRoleNameAsync(Role role, string name, CancellationToken cancellationToken)
        {
            var response = await PostAsync($"{Address}/SetNormalizedRoleName/{name}", role, cancellationToken).ConfigureAwait(false);
            role.NormalizedName = await response.Content.ReadFromJsonAsync<string>(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Role> FindByIdAsync(string id, CancellationToken cancellationToken) =>
            await GetAsync<Role>($"{Address}/FindById/{id}", cancellationToken).ConfigureAwait(false);

        public async Task<Role> FindByNameAsync(string name, CancellationToken cancellationToken) =>
            await GetAsync<Role>($"{Address}/FindByName/{name}", cancellationToken).ConfigureAwait(false);

        #endregion
    }
}
