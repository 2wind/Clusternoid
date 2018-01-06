using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public List<GameObject> characters; // 플레이어가 조종하는 복제인간들이 들어있는 리스트
    public static GameObject groupCenter; // 바로 이거.
    public GameObject characterModel; // 복제할 붕어빵
    public float distance; // 붕어빵 사이의 기본 거리
    

    Rigidbody2D playerRigidbody;          // Reference to the player's rigidbody. 
    Plane xyPlane;
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 1000f;          // The length of the ray from the camera into the scene.


    // Use this for initialization
    void Awake () {
        groupCenter = this.gameObject;
        xyPlane = new Plane(Vector3.forward, Vector3.zero);
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor") ;

        // Set up references.
        // anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        AddCharacter();
    }
	
	// Update is called once per frame
	void Update () {


       // TryMovingCharacters();



        if (Input.GetKeyDown(KeyCode.E))
        {
            AddCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            RemoveLastCharacter();
        }


        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
	}

    private void FixedUpdate()
    {

        // Turn the player to face the mouse cursor.
        Turning();

    }

    void Attack()
    {
        foreach (var item in characters)
        {
            //각 item의 characterManager의 Attack()을 호출하면
            //Attack()은 각 캐릭터마다 가지고 있는 무기로 공격을 한다
            //sendmessage()가 더 나으려나()
            item.SendMessage("Attack");
        }
    }



    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (xyPlane.Raycast(camRay, out distance))
        {
            transform.rotation = Clusternoid.Math.RotationAngle(transform.position, camRay.GetPoint(distance));
        }
        
    }

    
    //public void TryMovingCharacters()
    //{
    //    // characters.각각에 대해.calculatePlacement에 따라 계산된 좌표로 이동을 시도. 
    //    // Pathfinding을 이용하면 좋을 것 같다
    //    var posList = CalculatePlacement();
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        characters[i].SendMessage("MoveTo", posList[i]);
    //        // TODO: 여기서 setposandrotation 대신 각각의 characters를 posList로 pathfinding을 통해 이동하도록 해야 한다.

    //    }

    //}

    public List<Vector2> CalculatePlacement()
    {
        List<Vector2> placement = new List<Vector2>();
        //여기서 각 캐릭터가 가야 하는 위치들을 계산
        float height = distance * (characters.Count / 5);
        for (int i = 0; i < characters.Count; i++)
        {
            placement.Add(new Vector2(groupCenter.transform.position.x + (i % 5 - 2) * distance, groupCenter.transform.position.y - height / 2 + (i / 5) * distance));
        }
        return placement;
    }

    void AddCharacter()
    {
        var newCharacter = Instantiate(characterModel, transform.position, transform.rotation);
        characters.Add(newCharacter);
        //TryMovingCharacters();
        //instantiate(투명하게)
        //add to characters
        //anim
    }

    void AddCharacter(Vector3 position)
    {
        var newCharacter = Instantiate(characterModel, position, transform.rotation);
        characters.Add(newCharacter);
    }

    void RemoveCharacter(GameObject character)
    {
        //anim(투명하게 만들기)
        //remove from characters
        //Destroy
        if (characters.Count > 0)
        {
            characters.Remove(character);
            Destroy(character);
        }
    }

    void RemoveLastCharacter()
    {
        if(characters.Count() > 0) { RemoveCharacter(characters.Last()); }
        
    }
}
