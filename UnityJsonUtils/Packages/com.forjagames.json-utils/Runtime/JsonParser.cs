#define FORCE_CAMELCASE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ForjaGames.Json.Utils;


#if UNITY_EDITOR
using UnityEngine;
#endif

namespace ForjaGames.Json
{
  /// <summary>
  /// Converts any object into a JSON String.
  ///  
  /// Usage:
  ///
  /// string objJSON = JsonHelper.Json(myObject);
  /// string scrJSON = JsonHelper.Json(myScript);
  /// string arrayJSON = JsonHelper.Json(myArray);
  ///
  /// </summary>
  public static class JsonParser
  {
    public static JsonObject InterpretJson(string json)
    {
      json = json.Trim();

      if (json.StartsWith("<"))
      {
        throw new Exception("The data to parse is not JSON.");
      }

      if (json.StartsWith("{") && json.EndsWith("}"))
      {
        json = json.Substring(1);
      }

      string open = "";
      int status = 0;
      JsonObject result = new JsonObject();
      IJsonContainer currentObject = result;
      var stack = new Stack<IJsonContainer>();
      //stack.Push(currentObject);

      string attributeName = "";
      string type = "";
      string accumulator = "";

#if !UNITY && DEBUG
      string debug = "";
#endif

      for (int i = 0; i < json.Length; i++)
      {
        string chr = json[i].ToString();

#if !UNITY && DEBUG
        debug += chr;
#endif

        if (open == "" && status != 2)
        {
          if (Regex.IsMatch(chr, @"\s|:"))
            continue;

          #region if(status == 1)

          if (status == 1)
          {
            if (chr == "{")
            {
              open = "";
              type = "object";

#if !UNITY
              System.Diagnostics.Debug.WriteLine("Entrando en " + attributeName);
#endif

              var newObject = new JsonObject(attributeName);
              currentObject.AddValue(newObject);
              stack.Push(currentObject);

              currentObject = newObject;
#if !UNITY
              System.Diagnostics.Debug.WriteLine("Agregado a stack: " + ((JsonKeyValue)currentObject).Name);
              System.Diagnostics.Debug.WriteLine("Stack es ahora: " + PrintStack(stack));
              System.Diagnostics.Debug.WriteLine("Current: " + ((JsonKeyValue)currentObject).Name);
#endif

              status = 0;
            }
            else if (chr == "[")
            {
              open = "";
              type = "array";

#if !UNITY
              System.Diagnostics.Debug.WriteLine("Entrando en " + attributeName);
#endif

              var newObject = new JsonArray(attributeName);
              currentObject.AddValue(newObject);
              stack.Push(currentObject);
              currentObject = newObject;
              attributeName = "array_item";

#if !UNITY
              System.Diagnostics.Debug.WriteLine("Agregado a stack: " + ((JsonKeyValue)stack.Peek()).Name);
              System.Diagnostics.Debug.WriteLine("Stack es ahora: " + PrintStack(stack));
              System.Diagnostics.Debug.WriteLine("Current: " + ((JsonKeyValue)currentObject).Name);
#endif

              status = 1;
            }
            else if (Regex.IsMatch(chr, @"[0-9]"))
            {
              open = "number";
              type = "number";
              accumulator += chr;
            }
            else if ("tf".Contains(chr))
            {
              open = "boolean";
              type = "boolean";
              accumulator += chr;
            }
            else if (chr == "n")
            {
              open = "null";
              type = "null";
              accumulator += chr;
            }
            else if ("}]".Contains(chr))
            {
              i--;
              status = 2;
            }
          }

          #endregion

          if (chr == "\"" || chr == "'")
          {
            open = chr;
            if (status == 1)
              type = "string";
          }

          if (chr == "}")
          {
            i--;
            status = 2;
          }

        }
        else if (status == 0)
        {
          if (chr == open)
          {
            attributeName = accumulator;
            open = "";
            accumulator = "";
            status = 1;
          }
          else
          {
            accumulator += chr;
          }
        }
        else if (status == 1)
        {
          #region (...)

          if (type == "string")
          {
            if (chr == open)
            {
              currentObject.AddValue(JsonString.ParseJSON(attributeName, accumulator));
              open = "";
              accumulator = "";
              status = 2;

            }
            else
            {
              accumulator += chr;
            }
          }
          else if (type == "number")
          {
            if (!Regex.IsMatch(chr, "([0-9]|[.])"))
            {
              if (accumulator.Contains(".") || accumulator.Contains(","))
                currentObject.AddValue(new JsonFloat(attributeName, accumulator));
              else
                currentObject.AddValue(new JsonInt(attributeName, accumulator));

              open = "";
              accumulator = "";
              i--;
              status = 2;

            }
            else
            {
              accumulator += chr;
            }
          }
          else if (type == "boolean")
          {
            if (!Regex.IsMatch(chr, "([a-zA-Z])"))
            {
              currentObject.AddValue(new JsonBoolean(attributeName, accumulator));

              open = "";
              accumulator = "";
              i--;
              status = 2;

            }
            else
            {
              accumulator += chr;
            }
          }
          else if (type == "null")
          {
            if (!Regex.IsMatch(chr, "([a-zA-Z])"))
            {
              currentObject.AddValue(new JsonNull(attributeName));

              open = "";
              accumulator = "";
              i--;
              status = 2;

            }
            else
            {
              accumulator += chr;
            }
          }
          else if (type == "array")
          {
            throw new Exception("You should not be here");
          }
          else if (type == "object")
          {
            throw new Exception("You should not be here");
          }

          #endregion
        }
        else if (status == 2)
        {
          #region (...)

          if (Regex.IsMatch(chr, @"\s"))
          {
            continue;
          }

          if (chr == ",")
          {
            status = (currentObject is JsonArray /*|| currentObject is JsonObject*/) ? 1 : 0;
          }
          else if (chr == "]")
          {
            currentObject = stack.Pop();
          }
          else if (chr == "}")
          {
            if (stack.Count == 0)
              break;

            currentObject = stack.Pop();
          }

          #endregion
        }
      }

      return result;
    }

