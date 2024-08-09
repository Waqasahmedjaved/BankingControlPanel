using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.DTOs
{
    public class RegistrationDTO
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Role")]
        public string Role { get; set; }
    }
}
