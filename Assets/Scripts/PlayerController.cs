using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Math = Clusternoid.Math;

public class PlayerController : MonoBehaviour
{
    public List<Character> characters; // 플레이어가 조종하는 복제인간들이 들어있는 리스트
    public static PlayerController groupCenter; // 바로 이거.
    public GameObject characterModel; // 복제할 붕어빵
    public float maxDistance; // 인싸와 아싸를 결정하는 붕어빵 사이의 기본 거리

    public Character leader; // 중력의 중심점이 될 캐릭터;
    [NonSerialized] public Vector2 input;

    Plane xyPlane;
    Transform target;
    HashSet<Tuple<Character, Character>> charPairs;
    CharacterDistanceWorker distanceWorker;
    Dictionary<Character, List<Character>> distancePairs;

    public GameObject GameManager;

    public int emittingCount = 0;

    // Use this for initialization
    void Awake()
    {
        Clean();
        //Initialize();
        // Set up references.
        // anim = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(DistanceWorkCoroutine());
    }

    public void Clean()
    {
        while (characters.Any())
        {
            RemoveLastCharacter(true);
        }


        charPairs = new HashSet<Tuple<Character, Character>>();
        distanceWorker = new CharacterDistanceWorker();
        distancePairs = new Dictionary<Character, List<Character>>();
        //var targetGO = new GameObject("PathFinder Target");
        //target = targetGO.transform;
        var targetGO = transform.Find("PathFinder Target");
        target = targetGO.transform;
        target.SetParent(transform);
        groupCenter = this;
        xyPlane = new Plane(Vector3.forward, Vector3.zero);
        emittingCount = 0;
    }


    public void Initialize()
    {
        Vector2 startPosition = Vector2.zero;
        try
        {
            startPosition = GameObject.Find("StartPosition").transform.position;
        }
        catch (Exception)
        {
            if (Debug.isDebugBuild)
                Debug.LogError("Start Position not found!");
            throw;
        }
        if (!characters.Any())
        {
            leader = AddCharacter();
        }
        foreach (var item in characters)
        {
            Vector2 diff = item.transform.position - transform.position;
            var tempPosition = startPosition + diff;
            while (!PathFinder.IsInMap(tempPosition))
            {
                tempPosition = tempPosition + PathFinder.GetAcceleration(tempPosition);
            }
            item.transform.position = tempPosition;
        }
        distanceWorker = new CharacterDistanceWorker();
        distancePairs = new Dictionary<Character, List<Character>>();
        transform.position = startPosition;
        StopCoroutine(nameof(DoInsiderCheck));
        StartCoroutine(nameof(DoInsiderCheck));
        PathFinder.instance.target = target;
        ScoreBoard.instance.StartNewTracking();
    }

