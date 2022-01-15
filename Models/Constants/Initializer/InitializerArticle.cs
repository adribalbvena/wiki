using System.Collections.Generic;

namespace Wiki.Models.Constants.Initializer
{
    public class InitializerArticle
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string PrimaryCategory { get; set; }

        public List<string> SecondaryCategories { get; set; } = new List<string>();

        public List<string> KeyWords { get; set; } = new List<string>();

        public List<string> References { get; set; } = new List<string>();

        public List<InitializerEntry> Entries { get; set; } = new List<InitializerEntry>();

        public List<InitializerMessage> Messages { get; set; } = new List<InitializerMessage>();
    }
}
