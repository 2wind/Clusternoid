using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class CharacterPool : ObjectPool<CharacterPool>
{
    protected override string Path()
        => "Characters/";

    public static void ClearPool() => instance.Clear();
}