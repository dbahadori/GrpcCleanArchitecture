using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Common.Exceptions;
using CleanArchitectureTemplate.Grpc.Common.Exception;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Threading.Tasks;




namespace CleanArchitectureTemplate.Grpc.Interceptors
{
    public class ExceptionInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                Console.WriteLine("Incoming request: " + request.ToString());

                return await continuation(request, context);
            }
            catch (CustomValidationException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.DeveloperDetail), CreateTrailers((CustomException)ex));
            }
            catch (EntityNotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.DeveloperDetail), CreateTrailers((CustomException)ex));
            }
            catch (ForbiddenException ex)
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, ex.DeveloperDetail), CreateTrailers((CustomException)ex));
            }
            catch (Exception ex)
            {
                // Log unhandled exceptions
                throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."), CreateTrailers((CustomException)ex));
            }
        }

        private Metadata CreateTrailers(CustomException ex)
        {
            // Add custom metadata based on the exception properties if needed
            var metadata = new Metadata();
            if (!string.IsNullOrEmpty(ex.ErrorCode))
            {
                metadata.Add("error-code", ex.ErrorCode);
            }
            if (!string.IsNullOrEmpty(ex.UserFriendlyMessage))
            {
                metadata.Add("error-message", ex.UserFriendlyMessage);
            }
            return metadata;
        }
    }

}
