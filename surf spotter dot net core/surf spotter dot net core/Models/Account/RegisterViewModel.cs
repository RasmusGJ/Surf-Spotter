using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace surf_spotter_dot_net_core.Models.Account
{
    public class RegisterViewModel
    {
        //Data required in registration fields
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,50}$", ErrorMessage = "Password must be 8 characters long and have an uppercase letter, and a number")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //AccountStatus might be implimented in the future, to verify permissions and 
        //features.

        //public int AccountStatus { get; set; }
    }
}
