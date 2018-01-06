using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public GameObject bullet;
    public GameObject healthPack; 
    // TODO: ObjectPool.cs를 구현해서 거기에서 들고 있고(BulletPool.cs? 등?)
    // Instantiate() 하던 부분을 모두 Get()을 활용하도록 바꾸기


}
