using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Wiki.Models.Constants;

namespace Wiki.Models
{
    public class Body
    {
        public Guid Id { get; set; }

        [Display(Name = "Entradas")]
        [InverseProperty("Body")]
        public List<Entry> Entries { get; set; }

        [Required(ErrorMessage = ErrorMessages.ArticleRequired)]
        [ForeignKey("Article")]
        public Guid ArticleId { get; set; }

        public Article Article { get; set; }
    }
}
