using Azure.Core;
using CleanArchitectureTemplate.Application.DTO.V1;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Domain.Common.Validations;
using CleanArchitectureTemplate.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Domain.Interfaces;

namespace CleanArchitectureTemplate.Presentation.Controllers.V1.Common
{
    // Specify the base (or current) API route to:
    // - Keep the existing route serving a default version (backward compatible).
    // - Support query string and HTTP header versioning.
    [Route("api/[Controller]")]

    // Specify the route to support URI Versioning
    //[Route("api/v{version:apiVersion}/[Controller]")]

    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IAddUserUseCase _addUserUseCase;
        private readonly IGetUserUseCase _getUserUseCase;
        private readonly IDeleteUserUseCase _deleteUserUseCase;
        private readonly IUpdateUserUseCase _updateUserUseCase;
        private readonly IGetAllUsersUseCase _getAllUsersUseCase;


        private readonly ILogger<UserController> _logger;
        private readonly IModelValidator _modelValidator;
             
        public UserController(
            IAddUserUseCase addUserUseCase,
            IGetUserUseCase getUserUseCase,
            IDeleteUserUseCase deleteUserUseCase,
            IUpdateUserUseCase updateUserUseCase,
            IGetAllUsersUseCase getAllUsersUseCase,
            ILogger<UserController> logger,
            IModelValidator modelValidator)
        {

            _addUserUseCase = addUserUseCase;
            _deleteUserUseCase = deleteUserUseCase;
            _updateUserUseCase = updateUserUseCase;
            _getUserUseCase = getUserUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;
            _logger = logger;
            _modelValidator = modelValidator;
        }


        [HttpPost()]
        public async Task<IActionResult> addUser([FromBody] AddUserRequestDTO request, CancellationToken cancellationToken)
        {
            // Validation
            await _modelValidator.ValidateAndThrowAsync(request);
            try
            {
                // Register Process
                var registerResult = await _addUserUseCase.ExecuteAsync(request, cancellationToken);
                if (!registerResult.IsSuccessful)
                    throw registerResult.Exception!;

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getUser(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _getUserUseCase.ExecuteAsync(id, cancellationToken);


                if (!result.IsSuccessful)
                {
                    throw result!.Exception!;
                }
                else
                {
                    var successResult = result as OperationResult<UserResponseDTO>;
                    return Ok(successResult.Data);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequestDTO request,
        CancellationToken cancellationToken
        )
        {

            request.Id = id;
            var result = await _updateUserUseCase.ExecuteAsync(request, cancellationToken);

            if (!result.IsSuccessful)
            {
                throw result!.Exception!;
            }
            else
            {
                var successResult = result as OperationResult<UserResponseDTO>;
                return Ok(successResult.Data);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers(
        [FromQuery] PaginationRequestMetadata paginationMetadata,
        CancellationToken cancellationToken
        )
        {
            var request = new GetAllRequestDTO
            {
                PaginationRequestMetadata = paginationMetadata,

            };
            var result = await _getAllUsersUseCase.ExecuteAsync(request, cancellationToken);


            if (!result.IsSuccessful)
            {
                throw result!.Exception!;
            }
            else
            {
                var successResult = result as OperationResult<PagedResult<UserResponseDTO>>;
                return Ok(successResult.Data);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(
            Guid id,
            CancellationToken cancellationToken
            )
        {
            var result = await _deleteUserUseCase.ExecuteAsync(id, cancellationToken);


            if (!result.IsSuccessful)
            {
                throw result!.Exception!;
            }
            else
            {
                return Ok();
            }
        }

    }
}
