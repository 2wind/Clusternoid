using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : ObjectPool<EffectPool>
{
    protected override string Path()
    => "Effects/";
}
