namespace ForjaGames.Json
{
  public class JsonNull : JsonKeyValue
  {
    public JsonNull(string name)
    {
      Name = name;
    }

    public override object Value
    {
      get { return null; }
    }

    public override string Type
    {
      get { return Types.Null; }
    }

    public override string ToJson(bool complete)
    {
      return "null";
    }
  }
}
