using System;

namespace Wiki.Models
{
    public class Reference
    {
        public Guid MainArticleId { get; set; }

        public Article MainArticle { get; set; }

        public Guid ReferenceArticleId { get; set; }

        public Article ReferenceArticle { get; set; }
    }
}