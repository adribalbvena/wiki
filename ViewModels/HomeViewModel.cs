using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Models;

namespace Wiki.ViewModels
{
    public class HomeViewModel
    {
        public List<Article> Articles { get; set; }

        public List<Author> Authors { get; set; }

        public List<Category> Categories { get; set; }

        public List<KeyWord> KeyWords { get; set; }

        public HomeViewModel()
        {
            this.Articles = new List<Article>();
            this.Authors = new List<Author>();
            this.KeyWords = new List<KeyWord>();
            this.Categories = new List<Category>();
        }
    }
}