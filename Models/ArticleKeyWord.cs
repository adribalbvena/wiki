using System;

namespace Wiki.Models
{
    public class ArticleKeyWord
    {
        public Guid ArticleId { get; set; }

        public Article Article { get; set; }

        public Guid KeyWordId { get; set; }

        public KeyWord KeyWord { get; set; }
    }
}