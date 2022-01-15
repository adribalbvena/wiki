using System.Collections.Generic;
using Wiki.Models;

namespace Wiki.ViewModels.Articles
{
    public class ArticleNavigationViewModel
    {
        public ArticleNavigationViewModel()
        {
        }

        public Category Category { get; set; }

        public KeyWord KeyWord { get; set; }

        public List<Article> Articles { get; set; }
    }
}
