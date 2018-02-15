using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class CharacterPool : ObjectPool
{
    protected override string Path()
    => "Characters/";

}
