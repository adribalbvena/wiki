using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Wiki.Models.Constants;

namespace Wiki.Models
{
    public class Entry
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [Range(1, int.MaxValue)]
        public int Order { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.EntryTitle, ErrorMessage = ErrorMessages.StringMaxLength)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [StringLength(MaxLength.EntrySubTitle, ErrorMessage = ErrorMessages.StringMaxLength)]
        [Display(Name = "Subtítulo")]
        public string Subtitle { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.EntryText, ErrorMessage = ErrorMessages.StringMaxLength)]
        [Display(Name = "Texto")]
        public string Text { get; set; }

        [Required(ErrorMessage = ErrorMessages.BodyRequired)]
        [ForeignKey("Body")]
        public Guid BodyId { get; set; }

        public Body Body { get; set; }
    }
}
