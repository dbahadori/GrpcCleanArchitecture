using AutoMapper;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Infrastructure.CrossCutting.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserResponseDTO>();
        }

    }

}