    private static string PrintStack(Stack<IJsonContainer> stack)
    {
      if (stack.Count == 0)
        return "[Empty stack]";

      List<string> result = new List<string>();

      foreach (var item in stack)
      {
        string nombre = ((JsonKeyValue)item).Name;

        result.Add(string.IsNullOrEmpty(nombre) ? "ROOT" : nombre);
      }

      result.Reverse();

      return string.Join(" > ", result.ToArray());
    }

    private static void CombineJSON<T>(JsonObject interpreted, ref T obj)
    {
      object generic = obj;
      CombineJSON(interpreted, ref generic);
    }

    private static void SetValue(MemberInfo info, object target, object value)
    {
      if (info is PropertyInfo)
      {
        (info as PropertyInfo).SetValue(target, value, null);
      }
      else
      {
        (info as FieldInfo).SetValue(target, value);
      }
    }

    private static void CombineJSON(JsonObject interpreted, ref object obj)
    {
      Type objType = obj.GetType();

#if !CASE_SENSITIVE
      var props = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      var fields = objType.GetFields(BindingFlags.Instance | BindingFlags.Public);
#endif

      foreach (var attr in interpreted.Attributes)
      {
#if !CASE_SENSITIVE
        var prop = (props.FirstOrDefault(m => m.Name.ToLowerInvariant() == attr.Name.ToLowerInvariant()) as MemberInfo) ??
             fields.FirstOrDefault(m => m.Name.ToLowerInvariant() == attr.Name.ToLowerInvariant());
#else
                var prop =
                    ((MemberInfo)objType.GetProperty(attr.Name, BindingFlags.Instance | BindingFlags.Public)) ??
                    objType.GetField(attr.Name, BindingFlags.Instance | BindingFlags.Public);
#endif

        if (prop == null)
        {
#if UNITY_EDITOR
          Debug.LogWarning("Property not found: " + attr.Name + " on " + objType.Name);
#endif
          continue;
        }

        var type = TypeUtils.GetMemberType(prop);

        if (attr.Type == Types.Object)
        {
          if (type == typeof(string))
          {
            SetValue(prop, obj, attr.ToJson(true));
          }
          else
          {
            var subObj = Activator.CreateInstance(type);

            if (TypeUtils.IsDictionary(type))
            {
              var dict = subObj as IDictionary;
              var jsonObj = attr as JsonObject;
              var argType = dict.GetType().GetGenericArguments()[1];

              foreach (var subAttr in jsonObj.Attributes)
              {
                if (argType.IsClass && argType != typeof(string))
                {
                  var argJson = InterpretJson(JsonParser.Json(subAttr.Value));
                  var argObj = Activator.CreateInstance(argType);

                  CombineJSON(argJson, ref argObj);

                  dict[subAttr.Name] = argObj;

                }
                else
                {
                  dict[subAttr.Name] = subAttr.Value;
                }
              }
            }
            else
            {
              CombineJSON(attr as JsonObject, ref subObj);
            }

            SetValue(prop, obj, subObj);
          }

        }
        else if (attr.Type == Types.Array)
        {
          JsonArray arrayAttr = attr as JsonArray;

          if (arrayAttr.Items.Count == 0)
          {
            object[] array = new object[] { };
            Array destinationArray = Array.CreateInstance(type.GetElementType(), 0);
            Array.Copy(array, destinationArray, array.Length);
            SetValue(prop, obj, destinationArray);
          }
          else if (arrayAttr.Items.Count > 0)
          {
            string arrayAttrItemType = arrayAttr.Items[0].Type;
            if (type.IsArray)
            {
              object[] array;

              if (arrayAttrItemType != Types.Object)
              {
                array = (object[])attr.Value;
              }
              else
              {
                int count = arrayAttr.Items.Count;
                array = new object[count];

                for (int i = 0; i < count; i++)
                {
                  var item = arrayAttr.Items[i];
                  var subObj = Activator.CreateInstance(type.GetElementType());

                  CombineJSON(item as JsonObject, ref subObj);

                  array[i] = subObj;
                }
              }

              Array destinationArray = Array.CreateInstance(type.GetElementType(), array.Length);
              Array.Copy(array, destinationArray, array.Length);
              SetValue(prop, obj, destinationArray);
            }
            else
            {
              var instance = (IList)Activator.CreateInstance(type);

              foreach (var item in arrayAttr.Items)
              {
                if (item.Type == Types.Object)
                {
                  var subObj = Activator.CreateInstance(type.GetGenericArguments()[0]);
                  CombineJSON(item as JsonObject, ref subObj);
                  instance.Add(subObj);
                }
                else
                {
                  instance.Add(item.Value);
                }

              }

              SetValue(prop, obj, instance);
            }
          }
        }
        else if (type.IsEnum)
        {
          try
          {
            if (attr.Value != null)
            {
              SetValue(prop, obj, Convert.ChangeType(attr.Value, Enum.GetUnderlyingType(type)));
            }
          }
          catch
          {
#if UNITY_EDITOR
            Debug.LogWarning("Cannot assign value '" + attr.Value.ToString() + "' to '" + attr.Name + "' type '" + type.Name + "'.");
#endif
          }
        }
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
          try
          {
            if (attr.Value == null)
            {
              SetValue(prop, obj, null);
            }
            else
            {
              SetValue(prop, obj, Convert.ChangeType(attr.Value, Nullable.GetUnderlyingType(type)));
            }
          }
          catch
          {
#if UNITY_EDITOR
            Debug.LogWarning("Cannot assign value '" + attr.Value.ToString() + "' to '" + attr.Name + "' type '" + type.Name + "'.");
#endif
          }
        }
        else
        {
          try
          {
            SetValue(prop, obj, Convert.ChangeType(attr.Value, type));
          }
          catch
          {
#if UNITY_EDITOR
            Debug.LogWarning("Cannot assign value '" + attr.Value.ToString() + "' to '" + attr.Name + "' type '" + type.Name + "'.");
#endif
          }
        }
      }
    }

