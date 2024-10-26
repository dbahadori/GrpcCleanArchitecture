using CleanArchitectureTemplate.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.UseCases.Interfaces.Users
{
    public interface IDeleteUserUseCase
    {
        Task<OperationResult> ExecuteAsync(Guid id, CancellationToken cancellationToken);

    }
}
