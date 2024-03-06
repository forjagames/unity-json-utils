using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ForjaGames.Json.Utils;

namespace ForjaGames.Json
{
  public class JsonObject : JsonKeyValue, IJsonContainer
  {
    public JsonObject(string name = "")
    {
      this.Name = name;
      Attributes = new List<JsonKeyValue>();
    }

    public override object Value
    {
      get { return Attributes; }
    }

    public List<JsonKeyValue> Attributes { get; set; }

    public void AddValue(JsonKeyValue value)
    {
      Attributes.Add(value);
    }

    public override string Type
    {
      get { return Types.Object; }
    }

    public override string ToJson(bool complete)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append('{');

      bool first = true;

      foreach (var attribute in Attributes)
      {
        if (!first)
        {
          sb.Append(", ");
        }
        else
        {
          first = false;
        }

        sb.AppendFormat("'{0}': ", attribute.Name);

        if (complete)
        {
          sb.Append(attribute.ToJson(complete));
        }
        else
        {
          if (attribute is JsonObject)
            sb.Append("{...}");
          else if (attribute is JsonArray)
            sb.Append("[...]");
          else
            sb.Append(attribute.ToJson(complete));
        }
      }

      sb.Append('}');

      return sb.ToString();
    }

    public static string ToJsonInCascade(object obj, bool forceCamelcase, Func<object, string> cascadeJson)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("{");
      int i = 0;

      foreach (var prop in TypeUtils.GetProps(obj))
      {
        object value = (prop is PropertyInfo) ?
            (prop as PropertyInfo).GetValue(obj, null) :
            (prop as FieldInfo).GetValue(obj);

        if (i++ > 0)
        {
          sb.Append(", ");
        }

        if (forceCamelcase)
        {
          sb.AppendFormat("\"{0}\"", StringUtils.ToCamelCase(prop.Name));
        }
        else
        {
          sb.AppendFormat("\"{0}\"", prop.Name);
        }

        sb.Append(": ");
        sb.Append(cascadeJson(value));
      }
      sb.Append("}");

      return sb.ToString();
    }

    public static string ToJsonInCascade(IDictionary dict, Func<object, string> cascadeJson)
    {
      {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        int i = 0;

        foreach (var key in dict.Keys)
        {
          if (i++ > 0)
          {
            sb.Append(", ");
          }

#if FORCE_CAMELCASE
        sb.AppendFormat("\"{0}\"", StringUtils.ToCamelCase(key.ToString()));
#else
          sb.AppendFormat("\"{0}\"", key.ToString());
#endif
          sb.Append(": ");
          sb.Append(cascadeJson(dict[key]));
        }

        sb.Append("}");
        return sb.ToString();
      }
    }
  }
}
