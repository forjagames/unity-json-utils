using System.Globalization;

namespace ForjaGames.Json
{
  public class JsonFloat : JsonKeyValue
  {
    private float value;

    public JsonFloat(string name, string value)
    {
      Name = name;
      this.value = float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
    }

    public override object Value
    {
      get { return value; }
    }

    public override string Type
    {
      get { return Types.Float; }
    }

    public override string ToJson(bool complete)
    {
      return value.ToString().Replace(",", ".");
    }
  }
}
