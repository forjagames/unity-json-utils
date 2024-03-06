namespace ForjaGames.Json
{
  public class JsonString : JsonKeyValue
  {
    private string value;

    public JsonString(string name, string value)
    {
      this.value = value;
      Name = name;
    }

    public override object Value
    {
      get { return value; }
    }

    public override string Type
    {
      get { return Types.String; }
    }

    public static JsonString ParseJSON(string name, string value)
    {
      return new JsonString(name, value
          .Replace("\\\\", "\\")
          .Replace("\\\"", "\"")
          .Replace("\\n", "\n")
          .Replace("\\t", "\t")
          .Replace("\\r", "\r")
      );
    }

    public override string ToJson(bool complete)
    {
      return ValueToJson(this.value);
    }

    public static string ValueToJson(string value)
    {
      return string.Format("\"{0}\"", value
          .Replace("\\", "\\\\")
          .Replace("\"", "\\\"")
          .Replace("\n", "\\n")
          .Replace("\t", "\\t")
          .Replace("\r", "\\r")
      );
    }
  }
}
