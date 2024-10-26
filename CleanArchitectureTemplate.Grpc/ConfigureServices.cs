using CleanArchitectureTemplate.Grpc.Extentions;
using CleanArchitectureTemplate.Grpc.Interceptors;

namespace CleanArchitectureTemplate.Grpc

{
    public static class ConfigureServices
    {
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            // Add gRPC and configure to use the interceptors
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionInterceptor>();
            });
            services.AddCustomInterceptors();
            services.AddCustomServices();

            return services;
        }
    }
}
