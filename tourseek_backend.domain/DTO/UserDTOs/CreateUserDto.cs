﻿using System;
using System.Collections.Generic;
using tourseek_backend.domain.DTO.RoleDTOs;

namespace tourseek_backend.domain.DTO.UserDTOs
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<string> Roles { get; set; }

    }
}
