using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Domain.DTO;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Interfaces.Repositories;
using CleanArchitectureTemplate.Resources;

namespace CleanArchitectureTemplate.Application.UseCases.Implementations.Users
{
    public class DeleteUserInteractor : IDeleteUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserInteractor(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();

                return OperationResult.Success();
            }
            catch (Exception exception)
            {
                _unitOfWork.Rollback();
                var (defaultMessage, localizedMessage) = ResourceHelper.GetGeneralErrorMessages(em => ErrorMessages.ErrorDuringRemovingEntity, typeof(UserEntity), id);
                return OperationResult.Failure(
                     new EntityRemovingException()
                    .WithMessage(defaultMessage, localizedMessage)
                    .WithInnerCustomException(exception)
                    );


            }

        }
    }
}

