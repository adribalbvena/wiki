using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Wiki.Models.Constants;

namespace Wiki.Models
{
    [Display(Name = "Categoría")]
    public class Category
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.CategoryName, ErrorMessage = ErrorMessages.StringMaxLength)]
        [RegularExpression(Regex.NameValidation, ErrorMessage = ErrorMessages.RegexName)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Habilitado")]
        public bool Enabled { get; set; } = false;

        [StringLength(MaxLength.CategoryDescription, ErrorMessage = ErrorMessages.StringMaxLength)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [InverseProperty("PrimaryCategory")]
        public List<Article> PrimaryArticles { get; set; }

        [InverseProperty("Category")]
        public List<ArticleCategory> SecondaryArticles { get; set; }

        [NotMapped]
        public string EnabledName
        {
            get
            {
                var result = this.Name;

                if (!this.Enabled)
                {
                    result += " (INACTIVO)";
                }

                return result;
            }
        }
    }
}