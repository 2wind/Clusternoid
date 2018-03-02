using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPool : ObjectPool<UIPool>
{

 
    protected override string Path()
        => "UI/";



}