    Vector2 CenterOfGravity()
    {
        if (characters.Count == 0)
        {
            return new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        }
        var insiderCharacters = characters.Where(character => character.GetComponent<Character>().isInsider)
            .ToList();
        if (!insiderCharacters.Any()) return characters.First().transform.position;
        return new Vector2(
            insiderCharacters.Select(character => character.transform.position.x).Average(),
            insiderCharacters.Select(character => character.transform.position.y).Average()
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneLoader.instance.isMapLoading || !SceneLoader.instance.isLoadedSceneInGame
            || GameManager.GetComponent<PausePanel>().isOnPause
            || ScoreBoard.instance.isMapCleared)
        {
            GetComponent<SoundPlayer>().Stop();
            return;
        }

        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.E))
                AddCharacter();
            else if (Input.GetKeyDown(KeyCode.Q))
                RemoveLastCharacter();
        }

        if (characters.Count == 0) return;
        // Turn the player to face the mouse cursor.
        Turning();
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.magnitude > 0.1f)
        {
            GetComponent<SoundPlayer>().Play(SoundType.Player_Footstep, true);
        }
        else
        {
            GetComponent<SoundPlayer>().Stop();
        }
        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<SoundPlayer>().Play(SoundType.Player_Dash);
        }
        // Position `groupCenter` at the average position of the insider characters.
        var centerOfGravity = CenterOfGravity();
        if (characters.Any())
        {
            transform.position = (centerOfGravity * 2 + (Vector2) leader.transform.position) / 3;
            if (input.magnitude > 0.5f)
            {
                target.position = leader.transform.position;
            }
            else
            {
                target.position = centerOfGravity;
            }
            foreach (var item in characters)
            {
                //Debug.Log(emittingCount);
                item.GetComponent<Weapon>().firingPosition.GetComponent<SoundPlayer>()
                    .SetVolumeOverride(true, Mathf.Sqrt(emittingCount) / emittingCount);
            }
        }
    }

    IEnumerator DistanceWorkCoroutine()
    {
        while (enabled)
        {
            yield return StartCoroutine(distanceWorker.CalculateCharacterDistance(charPairs));
            distancePairs.Clear();
            if (distanceWorker.result == null) continue;
            foreach (var pair in distanceWorker.result)
            {
                distancePairs.Add(pair.Key, new List<Character>(pair.Value));
            }
        }
    }

    void FixedUpdate()
    {
        // FIXME: 수가 많아지면 심각하게 렉이 걸리므로 최적화 필요
        if (SceneLoader.instance.isMapLoading || !SceneLoader.instance.isLoadedSceneInGame) return;
        if (characters.Count == 0) return;
        InsiderCheck();
        AddRepulsions();
    }


    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (xyPlane.Raycast(camRay, out distance))
        {
            transform.rotation = Math.RotationAngle(transform.position, camRay.GetPoint(distance));
        }
    }

    Character AddCharacter()
    {
        // Place the character slightly next to groupCenter.
        var position = Math.RandomOffsetPosition(transform.position, 0.1f);
        return AddCharacter(position);
        //TryMovingCharacters();
        //instantiate(투명하게)
        //add to characters
        //anim
    }

    void AddRepulsions()
    {
        foreach (var pair in distancePairs)
        {
            var one = pair.Key;
            foreach (var other in pair.Value)
            {
                var t1 = one.transform.position;
                var t2 = other.transform.position;
                if (!one.IsRepulsing(other) && (!one.isInsider || !other.isInsider)) continue;
                var dist = Vector2.Distance(t1, t2);
                var repulsion = (1 - (dist / one.repulsionCollisionRadius)) * one.repulsionIntensity;
                one.repulsion += (Vector2) (t1 - t2).normalized * repulsion;
                other.repulsion += (Vector2) (t2 - t1).normalized * repulsion;
            }
        }
    }

    IEnumerable<Tuple<Character, Character>> GetPairs()
        => characters.Select((character, index) => new {character, index})
            .SelectMany(x => characters.Skip(x.index + 1),
                (x, y) => new Tuple<Character, Character>(x.character, y));

    public Character AddCharacter(Vector3 position)
    {
        //var newCharacter = Instantiate(characterModel, position, transform.rotation).GetComponent<Character>();
        //position.Set(position.x, position.y, 0.3f);
        var newCharacter = CharacterPool.Get("character").GetComponent<Character>();
        newCharacter.transform.SetPositionAndRotation(position, transform.rotation);

        //newCharacter.transform.SetParent(GameManager.transform);
        if (leader == null)
        {
            leader = newCharacter;
        }
        characters.Add(newCharacter);
        charPairs.Clear();
        charPairs.UnionWith(GetPairs());
        return newCharacter;
    }

    void ResetCenterOfGravityCharacter()
    {
        if (input.magnitude > 0.5f)
            leader = characters.Count(c => c.isInsider) == 1
                ? leader
                : GetCenterCharacter();
        else
        {
            var center = CenterOfGravity();
            leader = characters.Where(c => c.isInsider).OrderBy(character =>
                    Vector2.Distance(character.transform.position, center))
                .First();
        }
    }

    Character GetCenterCharacter()
    {
        if (!characters.Any(c => c.isInsider && IsInRange(c, leader)))
        {
            return characters.First(c => c.alive);
        }
        return characters.Where(c => c.isInsider && IsInRange(c, leader))
            .OrderByDescending(character => Vector2.Dot(character.transform.position, input))
            .First();
    }

    public void RemoveCharacter(Character character, bool silently = false)
    {
        if (characters.Count > 1 && leader.Equals(character))
        {
            characters.Remove(character);
            ResetCenterOfGravityCharacter();
        }
        else
        {
            characters.Remove(character);
        }
        charPairs.RemoveWhere(p => p.Item1 == character || p.Item2 == character);
        character.GetComponent<SoundPlayer>().SetPlayable(!silently);
        character.SendMessage("KillCharacter");
        if (!characters.Any() && SceneLoader.instance.isLoadedSceneInGame)
        {
            //GAMEOVER 게임오버 처리
            input = Vector2.zero;
            GetComponent<AudioSource>().Stop();
            if (!ScoreBoard.instance.isMapCleared)
            {
                ScoreBoard.instance.StopTracking();
                GameManager.GetComponent<GameOverPanel>().SetGameOverPanel(true);
            }
        }
    }

    void RemoveLastCharacter(bool silently = false)
    {
        if (characters.Any())
        {
            RemoveCharacter(characters.Last(), silently);
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
            ResetCenterOfGravityCharacter();
            yield return new WaitForFixedUpdate();
        }
        StopCoroutine(nameof(DoInsiderCheck));
    }

    void InsiderCheck()
    {
        foreach (var item in characters)
        {
            item.isInsider = false;
        }
        var first = leader;
        InsiderCheckRecursive(first, characters);
    }

    void InsiderCheckRecursive(Character vertex, List<Character> list)
    {
        vertex.isInsider = true;
        foreach (var item in list)
        {
            if (!item.isInsider
                && Vector3.Distance(vertex.transform.position, item.transform.position) < maxDistance)
            {
                InsiderCheckRecursive(item, list);
            }
        }
    }

    public Character FindNearestCharacter(Vector3 from)
    {
        Character nearest = leader;
        var distance = Vector3.Distance(leader.transform.position, from);
        foreach (var ch in characters)
        {
            var curr = Vector3.Distance(ch.transform.position, from);
            if (curr > distance) continue;
            nearest = ch;
            distance = curr;
        }
        return nearest;
    }

    public float FindNearestDistance(Vector3 from)
        => characters.Min(ch => Vector3.Distance(ch.transform.position, from));

    public Character FindClosestAngleCharacter(Vector3 from)
    {
        Character nearest = leader;
        var angle = Mathf.Abs(Math.RotationAngleFloat(from, leader.transform.position));
        foreach (var ch in characters)
        {
            var curr = Mathf.Abs(Math.RotationAngleFloat(from, ch.transform.position));
            if (curr > angle) continue;
            nearest = ch;
            angle = curr;
        }
        return nearest;
    }

    public float FindClosestAngle(Vector3 from)
        => characters.Min(ch => Mathf.Abs(Math.RotationAngleFloat(from, ch.transform.position)));

    public Character FindClosestAngleCharacterInRange(Vector3 from, float range)
    {
        Character nearest = leader;
        var angle = Mathf.Abs(Math.RotationAngleFloat(from, leader.transform.position));
        foreach (var ch in characters)
        {
            if (Vector3.Distance(ch.transform.position, from) < range)
            {
                var curr = Mathf.Abs(Math.RotationAngleFloat(from, ch.transform.position));
                if (curr > angle) continue;
                nearest = ch;
                angle = curr;
            }
        }
        if (Vector3.Distance(nearest.transform.position, from) < range)
        {
            return nearest;
        }
        else
        {
            return null;
        }
    }

    public float FindClosestAngleInRange(Vector3 from, float range)
        => characters.Where(ch => Vector3.Distance(ch.transform.position, from) < range)
            .Min(ch => Mathf.Abs(Math.RotationAngleFloat(from, ch.transform.position)));

    bool IsInRange(Character one, Character other)
        => Vector2.Distance(one.transform.position, other.transform.position) < maxDistance;
}