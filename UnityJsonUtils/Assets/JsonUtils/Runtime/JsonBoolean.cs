namespace ForjaGames.Json
{
  public class JsonBoolean : JsonKeyValue
  {
    private bool value;

    public JsonBoolean(string name, string value)
    {
      Name = name;
      this.value = value.ToLower().Equals("true");
    }

    public override object Value
    {
      get { return value; }
    }

    public override string Type
    {
      get { return Types.Boolean; }
    }

    public override string ToJson(bool complete)
    {
      return value ? "true" : "false";
    }
  }
}
