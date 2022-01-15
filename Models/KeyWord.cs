using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wiki.Models.Constants;

namespace Wiki.Models
{
    [Display(Name = "Palabra Clave")]
    public class KeyWord
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.KeyWordWord, ErrorMessage = ErrorMessages.StringMaxLength)]
        [RegularExpression(Regex.WordValidation, ErrorMessage = ErrorMessages.RegexWord)]
        [Display(Name = "Palabra")]
        public string Word { get; set; }

        public List<ArticleKeyWord> Articles { get; set; }
    }
}