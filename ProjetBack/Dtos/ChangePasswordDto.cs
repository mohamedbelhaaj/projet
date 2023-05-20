﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Dtos
{
    public class ChangePasswordDto
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
