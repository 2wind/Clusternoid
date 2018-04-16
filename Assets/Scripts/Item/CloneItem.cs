using UnityEngine;
using System.Collections;
using System;
using Clusternoid;

public class CloneItem : Item
{
    public override bool Action(GameObject other)
    {
        var spawnPosition = Clusternoid.Math.RandomOffsetPosition(other.transform.position, 0.1f);
        var contact = Physics2D.OverlapCircleAll(spawnPosition, 0.4f, 1 << LayerMask.NameToLayer("Wall"));
        if (contact != null && contact.Length > 0)
        {
            foreach (var wall in contact)
            {
                spawnPosition += (spawnPosition - wall.transform.position) * 0.4f;
            }
        }
        PlayerController.groupCenter.GetComponent<PlayerController>().AddCharacter(spawnPosition);

        // TODO: 분명 이것보단 더 좋은 호출 방법이 있을 것. 리팩토링...
        other.GetComponent<SoundPlayer>().Play(SoundType.Player_Clone);
        return true;
    }
}