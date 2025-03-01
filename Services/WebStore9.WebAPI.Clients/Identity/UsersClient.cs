using Microsoft.AspNetCore.Identity;
using System.Net.Http.Json;
using System.Security.Claims;
using WebStore9.Interfaces;
using WebStore9.Interfaces.Services.Identity;
using WebStore9.WebAPI.Clients.Base;
using WebStore9Domain.DTO.Identity;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.WebAPI.Clients.Identity
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(HttpClient client) : base(client, WebAPIAddresses.Identity.Users)
        {
        }

        #region Implementation of IUserStore<User>

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/UserId", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadAsStringAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/UserName", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadAsStringAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(User user, string name, CancellationToken cancellationToken)
        {
            user.UserName = name;
            await PostAsync($"{Address}/UserName/{name}", user, cancellationToken);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/NormalUserName/", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadAsStringAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = name;
            await PostAsync($"{Address}/NormalUserName/{name}", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/User", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
                .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PutAsync($"{Address}/User", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
                .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/User/Delete", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
                .ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await GetAsync<User>($"{Address}/User/Find/{id}", cancellationToken);
        }

        public async Task<User> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await GetAsync<User>($"{Address}/User/Normal/{name}", cancellationToken);
        }

        #endregion

        #region Implementation of IUserRoleStore<User>

        public async Task AddToRoleAsync(User user, string role, CancellationToken cancellationToken)
        {
            await PostAsync($"{Address}/Role/{role}", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(User user, string role, CancellationToken cancellationToken)
        {
            await PostAsync($"{Address}/Role/Delete/{role}", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/roles", user, cancellationToken))
                .EnsureSuccessStatusCode()

                .Content
                .ReadFromJsonAsync<IList<string>>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<bool> IsInRoleAsync(User user, string role, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/InRole/{role}", user, cancellationToken))
                .EnsureSuccessStatusCode()

                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string role, CancellationToken cancellationToken)
        {
            return await GetAsync<List<User>>($"{Address}/UsersInRole/{role}", cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserPasswordStore<User>

        public async Task SetPasswordHashAsync(User user, string hash, CancellationToken cancellationToken)
        {
            user.PasswordHash = hash;
            await PostAsync(
                $"{Address}/SetPasswordHash", new PasswordHashDTO { Hash = hash, User = user },
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetPasswordHash", user, cancellationToken))
                .EnsureSuccessStatusCode()

                .Content
               .ReadAsStringAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/HasPassword", user, cancellationToken))
                .EnsureSuccessStatusCode()

                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserEmailStore<User>

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            await PostAsync($"{Address}/SetEmail/{email}", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetEmail", user, cancellationToken))
                .EnsureSuccessStatusCode()

                .Content
               .ReadAsStringAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetEmailConfirmed", user, cancellationToken))
                .EnsureSuccessStatusCode()

                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            await PostAsync($"{Address}/SetEmailConfirmed/{confirmed}", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await GetAsync<User>($"{Address}/User/FindByEmail/{email}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/User/GetNormalizedEmail", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadAsStringAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            if (email == string.Empty) return;

            user.NormalizedEmail = email;
            await PostAsync($"{Address}/SetNormalizedEmail/{email}", user, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserPhoneNumberStore<User>

        public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phone;
            await PostAsync($"{Address}/SetPhoneNumber/{phone}", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetPhoneNumber", user, cancellationToken))
                .EnsureSuccessStatusCode()

                .Content
               .ReadAsStringAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetPhoneNumberConfirmed", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            await PostAsync($"{Address}/SetPhoneNumberConfirmed/{confirmed}", user, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserLoginStore<User>

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            await PostAsync($"{Address}/AddLogin", new AddLoginDTO { User = user, UserLoginInfo = login }, cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveLoginAsync(User user, string LoginProvider, string ProviderKey, CancellationToken cancellationToken)
        {
            await PostAsync($"{Address}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetLogins", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<List<UserLoginInfo>>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancellationToken)
        {
            return await GetAsync<User>($"{Address}/User/FindByLogin/{LoginProvider}/{ProviderKey}", cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserLockoutStore<User>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetLockoutEndDate", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<DateTimeOffset?>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? EndDate, CancellationToken cancellationToken)
        {
            user.LockoutEnd = EndDate;
            await PostAsync(
                $"{Address}/SetLockoutEndDate",
                new SetLockoutDTO { User = user, LockoutEnd = EndDate }, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/IncrementAccessFailedCount", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<int>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            await PostAsync($"{Address}/ResetAccessFailedCont", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetAccessFailedCount", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<int>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetLockoutEnabled", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            await PostAsync($"{Address}/SetLockoutEnabled/{enabled}", user, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserTwoFactorStore<User>

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            await PostAsync($"{Address}/SetTwoFactor/{enabled}", user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetTwoFactorEnabled", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<bool>(cancellationToken)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserClaimStore<User>

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetClaims", user, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<List<Claim>>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            await PostAsync($"{Address}/AddClaims", new AddClaimDTO { User = user, Claims = claims }, cancellationToken).ConfigureAwait(false);
        }

        public async Task ReplaceClaimAsync(User user, Claim OldClaim, Claim NewClaim, CancellationToken cancellationToken)
        {
            await PostAsync(
                $"{Address}/ReplaceClaim",
                new ReplaceClaimDTO { User = user, Claim = OldClaim, NewClaim = NewClaim }, cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            await PostAsync(
                $"{Address}/RemoveClaims", new RemoveClaimDTO { User = user, Claims = claims },
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return await (await PostAsync($"{Address}/GetUsersForClaim", claim, cancellationToken))
                .EnsureSuccessStatusCode()
                .Content
               .ReadFromJsonAsync<List<User>>(cancellationToken)
               .ConfigureAwait(false);
        }

        #endregion
    }
}
