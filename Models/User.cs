using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Wiki.Models.Constants;

namespace Wiki.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [DataType(DataType.Date, ErrorMessage = ErrorMessages.DataTypePassword)]
        [Display(Name = "Fecha De Creacion")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.UserFirstName, ErrorMessage = ErrorMessages.StringMaxLength)]
        [RegularExpression(Regex.NameValidation, ErrorMessage = ErrorMessages.RegexName)]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.UserLastName, ErrorMessage = ErrorMessages.StringMaxLength)]
        [RegularExpression(Regex.NameValidation, ErrorMessage = ErrorMessages.RegexName)]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Telefono")]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessages.DataTypePhoneNumber)]
        public string Phone { get; set; }

        [InverseProperty("User")]
        [Display(Name = "Mensaje")]
        public List<Message> Messages { get; set; }

        [NotMapped]
        public string CompleteName => $"{this.FirstName} {this.LastName}";
    }
}
