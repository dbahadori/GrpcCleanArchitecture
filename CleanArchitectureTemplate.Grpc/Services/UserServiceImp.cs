using CleanArchitectureTemplate.Application.DTO.V1;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Domain.DTO;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CleanArchitectureTemplate.Grpc.Services
{
    public class UserServiceImp : UserService.UserServiceBase
    {
        private readonly IAddUserUseCase _addUserUseCase;
        private readonly IGetUserUseCase _getUserUseCase;
        private readonly IUpdateUserUseCase _updateUserUseCase;
        private readonly IDeleteUserUseCase _deleteUserUseCase;
        private readonly IGetAllUsersUseCase _getAllUsersUseCase;

        public UserServiceImp(
            IAddUserUseCase addUserUseCase,
            IGetUserUseCase getUserUseCase,
            IUpdateUserUseCase updateUserUseCase,
            IDeleteUserUseCase deleteUserUseCase,
            IGetAllUsersUseCase getAllUsersUseCase)
        {
            _addUserUseCase = addUserUseCase;
            _getUserUseCase = getUserUseCase;
            _updateUserUseCase = updateUserUseCase;
            _deleteUserUseCase = deleteUserUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;
        }

        public override async Task<UserResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var result = await _getUserUseCase.ExecuteAsync(Guid.Parse(request.Id), context.CancellationToken);
            if (!result.IsSuccessful)
                throw result.Exception!;

            var userDto = (result as OperationResult<UserResponseDTO>)!.Data;
            return new UserResponse
            {
                Id = userDto.Id.ToString(),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                NationalCode = userDto.NationalCode,
                BirthDay = userDto.BirthDay.ToString("yyyy-MM-dd")
            };
        }


        public override async Task<Empty> AddUser(AddUserRequest request, ServerCallContext context)
        {
            var userDto = new AddUserRequestDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                NationalCode = request.NationalCode,
                BirthDay = DateTime.Parse(request.BirthDay)
            };

            var result = await _addUserUseCase.ExecuteAsync(userDto, context.CancellationToken);
            if (!result.IsSuccessful)
                throw result.Exception!;

            return new Empty();
        }

        public override async Task<Empty> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var userDto = new UpdateUserRequestDTO
            {
                Id = Guid.Parse(request.Id),
                FirstName = request.FirstName,
                LastName = request.LastName,
                NationalCode = request.NationalCode,
                BirthDay = DateTime.Parse(request.BirthDay)
            };

            var result = await _updateUserUseCase.ExecuteAsync(userDto, context.CancellationToken);
            if (!result.IsSuccessful)
                throw result.Exception!;

            return new Empty();
        }

        public override async Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var result = await _deleteUserUseCase.ExecuteAsync(Guid.Parse(request.Id), context.CancellationToken);
            if (!result.IsSuccessful)
                throw result.Exception!;

            return new Empty();
        }

        public override async Task<UserListResponse> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
        {
            var paginationMetadata = new PaginationRequestMetadata
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            var getAllRequest = new GetAllRequestDTO
            {
                PaginationRequestMetadata = paginationMetadata
            };

            var result = await _getAllUsersUseCase.ExecuteAsync(getAllRequest, context.CancellationToken);

            if (!result.IsSuccessful)
                throw result.Exception!;

            var pagedResult = (result as OperationResult<PagedResult<UserResponseDTO>>)?.Data;

            var users = pagedResult.Items.Select(userDto => new UserResponse
            {
                Id = userDto.Id.ToString(),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                NationalCode = userDto.NationalCode,
                BirthDay = userDto.BirthDay.ToString("yyyy-MM-dd")
            }).ToList();

            return new UserListResponse { Users = { users } };
        }




    }
}
