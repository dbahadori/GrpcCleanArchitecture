﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.DTO.V1.AdminControlPanel
{
    public class UserRegisterResponseDTO
    {
        public required string Id { get; set; }
        public string? FullName { get; set; }
        public required string Email { get; set; }
    }
}
