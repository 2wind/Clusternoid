using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public List<GameObject> characters; // 플레이어가 조종하는 복제인간들이 들어있는 리스트
    GameObject player; //플레이어가 wasd로 움직이는 투명한 무언가
    public GameObject characterModel; // 복제할 붕어빵
    public float distance; // 붕어빵 사이의 기본 거리
    

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    public float speed = 6f;            // The speed that the player will move at.
    Rigidbody2D playerRigidbody;          // Reference to the player's rigidbody. 
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 1000f;          // The length of the ray from the camera into the scene.


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


        TryMovingCharacters();



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
        // Store the input axes.
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");
        // TODO: 키보드 입력의 경우 int처럼 빠릿빠릿해야 하고(-1 -> 0 -> 1) 컨트롤러 입력의 경우 적절한 입력 곡선을 가져야 함

        // Move the player around the scene.
        Move(h, v);
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

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, v, 0f);
        
        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        transform.Translate(movement, Space.World);
    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            transform.rotation = GameManager.RotationAngle(transform.position, floorHit.point);
        }
    }



    public void TryMovingCharacters()
    {
        // characters.각각에 대해.calculatePlacement에 따라 계산된 좌표로 이동을 시도. 
        // Pathfinding을 이용하면 좋을 것 같다
        var posList = CalculatePlacement();
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].SendMessage("MoveTo", posList[i]);
            // TODO: 여기서 setposandrotation 대신 각각의 characters를 posList로 pathfinding을 통해 이동하도록 해야 한다.

        }

    }

    public List<Vector2> CalculatePlacement()
    {
        List<Vector2> placement = new List<Vector2>();
        //여기서 각 캐릭터가 가야 하는 위치들을 계산
        float height = distance * (characters.Count / 5);
        for (int i = 0; i < characters.Count; i++)
        {
            placement.Add(new Vector2(player.transform.position.x + (i % 5 - 2) * distance, player.transform.position.y - height / 2 + (i / 5) * distance));
        }
        return placement;
    }

    void AddCharacter()
    {
        var newCharacter = Instantiate(characterModel, transform.position, transform.rotation);
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
        if(characters.Count() > 0) { RemoveCharacter(characters.Last()); }
        
    }
}
