using System;
using System.ComponentModel.DataAnnotations;
using Wiki.Models.Constants;

namespace Wiki.ViewModels.Articles
{
    public class ArticleEntryViewModel
    {
        public ArticleEntryViewModel()
        {

        }

        public Guid? Id { get; set; }

        public int Order { get; set; }

        [Display(Name = "Título")]
        [RegularExpression(Regex.GeneralStringValidation, ErrorMessage = ErrorMessages.RegexGeneralString)]
        [StringLength(MaxLength.EntryTitle, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string Title { get; set; }

        [Display(Name = "Subtítulo")]
        [RegularExpression(Regex.GeneralStringValidation, ErrorMessage = ErrorMessages.RegexGeneralString)]
        [StringLength(MaxLength.EntrySubTitle, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string SubTitle { get; set; }

        [Display(Name = "Texto")]
        [RegularExpression(Regex.TextFields, ErrorMessage = ErrorMessages.RegexTextFields)]
        [StringLength(MaxLength.EntryText, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string Text { get; set; }

        public bool Editor { get; set; } = false;
    }
}
