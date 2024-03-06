using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForjaGames.Json
{
  public class JsonArray : JsonKeyValue, IJsonContainer
  {
    public JsonArray(string name = "")
    {
      this.Name = name;
      Items = new List<JsonKeyValue>();
    }

    public override object Value
    {
      get
      {
        return Items.Select(m => m.Value).ToArray<object>();
      }
    }

    public List<JsonKeyValue> Items { get; set; }

    public override string Type
    {
      get { return Types.Array; }
    }

    public void AddValue(JsonKeyValue value)
    {
      Items.Add(value);
    }

    public override string ToJson(bool complete)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append('[');

      bool first = true;

      foreach (var item in Items)
      {
        if (!first)
        {
          sb.Append(", ");
        }
        else
        {
          first = false;
        }

        if (!complete)
        {
          if (item is JsonObject)
            sb.Append("{...}");
          else if (item is JsonArray)
            sb.Append("[...]");
          else
            sb.Append(item.ToJson(complete));
        }
        else
        {
          sb.Append(item.ToJson(complete));
        }
      }

      sb.Append(']');

      return sb.ToString();
    }

    public static string ToJsonInCascade(Array array, Func<object, string> cascadeJson)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("[");
      int i = 0;

      foreach (object val in array)
      {
        if (i++ > 0)
        {
          sb.Append(", ");
        }

        sb.Append(cascadeJson(val));
      }

      sb.Append("]");

      return sb.ToString();
    }
  }
}
