namespace ForjaGames.Json.Utils
{
  internal static class StringUtils
  {
    public static string ToCamelCase(string text)
    {
      if (string.IsNullOrEmpty(text) || text.Length == 1)
      {
        return text;
      }

      return text[0].ToString().ToLower() + text.Substring(1);
    }
  }
}
