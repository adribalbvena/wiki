using System.Collections.Generic;

namespace Wiki.Models.Constants
{
    public class Const
    {
        public const int SearchResults = 10;
        public const int TopAuthors = 4;
        public const int TopArticles = 6;

        public const string AdminPassword = "Password1";

        public const string AdminRoleName = "Admin";
        public const string AuthorRoleName = "Author";

        public static readonly List<string> Roles = new List<string>() { AdminRoleName, AuthorRoleName };
    }
}
