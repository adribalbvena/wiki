using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Wiki.ViewModels.Articles
{
    public class ArticleReferencesViewModel
    {
        public ArticleReferencesViewModel()
        {
            this.References = new List<ArticleReferenceViewModel>();
            this.Results = new List<ArticleReferenceViewModel>();
        }

        public Guid ArticleId { get; set; }

        [Display(Name = "Referencias")]
        public List<ArticleReferenceViewModel> References { get; set; }

        public string Title { get; set; }

        [Display(Name = "Resultados")]
        public List<ArticleReferenceViewModel> Results { get; set; }

        public void AddReference(Guid id, string title)
        {
            /* se agrega solo si el artículo no se repite */
            if (!this.References.Any(r => r.Id == id))
            {
                this.References.Add(new ArticleReferenceViewModel { Id = id, Title = title });
            }
        }

        public void RemoveReference(Guid id)
        {
            this.References.RemoveAll(r => r.Id == id);
        }
    }
}
