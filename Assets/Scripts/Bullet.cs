﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public string firedFrom =  "Player";// 누가 쐈는가?
    public int damage = 3;

    //캐릭터가 쐈으면 캐릭터는 통과, 적에는 히트 판정&대미지.
    //적이 쐈으면 캐릭터는 히트 판정, 적에는 ???(통과? 히트 판정? 대미지까지?)
    // 공통으로 지형지물은 부딛힘(나중에 지형무시 총알이 나올 떄 다시 생각해보자)
    // 언젠가는 enum으로 넘어가야 한다.


    public void Initialize()
    {
        // 아직은 아무것도 안하지만
        // firedFrom이나 대미지 등을 어딘가에서 불러와서?
        // 결정해준다.

    }

    // OnCollisionEnter2D는 이 collider2D/rigidbody2D가 다른 rigidbody2D/collider2D에 접촉되기 시작하면 호출됩니다(2D 물리학에만 해당)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 일단 플레이어만 쏜다고 가정하고 만들어보자. 
        if (collision.gameObject.CompareTag("Player"))
        {
            // do nothing just pass
            return;
        }
        else if (collision.gameObject.CompareTag("enemy"))
        {
            //do some damage
            //collision.gameObject.

        }
        //and destroy itself
        //with animation hopefully..
        //여기에 충돌 애니메이션을 넣으시오
        Destroy(this);
    }


}
