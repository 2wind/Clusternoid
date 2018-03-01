using UnityEngine;
using System.Collections;
using System;
using Clusternoid;

public class CloneItem : Item
{


    public override bool Action(GameObject other)
    {
        PlayerController.groupCenter.GetComponent<PlayerController>().AddCharacter(
            Clusternoid.Math.RandomOffsetPosition(other.transform.position, 0.1f)
            );
        // TODO: 분명 이것보단 더 좋은 호출 방법이 있을 것. 리팩토링...
        other.GetComponent<SoundPlayer>().Play(SoundType.Player_Clone);
        return true;
    }

}
