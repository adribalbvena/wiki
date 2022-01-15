using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki.ViewModels
{
    public class Login
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }


        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}
