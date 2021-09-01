using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using tourseek_backend.domain.DTO.UserDTOs;
using tourseek_backend.domain.Validators;
using tourseek_backend.services.RolesService;
using tourseek_backend.services.UsersService;

namespace tourseek_backend.services
{
    public static class InjectServices
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddTransient<IValidator<CreateUserDto>, UserValidator>();
            return services;
        }
    }
}
