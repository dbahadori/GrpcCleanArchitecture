using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.DTO.V1;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Domain.DTO;
using CleanArchitectureTemplate.Domain.Interfaces.Repositories;
using CleanArchitectureTemplate.Resources;
using FluentValidation;

namespace CleanArchitectureTemplate.Application.UseCases.Implementations.Users
{
    public class GetAllUsersInteractor : IGetAllUsersUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersInteractor(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OperationResult> ExecuteAsync(GetAllRequestDTO request, CancellationToken cancellationToken)
        {
            try
            {
                var usersPagedResult = await _unitOfWork.UserRepository
                .GetAllPagedAsync(
                condition: null,
                orderBy: d => d.NationalCode,
                pageSize: request.PaginationRequestMetadata.PageSize,
                pageNumber: request.PaginationRequestMetadata.PageNumber,
                orderByDescending: request.PaginationRequestMetadata.OrderByDescending
                );
                var userResponses = _mapper.Map<List<UserResponseDTO>>(usersPagedResult.Items);
                var userResponsePagedResult = new PagedResult<UserResponseDTO>
                {
                    Items = userResponses,
                    Paging = usersPagedResult.Paging
                };
                return OperationResult<PagedResult<UserResponseDTO>>.Success(userResponsePagedResult);
            }
            catch (ValidationException ve)
            {
                throw ve;
            }
            catch (Exception exception)
            {
                _unitOfWork.Rollback();
                var (defaultMessage, localizedMessage) = ResourceHelper.GetGeneralErrorMessages(em => ErrorMessages.ErrorDuringEntitiesRetrieval);
                return OperationResult.Failure(
                     new EntityRetrievalException()
                    .WithUserFriendlyMessage(localizedMessage)
                    .WithDeveloperDetail(defaultMessage)
                    .WithInnerCustomException(exception)
                    );
            }
        }
    }
}
