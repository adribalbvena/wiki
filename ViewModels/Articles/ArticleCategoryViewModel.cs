using System;

namespace Wiki.ViewModels.Articles
{
    public class ArticleCategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }
    }
}
