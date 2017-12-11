using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public List<GameObject> characters; // 플레이어가 조종하는 복제인간들이 들어있는 리스트
    GameObject player; //플레이어가 wasd로 움직이는 투명한 무언가
    public GameObject character; // 복제할 붕어빵
    public float distance; // 붕어빵 사이의 기본 거리

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    public float speed = 6f;            // The speed that the player will move at.
    Rigidbody2D playerRigidbody;          // Reference to the player's rigidbody. 
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.


    // Use this for initialization
    void Awake () {
        player = this.gameObject;
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor") ;

        // Set up references.
        // anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        // Store the input axes.
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");
        // TODO: 키보드 입력의 경우 int처럼 빠릿빠릿해야 하고(-1 -> 0 -> 1) 컨트롤러 입력의 경우 적절한 입력 곡선을 가져야 함

        // Move the player around the scene.
        Move(h, v);
        // Turn the player to face the mouse cursor.
        Turning();

        TryMovingCharacters();



        if (Input.GetKeyDown(KeyCode.E))
        {
            AddCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            RemoveLastCharacter();
        }

        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
	}

    void Attack()
    {
        foreach (var item in characters)
        {
            //각 item의 charactercontroller의 Attack()을 호출하면
            //Attack()은 각 캐릭터마다 가지고 있는 무기로 공격을 한다
        }
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, v, 0f);
        
        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector2 playerToMouse = floorHit.point - transform.position;
            playerToMouse.Normalize();
            float rot_z = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            // FIXME: 정상적으로 바라보도록 수정

        }
    }



    public void TryMovingCharacters()
    {
        // characters.각각에 대해.calculatePlacement에 따라 계산된 좌표로 이동을 시도. 
        // Pathfinding을 이용하면 좋을 것 같다
        // 지금은 순간이동을 시키자
        var posList = CalculatePlacement();
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].transform.SetPositionAndRotation(posList[i].transform.position, posList[i].transform.rotation);
            // TODO: 여기서 setposandrotation 대신 각각의 characters를 posList로 pathfinding을 통해 이동하도록 해야 한다.
        }
        
    }

    public List<Transform> CalculatePlacement()
    {
        List<Transform> placement = new List<Transform>(characters.Select(go => go.transform)); 
        //여기서 각 캐릭터가 가야 하는 위치들을 계산
        for (int i = 0; i < placement.Count; i++)
        {
            float height = distance * (placement.Count / 5);
            placement[i].SetPositionAndRotation(new Vector3(player.transform.position.x + (i % 5 - 2) * distance, player.transform.position.y - height / 2  + (i / 5) * distance), player.transform.rotation);
            placement[i].Rotate(new Vector3(-90, 0, 0));
        }
        return placement;
    }

    void AddCharacter()
    {
        var newCharacter = Instantiate(character);
        characters.Add(newCharacter);
        TryMovingCharacters();
        //instantiate(투명하게)
        //add to characters
        //anim
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

        RemoveCharacter(characters.Last());

    }
}
