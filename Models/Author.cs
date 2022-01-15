using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wiki.Models
{
    public class Author : User
    {
        [InverseProperty("Author")]
        public List<Article> Articles { get; set; }
    }
}
