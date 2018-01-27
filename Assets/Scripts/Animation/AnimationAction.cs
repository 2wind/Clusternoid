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
            case AnimatorBehaviour.ActionType.Fire:
                root.GetComponent<Weapon>().SendMessage("Fire");
                break;
            case AnimatorBehaviour.ActionType.Rotate:
                rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, root.GetComponent<AI>().Rotation,
                    action.value * Time.deltaTime));
                //transform.rotation = Clusternoid.Math.RotationAngle(transform.position, PlayerController.groupCenter.transform.position);
                break;
            case AnimatorBehaviour.ActionType.MoveForward:
                rb.velocity = (Vector2.up * action.value);
                break;
            case AnimatorBehaviour.ActionType.MoveSideways:
                rb.velocity = (Vector2.right * action.value);
                break;
            case AnimatorBehaviour.ActionType.MoveRandom:
                rb.velocity = (root.GetComponent<AI>().direction * action.value);
                break;
            case AnimatorBehaviour.ActionType.Accelerate:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
            case AnimatorBehaviour.ActionType.Death:
                root.GetComponent<DropItem>()?.Drop();
                Destroy(root.gameObject, action.value);
                break;
            case AnimatorBehaviour.ActionType.TowardRandom:
                rb.rotation = Random.Range(0, 360f);
                break;
            case AnimatorBehaviour.ActionType.ChooseRotation:
                root.GetComponent<AI>().ChooseRotation();
                break;
            case AnimatorBehaviour.ActionType.ChooseDirection:
                root.GetComponent<AI>().ChooseDirection();
                break;
            case AnimatorBehaviour.ActionType.LookAt:
                var localPosition = transform.InverseTransformPoint(root.GetComponent<AI>().nearestCharacter.gameObject.transform.position);
                var angle = -(Mathf.Atan2(localPosition.x, localPosition.y) * Mathf.Rad2Deg);

                rb.rotation = ( angle * Time.deltaTime);
                //rb.rotation = Vector2.SignedAngle(rb.position, root.GetComponent<AI>().nearestCharacter.transform.position);
                //root.transform.LookAt(GetComponent<AI>().nearestCharacter.transform);
                break;
            case AnimatorBehaviour.ActionType.PathFind:
                root.GetComponent<Robot>().path = PathFinder.GetAcceleration(root.transform.position);
                break;
            case AnimatorBehaviour.ActionType.FindNearestCharacter:
                root.GetComponent<AI>().FindNearestCharacter();
                localPosition = transform.InverseTransformPoint(root.GetComponent<AI>().nearestCharacter.gameObject.transform.position);
                angle = -(Mathf.Atan2(localPosition.x, localPosition.y) * Mathf.Rad2Deg);
                root.GetComponent<AI>().Rotation = angle;
                break;
            case AnimatorBehaviour.ActionType.SetSuperArmor:
                root.GetComponent<Robot>().superArmor = action.value > 0 ? true : false;
                break;
            case AnimatorBehaviour.ActionType.MovePath:
                rb.velocity = root.GetComponent<Robot>().path * action.value;
                root.GetComponent<AI>().Rotation = -(Mathf.Atan2(root.GetComponent<Robot>().path.x, root.GetComponent<Robot>().path.y) * Mathf.Rad2Deg);
                break;
            case AnimatorBehaviour.ActionType.Stop:
                rb.velocity = Vector2.zero;
                break;
            default:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
        }
    }
}