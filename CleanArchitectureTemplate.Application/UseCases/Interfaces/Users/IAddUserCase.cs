using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.UseCases.Interfaces.Users
{
    public interface IAddUserUseCase
    {
        Task<OperationResult> ExecuteAsync(AddUserRequestDTO request, CancellationToken cancellationToken);

    }
}
