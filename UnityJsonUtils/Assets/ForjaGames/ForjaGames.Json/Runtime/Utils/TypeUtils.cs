using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ForjaGames.Json.Utils
{
  internal class TypeUtils
  {
    internal static bool IsDictionary(Type t)
    {
      bool isDict = t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
      return isDict;
    }

    internal static bool IsGenericList(object o)
    {
      bool isGenericList = false;

      var oType = o.GetType();

      if (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)))
      {
        isGenericList = true;
      }

      return isGenericList;
    }

    internal static Type GetMemberType(MemberInfo info)
    {
      if (info is PropertyInfo)
      {
        return (info as PropertyInfo).PropertyType;
      }
      else
      {
        return (info as FieldInfo).FieldType;
      }
    }

    internal static IEnumerable<MemberInfo> GetProps(object obj)
    {
      Type t = obj.GetType();
      List<MemberInfo> result = new List<MemberInfo>();

      BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

#if UNITY_EDITOR
      if (t.BaseType == typeof(MonoBehaviour) || t.BaseType == typeof(ScriptableObject))
      {
        flags = flags | BindingFlags.DeclaredOnly;
      }
#endif

      bool isAnonymousType = t.Name.Contains("AnonymousType");

      foreach (PropertyInfo prop in t
          .GetProperties(flags)
          .Where(m => m.CanRead && (isAnonymousType || m.CanWrite))
      )
      {
        //if (HasAttribute<Ignore>(prop, true))
        //    continue;

        result.Add(prop);
      }

      foreach (FieldInfo prop in t.GetFields(flags))
      {
        //if (HasAttribute<Ignore>(prop, true))
        //    continue;

        result.Add(prop);
      }


      return result;
    }
  }
}
