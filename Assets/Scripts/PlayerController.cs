using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Clusternoid;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> characters; // 플레이어가 조종하는 복제인간들이 들어있는 리스트
    public static GameObject groupCenter; // 바로 이거.
    public GameObject characterModel; // 복제할 붕어빵
    public float distance; // 인싸와 아싸를 결정하는 붕어빵 사이의 기본 거리

    public GameObject centerOfGravityCharacter; // 중력의 중심점이 될 캐릭터;

    Rigidbody2D playerRigidbody; // Reference to the player's rigidbody. 
    Plane xyPlane;
    int floorMask; // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 1000f; // The length of the ray from the camera into the scene.


    // Use this for initialization
    void Awake()
    {
        groupCenter = this.gameObject;
        xyPlane = new Plane(Vector3.forward, Vector3.zero);
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        // Set up references.
        // anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        centerOfGravityCharacter = AddCharacter();
        StartCoroutine(nameof(DoInsiderCheck));
    }

    void Start()
    {
        PathFinder.instance.target = transform;
    }

    Vector2 CenterOfGravity()
    {
        if (characters.Count == 0)
        {
            return new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        }
        else
        {
            var insiderCharacters = characters.Where(character => character.GetComponent<CharacterManager>().isInsider)
                .ToList();
            return new Vector2(
                insiderCharacters.Select(character => character.transform.position.x).Average(),
                insiderCharacters.Select(character => character.transform.position.y).Average()
            );
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Position `groupCenter` at the average position of the insider characters.
        var centerOfGravity = CenterOfGravity();
        groupCenter.transform.position = new Vector3(
            centerOfGravity.x, centerOfGravity.y, groupCenter.transform.position.z
        );


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

        // Add Repulsion and Attraction
        var characterManagers = characters.Select(x => x.GetComponent<CharacterManager>()).ToList();
        foreach (var character in characterManagers)
        {
            foreach (var otherCharacter in characterManagers.Where(c => c != character))
            {
                if (!character.IsRepulsing(otherCharacter))
                {
                    continue;
                }

                var distanceVector = otherCharacter.transform.position - character.transform.position;
                otherCharacter.AddForce(character.repulsionIntensity * distanceVector);
            }
            Vector3 attractDirection = centerOfGravityCharacter.transform.position - character.transform.position;
            character.AddForce(attractDirection.normalized * Time.fixedDeltaTime * character.repulsionIntensity);
        }

        
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

    //public List<Vector2> CalculatePlacement()
    //{
    //    List<Vector2> placement = new List<Vector2>();
    //    //여기서 각 캐릭터가 가야 하는 위치들을 계산
    //    float height = distance * (characters.Count / 5);
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        placement.Add(new Vector2(groupCenter.transform.position.x + (i % 5 - 2) * distance,
    //            groupCenter.transform.position.y - height / 2 + (i / 5) * distance));
    //    }
    //    return placement;
    //}

    GameObject AddCharacter()
    {
        // Place the character slightly next to groupCenter.
        var position = Clusternoid.Math.RandomOffsetPosition(transform.position, 0.1f);
        return AddCharacter(position);
        //TryMovingCharacters();
        //instantiate(투명하게)
        //add to characters
        //anim
    }

    public GameObject AddCharacter(Vector3 position)
    {
        var newCharacter = Instantiate(characterModel, position, transform.rotation);
        characters.Add(newCharacter);
        return newCharacter;
    }

    void resetCenterOfGravityCharacter()
    {
        var centerOfGravity = CenterOfGravity();
        centerOfGravityCharacter = characters.OrderBy(
            character => ((Vector2) character.transform.position - centerOfGravity).sqrMagnitude
        ).First();
    }


    void RemoveCharacter(GameObject character)
    {
        if (characters.Count > 1 && centerOfGravityCharacter.Equals(character))
        {
            characters.Remove(character);
            resetCenterOfGravityCharacter();
        }
        else
        {
            characters.Remove(character);
        }
        character.SendMessage("KillCharacter");

    }

    void RemoveLastCharacter()
    {
        if (characters.Count() > 0)
        {
            RemoveCharacter(characters.Last());
        }
    }

    /// <summary>
    /// insider인지 체크하는 함수
    /// </summary>
    /// 0. 모두 (isInsider =  false)
    /// 0.1. isCenterOfGravity == true인 item부터 시작한다. item.isInsider = true;
    /// 1. item과 insiderDistance 이내인 친구들을 모두 선택(콜라이더 이용)
    /// 2. 그 친구들에 대해 모두 isInsider = true;
    /// 3. 재귀적으로 그 친구들에게 InsiderCheck() 수행
    /// 4. 더 이상 방문할 친구들이 없으면 끝
    /// 코루틴으로 빼도록 하자.
    IEnumerator DoInsiderCheck()
    {
        while (characters.Count > 0)
        {
            resetCenterOfGravityCharacter();
            InsiderCheck();
            yield return new WaitForSeconds(.1f);
        }
        StopCoroutine("DoInsiderCheck");
    }

    void InsiderCheck()
    {
        foreach (var item in characters)
        {
            item.GetComponent<CharacterManager>().isInsider = false;
        }
        var temp = centerOfGravityCharacter;
        InsiderCheckRecursive(temp, characters);
    }

    void InsiderCheckRecursive(GameObject vertex, List<GameObject> list)
    {
        vertex.GetComponent<CharacterManager>().isInsider = true;
        foreach (var item in list)
        {
            if (!item.GetComponent<CharacterManager>().isInsider
                && Vector3.Distance(vertex.transform.position, item.transform.position) < distance)
            {
                InsiderCheckRecursive(item, list);
            }
        }
    }

    public GameObject FindNearestCharacter(Vector3 from)
    {
        GameObject nearest = gameObject;
        float distance = Vector3.Distance(transform.position, from);
        foreach (var ch in characters) 
        {
            var curr = Vector3.Distance(ch.transform.position, from);
            if (curr < distance)
            {
                nearest = ch;
                distance = curr;
            }
        }
        return nearest;
    }
    
    public float FindNearestDistance(Vector3 from)
    {
        GameObject nearest = gameObject;
        float distance = Vector3.Distance(transform.position, from);
        foreach (var ch in characters)
        {
            var curr = Vector3.Distance(ch.transform.position, from);
            if (curr < distance)
            {
                nearest = ch;
                distance = curr;
            }
        }
        return distance;
    }
}