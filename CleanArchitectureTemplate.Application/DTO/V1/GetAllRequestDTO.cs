using CleanArchitectureTemplate.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Interfaces;
using Ticketing.Domain.Interfaces;

namespace CleanArchitectureTemplate.Application.DTO.V1
{
    public class GetAllRequestDTO
    {
        public PaginationRequestMetadata PaginationRequestMetadata { get; set; }
        public IFilteringMetaData FilteringMetadata { get; set; }

    }

    public class GetAllRequestDTO<Key>: GetAllRequestDTO
    {
        [JsonIgnore]
        public required Key Id { get; set; }
    }
}
