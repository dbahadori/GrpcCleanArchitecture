using Azure.Core;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Domain.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Infrastructure.CrossCutting.Validation.DTO
{
    public class AddUserRequestFlentValidator: AbstractValidator<AddUserRequestDTO>
    {
        public AddUserRequestFlentValidator(){
            RuleFor(request => request.NationalCode)
                .NotNull()
                    .WithMessage("The National Code field is required.");
        }
        
    }
}
