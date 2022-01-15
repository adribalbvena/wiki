namespace Wiki.Models.Constants
{
    public static class Regex
    {
        public const string NameValidation = "[A-Za-zÀ-ÖØ-öø-ÿ ']+";
        public const string WordValidation = "[A-Za-zÀ-ÖØ-öø-ÿ']+";
        public const string GeneralStringValidation = "[A-Za-zÀ-ÖØ-öø-ÿ0-9 ']+";
        public const string TextFields = "[A-Za-zÀ-ÖØ-öø-ÿ0-9 ',.;:()\"\\/%°]+";
    }
}
