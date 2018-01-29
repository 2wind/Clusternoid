using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : ObjectPool
{
    protected override string Path()
    => "Bullets/";
}
