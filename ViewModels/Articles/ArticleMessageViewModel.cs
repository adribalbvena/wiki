using System;
using System.ComponentModel.DataAnnotations;
using Wiki.Models.Constants;

namespace Wiki.ViewModels.Articles
{
    public class ArticleMessageViewModel
    {
        public string AuthorName { get; set; }

        public DateTime? Date { get; set; }

        private DateTime ReaderDate => this.Date ?? DateTime.MinValue;

        public string ReaderDescription => $"El {this.ReaderDate:dd/MM/yyyy} a las {this.ReaderDate:HH:mm}, {AuthorName} escribió:";

        [Display(Name = "Título")]
        [RegularExpression(Regex.GeneralStringValidation, ErrorMessage = ErrorMessages.RegexGeneralString)]
        [StringLength(MaxLength.MessageTitle, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string Title { get; set; }

        [Display(Name = "Texto")]
        [RegularExpression(Regex.TextFields, ErrorMessage = ErrorMessages.RegexTextFields)]
        [StringLength(MaxLength.MessageText, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string Text { get; set; }
    }
}
