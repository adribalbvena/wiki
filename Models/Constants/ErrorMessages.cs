namespace Wiki.Models.Constants
{
    public static class ErrorMessages
    {
        public const string Required = "El campo {0} es requerido";
        public const string DataTypeDateTime = "Fecha y hora inválidos";
        public const string DataTypeEmail = "Email inválido";
        public const string DataTypePassword = "Contraseña inválida";
        public const string DataTypePhoneNumber = "Teléfono inválido";
        public const string StringLength = "El valor debe estar entre {2} y {1} caracteres";
        public const string StringMaxLength = "El valor no puede superar los {1} caracteres";
        public const string MinLength = "Deben haber al menos 3 palabras claves";
        public const string CategoryRequired = "La categoria es obligatoria";
        public const string CategoryAlreadyExists = "La categoria ya existe";
        public const string CategoryDoesNotExist = "La categoria no existe";
        public const string CategoryAlreadyPrimary = "La categoria ya es primaria";
        public const string CategoryAlreadySecondary = "La categoria ya es secundaria";
        public const string KeyWordRequired = "La palabra clave es requerida";
        public const string KeyWordAlreadyExists = "La palabra clave ya existe";
        public const string ArticleRequired = "El artículo es obligatorio";
        public const string ArticleInvalid = "El artículo es inválido";
        public const string ArticleAlreadyExists = "Ya existe un artículo con ese título";

        public const string BodyRequired = "El cuerpo es obligatorio";
        public const string UserRequired = "El usuario es obligatorio";
        public const string PasswordRequiredNonAlphanumeric = "La contrasena debe ser alfanumerica";

        public const string AuthorRequired = "El autor es obligatorio";

        public const string WarningMessageLabel = "ADVERTENCIA: {0}";

        public const string RegexName = "El campo solo acepta letras, espacios y '";
        public const string RegexWord = "El campo solo acepta letras, y '";
        public const string RegexGeneralString = "El campo solo acepta letras, números, espacios y '";
        public const string RegexTextFields = "El campo solo acepta letras, números, espacios y los siguientes símbolos: ' , . ; : ( ) \" / % °";
    }
}
