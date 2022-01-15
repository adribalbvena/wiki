using System;
using System.ComponentModel.DataAnnotations;

using Wiki.Models.Constants;

namespace Wiki.ViewModels
{
    public class KeyWordViewModel
    {
        public KeyWordViewModel()
        {

        }

        public Guid? Id { get; set; }

        [StringLength(MaxLength.KeyWordWord, ErrorMessage = ErrorMessages.StringMaxLength)]
        [RegularExpression(Regex.WordValidation, ErrorMessage = ErrorMessages.RegexWord)]
        public string Word { get; set; }
    }
}
