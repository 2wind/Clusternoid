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
                root.GetComponent<Weapon>().SendMessage("Fire");
                break;
            case AnimatorBehaviour.ActionType.Rotate:
                root.GetComponent<Rigidbody2D>()?.AddTorque(GetComponent<Turret>().Rotation * 1f);
                //transform.rotation = Clusternoid.Math.RotationAngle(transform.position, PlayerController.groupCenter.transform.position);
                break;
            case AnimatorBehaviour.ActionType.MoveForward:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
            case AnimatorBehaviour.ActionType.MoveSideways:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
            case AnimatorBehaviour.ActionType.MoveRandom:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
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
                root.GetComponent<Turret>()?.ChooseRotation();
                break;
            default:
                Debug.Log("Action not implemented!!" + " Action name: " + action.type);
                break;
        }
    }
}