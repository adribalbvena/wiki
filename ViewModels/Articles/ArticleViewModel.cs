using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Wiki.Models.Constants;

namespace Wiki.ViewModels.Articles
{
    public class ArticleViewModel
    {
        public ArticleViewModel()
        {
            this.SecondaryCategories = new List<ArticleCategoryViewModel>();
        }

        public Guid? ArticleId { get; set; }

        [Display(Name = "Categoría principal")]
        public Guid? PrimaryCategoryId { get; set; }

        [StringLength(MaxLength.CategoryName, ErrorMessage = ErrorMessages.StringMaxLength)]
        [RegularExpression(Regex.GeneralStringValidation, ErrorMessage = ErrorMessages.RegexGeneralString)]
        public string Name { get; set; }

        [Display(Name = "Categorías secundarias")]
        public Guid? SecondaryCategoryId { get; set; }

        [Display(Name = "Título")]
        [RegularExpression(Regex.GeneralStringValidation, ErrorMessage = ErrorMessages.RegexGeneralString)]
        [StringLength(MaxLength.ArticleTitle, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        [RegularExpression(Regex.TextFields, ErrorMessage = ErrorMessages.RegexTextFields)]
        [StringLength(MaxLength.ArticleDescription, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string Description { get; set; }

        [Display(Name = "Categorías secundarias")]
        public List<ArticleCategoryViewModel> SecondaryCategories { get; set; }

        public void AddSecondaryCategory(Guid id, string name)
        {
            if (!this.SecondaryCategories.Any(sc => sc.Id == this.SecondaryCategoryId))
            {
                this.SecondaryCategories.Add(new ArticleCategoryViewModel { Id = id, Name = name });
            }
        }

        public void RemoveSecondaryCategory(Guid id)
        {
            this.SecondaryCategories.RemoveAll(sc => sc.Id == id);
        }

        public bool ValidatePrimaryCategory()
        {
            return !this.PrimaryCategoryId.HasValue || !this.SecondaryCategories.Any(sc => sc.Id == this.PrimaryCategoryId.Value);
        }
    }
}
