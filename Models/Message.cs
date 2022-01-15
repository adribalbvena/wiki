using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Wiki.Models.Constants;

namespace Wiki.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.DataTypeDateTime)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.MessageTitle, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(MaxLength.MessageText, ErrorMessage = ErrorMessages.StringMaxLength)]
        public string Text { get; set; }

        [Required(ErrorMessage = ErrorMessages.UserRequired)]
        /* La relación es con User debido a que los administradores (no autores) también pueden escribir mensajes */
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required(ErrorMessage = ErrorMessages.ArticleRequired)]
        [ForeignKey("Article")]
        public Guid ArticleId { get; set; }

        public Article Article { get; set; }
    }
}