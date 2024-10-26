using CleanArchitectureTemplate.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Grpc.Services;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Grpc.Interceptors;
//using CleanArchitectureTemplate.Grpc.Services;

namespace CleanArchitectureTemplate.Grpc.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomInterceptors(this IServiceCollection services)
        {
            services.AddScoped<ExceptionInterceptor>();

            return services;
        }
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
