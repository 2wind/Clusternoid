using System.Collections.Generic;
using UnityEngine;

public static class DataTables
{
    public static readonly AttackTable attacks = new AttackTable();
}

public abstract class Table<T>
{
    public abstract string Gid();
    public abstract string Path();
    Dictionary<string, T> table;

    public T this[string key]
    {
        get
        {
            if (table != null) return table[key];
            var text = Resources.Load<TextAsset>(Path()).text;
            table = TSVLoader.ReadDictionary<T>(text);
            return table[key];
        }
    }
}

public class AttackTable : Table<AttackData>
{
    public override string Gid() => "0";
    public override string Path() => "Data/AttackTable";
}