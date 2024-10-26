using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureTemplate.Application.Services.Interfaces;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Application.UseCases.Implementations.Users;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Application.UseCases.Implementations.Users;

namespace CleanArchitectureTemplate.Application.Extensions
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            return services;
        }
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {

            return services;
        }
        public static IServiceCollection AddFactoryProviders(this IServiceCollection services)
        {
            return services;
        }
        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            return services;
        }
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IAddUserUseCase, AddUserInteractor>();
            services.AddScoped<IGetUserUseCase, GetUserInteractor>();
            services.AddScoped<IGetAllUsersUseCase, GetAllUsersInteractor>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserInteractor>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserInteractor>();

            return services;
        }



    }
}
