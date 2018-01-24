using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationAction : MonoBehaviour
{
    GameObject root;

    // Use this for initialization
    void Start()
    {
        SetAnimatorAction();
    }

    void SetAnimatorAction()
    {
        root = GetComponentInParent<Rigidbody2D>().gameObject;
        var anim = GetComponent<Animator>();
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
            case AnimatorBehaviour.ActionType.Fire:
                root.GetComponent<Weapon>()?.SendMessage("Fire");
                break;
            case AnimatorBehaviour.ActionType.Rotate:
                root.GetComponent<Rigidbody2D>()?.AddTorque(GetComponent<AI>().Rotation * 1f);
                //transform.rotation = Clusternoid.Math.RotationAngle(transform.position, PlayerController.groupCenter.transform.position);
                break;
            case AnimatorBehaviour.ActionType.MoveForward:
                root.GetComponent<Rigidbody2D>()?.AddRelativeForce(Vector2.up * action.value);
                break;
            case AnimatorBehaviour.ActionType.MoveSideways:
                root.GetComponent<Rigidbody2D>()?.AddRelativeForce(Vector2.right * action.value);
                break;
            case AnimatorBehaviour.ActionType.MoveRandom:
                GetComponent<Robot>()?.MoveRandomDirection(action.value, 0.7f);
                break;
            case AnimatorBehaviour.ActionType.Accelerate:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
            case AnimatorBehaviour.ActionType.Death:
                root.GetComponent<DropItem>()?.Drop();
                Destroy(root.gameObject, action.value);
                break;
            case AnimatorBehaviour.ActionType.TowardRandom:
                root.GetComponent<Rigidbody2D>().rotation = Random.Range(0, 360f);
                break;
            case AnimatorBehaviour.ActionType.ChooseRotation:
                root.GetComponent<AI>()?.ChooseRotation();
                break;
            case AnimatorBehaviour.ActionType.ChooseDirection:
                root.GetComponent<AI>()?.ChooseDirection();
                break;
            case AnimatorBehaviour.ActionType.LookAt:
                root.transform.LookAt(GetComponent<AI>().nearestCharacter.transform);
                break;
            case AnimatorBehaviour.ActionType.PathFind:
                // 위에서 저장된 character로 Pathfind를 실시한다.
                break;
            case AnimatorBehaviour.ActionType.FindNearestCharacter:
                root.GetComponent<AI>()?.FindNearestCharacter();
                break;
            default:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
        }
    }
}