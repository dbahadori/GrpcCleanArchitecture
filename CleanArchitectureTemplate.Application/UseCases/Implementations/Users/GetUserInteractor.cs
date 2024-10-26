using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Domain.Common.Exceptions;
using CleanArchitectureTemplate.Domain.DTO;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Interfaces.Repositories;
using CleanArchitectureTemplate.Resources;

namespace CleanArchitectureTemplate.Application.UseCases.Implementations.Users
{
    public class GetUserInteractor : IGetUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetUserInteractor(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OperationResult> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

                if (user != null)
                {
                    var ticketResponse = _mapper.Map<UserResponseDTO>(user);
                    return OperationResult<UserResponseDTO>.Success(ticketResponse);
                }
                else
                {
                    var (defaultMessage, localizedMessage) = ResourceHelper.GetGeneralErrorMessages(em => ErrorMessages.EntityNotFound, typeof(UserEntity).Name, "Id", id);

                    return OperationResult.Failure(
                        new EntityNotFoundException()
                        .WithDeveloperDetail(defaultMessage)
                        .WithUserFriendlyMessage(localizedMessage)
                        );
                }

            }
            catch (Exception exception)
            {
                var (defaultMessage, localizedMessage) = ResourceHelper.GetGeneralErrorMessages(em => ErrorMessages.ErrorDuringUserRetrievalById, typeof(UserEntity).Name, "Id", id);
                return OperationResult.Failure(
                     new EntityCreationException()
                    .WithUserFriendlyMessage(localizedMessage)
                    .WithDeveloperDetail(defaultMessage)
                    .WithInnerCustomException(exception)
                    );
            }

        }

    }
}
