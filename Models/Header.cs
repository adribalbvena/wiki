using System;
using System.ComponentModel.DataAnnotations;

using Wiki.Models.Constants;

namespace Wiki.Models
{
    public class Header
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.ArticleTitle, ErrorMessage = ErrorMessages.StringMaxLength)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [StringLength(MaxLength.ArticleDescription, ErrorMessage = ErrorMessages.StringMaxLength)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.AuthorRequired)]
        public Guid ArticleId { get; set; }

        public Article Article { get; set; }
    }
}
