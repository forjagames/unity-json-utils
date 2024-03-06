namespace ForjaGames.Json
{
  public class JsonInt : JsonKeyValue
  {
    private int value;

    public JsonInt(string name, string value)
    {
      Name = name;
      this.value = int.Parse(value);
    }

    public override object Value
    {
      get { return value; }
    }

    public override string Type
    {
      get { return Types.Integer; }
    }

    public override string ToJson(bool complete)
    {
      return value.ToString();
    }
  }
}
