using System.Text.RegularExpressions;

namespace HS
{
    public static partial class CoreExtensions
    {

        public static string String(this Guid id)
        {
            return Convert.ToBase64String(id.ToByteArray())
              .Replace("/", "-")
              .Replace("+", "_")
              .Replace("=", "");
        }

        public static string? String(this Guid? id)
        {
            if (id.HasValue)
                return String(id.Value);
            return null;
        }

        public static bool IsGuid(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            if (str.Length != 22)
                return false;
            return GuidRegex().IsMatch(str);
        }

        public static Guid Guid(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);
            if (!IsGuid(str)) throw new GuidFormatException(str);

            var tmp = str.Replace("-", "/")
              .Replace("_", "+");
            return new Guid(Convert.FromBase64String(tmp + "=="));
        }

        public static Guid? GuidONull(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return Guid(str);
        }

        [GeneratedRegex("[a-zA-Z0-9_-]{22}")]
        private static partial Regex GuidRegex();
    }
}
