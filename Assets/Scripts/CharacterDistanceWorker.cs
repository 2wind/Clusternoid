using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class CharacterDistanceWorker
{
    public Dictionary<Character, List<Character>> result;
    readonly Dictionary<Character, List<Character>> calculation = new Dictionary<Character, List<Character>>();
    readonly Dictionary<Character, Vector3> positions = new Dictionary<Character, Vector3>();
    readonly HashSet<List<Character>> listpool = new HashSet<List<Character>>();
    HashSet<Tuple<Character, Character>> charPairs = new HashSet<Tuple<Character, Character>>();

    public IEnumerator CalculateCharacterDistance(HashSet<Tuple<Character, Character>> _charPairs)
    {
        result = null;
        charPairs.Clear();
        charPairs.UnionWith(_charPairs);
        listpool.UnionWith(calculation.Values);
        calculation.Clear();
        positions.Clear();
        foreach (var character in charPairs)
        {
            if (calculation.ContainsKey(character.Item1)) continue;
            List<Character> list;
            if (listpool.Count != 0)
            {
                list = listpool.First();
                listpool.Remove(list);
                list.Clear();
            }
            else
            {
                list = new List<Character>();
            }
            calculation.Add(character.Item1, list);
            positions.Add(character.Item1, character.Item1.transform.position);
        }
        var task = Task.Run(() => CalculateTask());
        while (!task.IsCompleted) yield return null;
        //return result
        result = calculation;
    }

    void CalculateTask()
    {
        foreach (var pair in charPairs)
        {
            if (Vector2.Distance(positions[pair.Item1], positions[pair.Item2]) <
                pair.Item1.repulsionCollisionRadius)
                calculation[pair.Item1].Add(pair.Item2);
        }
    }
}