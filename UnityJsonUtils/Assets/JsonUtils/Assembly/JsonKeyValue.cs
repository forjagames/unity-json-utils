namespace ForjaGames.Json
{
  public abstract class JsonKeyValue
  {
    public string Name { get; set; }

    public abstract string Type { get; }

    public abstract object Value { get; }

    public abstract string ToJson(bool complete);

    public override string ToString()
    {
      return Name + "=" + ToJson(false);
    }
  }
}
