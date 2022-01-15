using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wiki.ViewModels.Articles
{
    public class ArticleReaderViewModel
    {
        public ArticleReaderViewModel()
        {
            this.SecondaryCategories = new List<ArticleCategoryViewModel>();
            this.References = new List<ArticleReferenceViewModel>();
            this.KeyWords = new List<KeyWordViewModel>();
            this.Entries = new List<ArticleEntryViewModel>();
            this.Messages = new List<ArticleMessageViewModel>();
        }

        //public bool Enabled { get; set; }

        public bool CanEdit { get; set; }

        public Guid? ArticleId { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Categoría principal")]
        public ArticleCategoryViewModel PrimaryCategory { get; set; }

        [Display(Name = "Categorías secundarias")]
        public List<ArticleCategoryViewModel> SecondaryCategories { get; set; }

        [Display(Name = "Referencias")]
        public List<ArticleReferenceViewModel> References { get; set; }

        [Display(Name = "Palabras clave")]
        public List<KeyWordViewModel> KeyWords { get; set; }

        [Display(Name = "Entradas")]
        public List<ArticleEntryViewModel> Entries { get; set; }

        [Display(Name = "Mensajes")]
        public List<ArticleMessageViewModel> Messages { get; set; }

        public ArticleMessageViewModel NewMessage { get; set; }
    }
}