    class InterpretedFixed<T>
    {
      public T Value { get; set; }
    }

    public static T Parse<T>(string json)
    {
      return Parse<T>(json, null);
    }

    public static T Parse<T>(string json, object instanceToFill)
    {
      T result = (instanceToFill == null) ? Activator.CreateInstance<T>() : (T)instanceToFill;

      if (json.Contains(">k__BakingField"))
      {
        json = Regex.Replace(json, @"<(([a-zA-Z0-9]|_|-)+)>k__BackingField", "$1");
      }

      int type = 0;

      if (!json.Trim().StartsWith("{"))
      {
        type = 1;
        json = "{\"Value\": " + json + "}";
      }

      if (type == 1)
      {
        var fix = new InterpretedFixed<T>();
        var interpretedObj = InterpretJson(json);
        CombineJSON(interpretedObj, ref fix);
        result = fix.Value;
      }
      else
      {
        var interpretedObj = InterpretJson(json);
        CombineJSON(interpretedObj, ref result);
      }

      return result;
    }

    public static string Json(object obj)
    {
      if (obj == null)
      {
        return "null";
      }

      var type = obj.GetType();

      if (obj is DateTime)
      {
        return "\"" + ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss") + "\"";
      }

      if (obj is float || obj is decimal || obj is double)
      {
        return obj.ToString().Replace(',', '.');
      }

      if (obj is bool)
      {
        return ((bool)obj) ? "true" : "false";
      }

      if (type.IsPrimitive)
      {
        return obj.ToString();
      }

      if (TypeUtils.IsGenericList(obj))
      {
        var collection = (IEnumerable)obj;
        return JsonArray.ToJsonInCascade(collection.Cast<object>().ToArray(), Json);
      }

      if (type == typeof(string))
      {
        return JsonString.ValueToJson(obj.ToString());
      }

      if (type.IsArray)
      {
        return JsonArray.ToJsonInCascade(obj as Array, Json);
      }

      if (type.IsClass)
      {
        if (TypeUtils.IsDictionary(type))
        {
          return JsonObject.ToJsonInCascade((IDictionary)obj, Json);
        }
        else
        {
#if FORCE_CAMELCASE
          return JsonObject.ToJsonInCascade(obj, true, Json);
#else
          return JsonObject.ToJsonInCascade(obj, false, Json);
#endif
        }
      }

      if (type.IsEnum)
      {
        return Convert.ToInt32(obj).ToString();
      }

      return obj.ToString();
    }
  }
}
