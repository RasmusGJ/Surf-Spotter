using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace surf_spotter_dot_net_core.Models.Account
{
    public class LoginViewModel
    {
        //Properties required in login fields: UserName, Password.
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,50}$", ErrorMessage = "Password must be 8 characters long and have an uppercase letter, and a number")]
        public string Password { get; set; }

        //Required not specified, this is optional. 
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
