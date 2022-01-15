using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Wiki.ViewModels.Articles
{
    public class ArticleKeyWordsViewModel
    {
        public ArticleKeyWordsViewModel()
        {
            this.KeyWords = new List<KeyWordViewModel>();
            this.KeyWord = new KeyWordViewModel();
            this.Results = new List<KeyWordViewModel>();
        }

        public Guid ArticleId { get; set; }

        [Display(Name = "Palabras clave")]
        public List<KeyWordViewModel> KeyWords { get; set; }

        [Display(Name = "Resultados")]
        public List<KeyWordViewModel> Results { get; set; }

        public string SearchText { get; set; }

        public KeyWordViewModel KeyWord { get; set; }

        public void AddKeyWord()
        {
            this.AddKeyWord(this.KeyWord.Id, this.KeyWord.Word);
            this.KeyWord = new KeyWordViewModel();
        }

        public void AddKeyWord(Guid? id, string word)
        {
            /* se agrega solo si la palabra no se repite */
            if (!this.KeyWords.Any(kw => kw.Word == word))
            {
                this.KeyWords.Add(new KeyWordViewModel { Id = id, Word = word });
            }
        }

        public void RemoveKeyWord(string word)
        {
            this.KeyWords.RemoveAll(sc => sc.Word == word);
        }
    }
}
