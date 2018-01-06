using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class TSVLoader
{
    public static Dictionary<string, T> ReadDictionary<T>(string text)
        => ReadDictionary<T>(GetLines(text));

    public static List<T> ReadList<T>(string text)
        => ReadList<T>(GetLines(text));

    static IEnumerable<string> GetLines(string text)
    {
        using (var reader = new StringReader(text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }

    public static Dictionary<string, T> ReadDictionary<T>(IEnumerable<string> lines)
        => lines.Skip(1).Select(line => line.Split('\t'))
            .ToDictionary(l => l.First(), l => ParseObject<T>(l.Skip(1).ToArray()));

    public static List<T> ReadList<T>(IEnumerable<string> lines)
        => lines.Skip(1).Select(line => line.Split('\t')).Select(ParseObject<T>)
            .ToList();

    static T ParseObject<T>(string[] splitted)
    {
        var type = typeof(T);
        if (type.IsPrimitive || type == typeof(string))
            return (T) Convert.ChangeType(splitted[0], type);
        var fields = type.GetFields();
        var newT = Activator.CreateInstance(type);
        for (var i = 0; i < fields.Length; i++)
        {
            if (splitted.Count() == i) break;
            if (splitted[i] == "") continue;
            fields[i].SetValue(newT, Convert.ChangeType(splitted[i], fields[i].FieldType));
        }
        return (T) newT;
    }
}