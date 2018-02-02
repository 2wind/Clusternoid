using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationAction : MonoBehaviour
{
    GameObject root;
    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        SetAnimatorAction();
    }

    void SetAnimatorAction()
    {
        root = GetComponentInParent<Rigidbody2D>().gameObject;
        var anim = GetComponent<Animator>();
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
                root.GetComponent<Weapon>().Fire();
                break;
            /// 초당 action.value도(degree) 만큼 정해진 방향으로 회전한다.
            case AnimatorBehaviour.ActionType.Rotate:
                rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, root.GetComponent<AI>().Rotation,
                    action.value * Time.deltaTime));
                //transform.rotation = Clusternoid.Math.RotationAngle(transform.position, PlayerController.groupCenter.transform.position);
                break;
            /// 속도를 action.value로 해 직진한다.
            case AnimatorBehaviour.ActionType.MoveForward:
                rb.velocity = (Vector2.up * action.value);
                break;
            /// 속도를 action.value로 해 왼쪽으로 직선 이동한다.
            case AnimatorBehaviour.ActionType.MoveSideways:
                rb.velocity = (Vector2.right * action.value);
                break;
            /// 속도를 action.value로 해 ChooseDirection에서 정한 무작위 방향으로 직선 이동한다.
            case AnimatorBehaviour.ActionType.MoveRandom:
                rb.velocity = (root.GetComponent<AI>().direction * action.value);
                break;
            /// 
            case AnimatorBehaviour.ActionType.Accelerate:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
            /// 캐릭터를 사망 처리한다.
            case AnimatorBehaviour.ActionType.Death:
                root.GetComponent<DropItem>()?.Drop();
                Destroy(root.gameObject, action.value);
                break;
            /// 캐릭터를 무작위 방향으로 바라보게 한다.
            case AnimatorBehaviour.ActionType.TowardRandom:
                rb.rotation = Random.Range(0, 360f);
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
                rb.MoveRotation(Clusternoid.Math.RotationAngleFloat(rb.position, root.GetComponent<AI>().nearestCharacter.gameObject.transform.position));
                break;
            /// 플레이어 무리 방향으로 이동하는 길을 찾는다.
            case AnimatorBehaviour.ActionType.PathFind:
                root.GetComponent<Robot>().path = PathFinder.GetAcceleration(root.transform.position);
                break;
            /// 가장 가까운 플레이어 캐릭터를 찾고, 그쪽 방향으로 회전을 정한다(Rotate를 이용하면 자연스럽게 그 캐릭터를 바라보게 된다)
            case AnimatorBehaviour.ActionType.FindNearestCharacter:
                root.GetComponent<AI>().FindNearestCharacter();
                root.GetComponent<AI>().Rotation = Clusternoid.Math.RotationAngleFloat(rb.position, root.GetComponent<AI>().nearestCharacter.gameObject.transform.position);
                break;
            /// action.value가 0보다 크면 슈퍼아머를 설정하고, 그렇지 않으면 해제한다.
            case AnimatorBehaviour.ActionType.SetSuperArmor:
                root.GetComponent<Robot>().superArmor = action.value > 0 ? true : false;
                break;
            /// 위에서 PathFind로 찾은 방향으로 action.value 속도로 이동한다. 이동하는 방향으로 회전을 정한다.(Rotate를 이용하면 
            /// 자연스럽게 앞 방향을 바라보게 된다.)
            case AnimatorBehaviour.ActionType.MovePath:
                rb.velocity = root.GetComponent<Robot>().path * action.value;
                root.GetComponent<AI>().Rotation = Clusternoid.Math.RotationAngleFloat(Vector2.zero, root.GetComponent<Robot>().path);
                break;
            /// 캐릭터를 제자리에 멈춘다.
            case AnimatorBehaviour.ActionType.Stop:
                rb.velocity = Vector2.zero;
                break;
            default:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
        }
    }
}