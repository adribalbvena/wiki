using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Wiki.Models.Constants;

namespace Wiki.Models
{
    [Display(Name = "Artículo")]
    public class Article
    {
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de creación")]
        public DateTime Date { get; set; }

        [Display(Name = "Habilitado")]
        public bool Enabled { get; set; } = false;

        [Required(ErrorMessage = ErrorMessages.AuthorRequired)]
        [Display(Name = "Autor")]
        [ForeignKey("Author")]
        public Guid AuthorId { get; set; }

        [Display(Name = "Autor")]
        public Author Author { get; set; }

        [Required(ErrorMessage = ErrorMessages.CategoryRequired)]
        [Display(Name = "Categoría primaria")]
        [ForeignKey("PrimaryCategory")]
        public Guid PrimaryCategoryId { get; set; }

        [Display(Name = "Categoría primaria")]
        public Category PrimaryCategory { get; set; }

        [MinLength(3, ErrorMessage = ErrorMessages.MinLength)]
        [Display(Name = "Palabras clave")]
        public List<ArticleKeyWord> KeyWords { get; set; }

        [Display(Name = "Cuerpo")]
        public Body Body { get; set; }

        [Display(Name = "Cabecera")]
        public Header Header { get; set; }

        [Display(Name = "Mensajes")]
        public List<Message> Messages { get; set; }

        [Display(Name = "Referencias")]
        [InverseProperty(nameof(Reference.MainArticle))]
        public List<Reference> MainReferences { get; set; }

        [Display(Name = "Referido por")]
        [InverseProperty("ReferenceArticle")]
        public List<Reference> References { get; set; }

        [Display(Name = "Categorías Secundarias")]
        [InverseProperty("Article")]
        public List<ArticleCategory> SecondaryCategories { get; set; }

        [Display(Name = "Título")]
        public string Title => this.Header?.Title ?? string.Empty;

        [Display(Name = "Descripción")]
        public string Description => this.Header?.Description ?? string.Empty;
    }
}