using System;
using System.Linq;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationAction : MonoBehaviour
{
    GameObject root;
    Rigidbody2D rb;
    Animator anim;

    // Use this for initialization
    void OnEnable()
    {
        SetAnimatorAction();
    }

    void SetAnimatorAction()
    {
        root = GetComponentInParent<Rigidbody2D>().gameObject;
        anim = GetComponent<Animator>();
        rb = root.GetComponent<Rigidbody2D>();
        var behaviours = anim.GetBehaviours<AnimatorBehaviour>();
        foreach (var behaviour in behaviours)
        {
            behaviour.action = this;
        }
    }

    public void PlayAction(AnimatorBehaviour.Action action)
    {
        switch (action.type)
        {
            /// 사격한다.
            case AnimatorBehaviour.ActionType.Fire:
                root.GetComponent<Weapon>()?.Fire();
                break;
            /// 초당 action.value도(degree) 만큼 정해진 방향으로 회전한다.
            case AnimatorBehaviour.ActionType.Rotate:
                rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, root.GetComponent<AI>().Rotation,
                    action.value * Time.deltaTime));
                //transform.rotation = Clusternoid.Math.RotationAngle(transform.position, PlayerController.groupCenter.transform.position);
                break;
            /// 속도를 action.value로 해 직진한다.
            case AnimatorBehaviour.ActionType.MoveForward:
                rb.velocity = (root.transform.up * action.value);
                break;
            /// 속도를 action.value로 해 왼쪽으로 직선 이동한다.
            case AnimatorBehaviour.ActionType.MoveSideways:
                rb.velocity = (root.transform.right * action.value);
                break;
            /// 속도를 action.value로 해 ChooseDirection에서 정한 무작위 방향으로 직선 이동한다.
            case AnimatorBehaviour.ActionType.MoveRandom:
                rb.velocity = (root.GetComponent<AI>().direction * action.value);
                break;
            /// 속도를 action.value까지 action.key초동안 linear하게 변화시킨다.
            case AnimatorBehaviour.ActionType.Accelerate:
                rb.velocity = transform.up * Mathf.MoveTowards(rb.velocity.magnitude, action.value,
                                  Int32.Parse(action.key) * Time.deltaTime);
                break;
            /// 캐릭터를 사망 처리한다.
            case AnimatorBehaviour.ActionType.Death:
                root.GetComponent<DropItem>()?.Drop();
                //Destroy(root.gameObject, action.value);
                if (root.GetComponent<Character>() != null)
                    anim.Update(Time.deltaTime);
                root.SetActive(false);
                break;
            /// 캐릭터를 무작위 방향으로 바라보게 한다.
            case AnimatorBehaviour.ActionType.TowardRandom:
                rb.rotation = UnityEngine.Random.Range(0, 360f);
                break;
            /// 임의 회전 방향을 정한다.
            case AnimatorBehaviour.ActionType.ChooseRotation:
                root.GetComponent<AI>().ChooseRotation();
                break;
            /// 임의 이동 방향을 정한다.
            case AnimatorBehaviour.ActionType.ChooseDirection:
                root.GetComponent<AI>().ChooseDirection();
                break;
            /// 가장 가까운 캐릭터 방향으로 바라본다. 이전에 FindNearestCharacter를 호출해야 한다.
            case AnimatorBehaviour.ActionType.LookAt:
                rb.MoveRotation(Clusternoid.Math.RotationAngleFloat(rb.position,
                    root.GetComponent<AI>().nearestCharacter.gameObject.transform.position));
                break;
            /// 플레이어 무리 방향으로 이동하는 길을 찾는다.
            case AnimatorBehaviour.ActionType.PathFind:
                root.GetComponent<MovingAI>().path = PathFinder.GetAcceleration(root.transform.position);
                break;
            /// 가장 가까운 플레이어 캐릭터를 찾고, 그쪽 방향으로 회전을 정한다(Rotate를 이용하면 자연스럽게 그 캐릭터를 바라보게 된다)
            case AnimatorBehaviour.ActionType.FindNearestCharacter:
                if (PlayerController.groupCenter.characters.Count > 0)
                {
                    root.GetComponent<AI>().FindNearestCharacter();
                    root.GetComponent<AI>().Rotation = Clusternoid.Math.RotationAngleFloat(rb.position,
                        root.GetComponent<AI>().nearestCharacter.gameObject.transform.position);
                }
                break;
            /// action.value가 0보다 크면 슈퍼아머를 설정하고, 그렇지 않으면 해제한다.
            case AnimatorBehaviour.ActionType.SetSuperArmor:
                root.GetComponent<MovingAI>().superArmor = action.value > 0 ? true : false;

                break;
            /// 위에서 PathFind로 찾은 방향으로 action.value 속도로 이동한다. 이동하는 방향으로 회전을 정한다.(Rotate를 이용하면 
            /// 자연스럽게 앞 방향을 바라보게 된다.)
            case AnimatorBehaviour.ActionType.MovePath:
                rb.velocity = root.GetComponent<MovingAI>().path * action.value;
                root.GetComponent<AI>().Rotation =
                    Clusternoid.Math.RotationAngleFloat(Vector2.zero, root.GetComponent<MovingAI>().path);
                break;
            /// 캐릭터를 제자리에 멈춘다.
            case AnimatorBehaviour.ActionType.Stop:
                rb.velocity = Vector2.zero;
                break;
            /// 캐릭터가 Rotation의 방향과 action.value의 속도를 최대로 하여 계속 돌도록 한다.
            case AnimatorBehaviour.ActionType.KeepRotating:
                rb.MoveRotation(rb.rotation +
                                Mathf.Clamp(root.GetComponent<AI>().Rotation, -action.value, action.value) *
                                Time.deltaTime);
                break;
            /// action.value가 0보다 크면 charging을 true로 하고, 그렇지 않으면 해제한다.
            case AnimatorBehaviour.ActionType.SetCharging:
                root.GetComponent<RushDrone>().isCharging = action.value > 0 ? true : false;
                break;
            /// action.value만큼 피해를 준다. action.key가 있다면 그만큼 넉백을 준다.(는 미구현)
            case AnimatorBehaviour.ActionType.Damage:
                var attack = new Attack(root.tag.GetHashCode(), Mathf.RoundToInt(action.value), Vector3.zero, 0, 0);
                root.GetComponent<Health>()?.GetAttack(attack);
                break;
            /// Melee 콜라이더를 설정한다. action.value가 0보다 크면 활성화하고, 0보다 작으면 비활성화한다.
            case AnimatorBehaviour.ActionType.Melee:
                bool isActive = action.value > 0 ? true : false;
                root.GetComponentInChildren<Melee>()?.SetActive(isActive);
                break;
            /// action.key 이름의 메소드를 호출한다. string형 변수는 뒤에 ","를 이용해 추가할 수 있다.
            case AnimatorBehaviour.ActionType.SendMessage:
                var args = action.key.Split(',').ToList();
                if (args.Count > 1)
                {
                    root.SendMessage(args[0], args.GetRange(1, args.Count - 1));
                }
                else
                {
                    root.SendMessage(args[0]);
                }
                break;
            case AnimatorBehaviour.ActionType.PlayLoop:
                root.GetComponent<SoundPlayer>()?.Play(action.key, true);
                break;
            case AnimatorBehaviour.ActionType.PlayOneShot:
                root.GetComponent<SoundPlayer>().Play(action.key);
                break;
            case AnimatorBehaviour.ActionType.StopSound:
                root.GetComponent<AudioSource>().Stop();
                break;
            default:
                if (Debug.isDebugBuild)
                    Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
        }
    }
}