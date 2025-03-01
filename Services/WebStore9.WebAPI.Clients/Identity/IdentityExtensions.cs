using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.WebAPI.Clients.Identity
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityWebStoreWebAPIClients(this IServiceCollection services)
        {
            services.AddHttpClient("WebStore9WebAPIIdentity",
                    (s, client) => client.BaseAddress = new(s.GetRequiredService<IConfiguration>()["WebAPI"]))
                .AddTypedClient<IUserStore<User>, UsersClient>()
                .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
                .AddTypedClient<IUserEmailStore<User>, UsersClient>()
                .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
                .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
                .AddTypedClient<IUserClaimStore<User>, UsersClient>()
                .AddTypedClient<IUserLoginStore<User>, UsersClient>()
                .AddTypedClient<IRoleStore<Role>, RolesClient>()
                ;

            return services;
        }

        public static IdentityBuilder AddIdentityWebStoreWebAPIClients(this IdentityBuilder identityBuilder)
        {
            identityBuilder.Services.AddIdentityWebStoreWebAPIClients();
            return identityBuilder;
        }

    }
}
