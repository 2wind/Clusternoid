using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public Transform firingPosition;

    GameObject weapon;// 일단 무기를 여기다 담고 있는다. 
    //장기적으로는 weapon.cs로 옮겨서 enum으로 불러오는 것을 지원해야 함(그래야 다양한 종류의 무기를 지원가능)



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack()
    {
        var bullet = Instantiate(GameManager.instance.bullet, firingPosition.position, firingPosition.rotation);
        //bullet.getGameObject<bullet>().initialize;
        //bullet.applyforce(앞쪽으로 정해진 속도로);

    }

    
}
