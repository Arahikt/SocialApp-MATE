using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        
        //Data Anotation is required( return user)
        [Required]
        public string Username { get; set; }
        //max 8, min 4
        [Required]
        [StringLength(10, MinimumLength =4)]
        public string Password { get; set; }
    }
}