using UnityEngine;
using System.Collections;

public class ItemListener : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other) => TriggerListener(other);

    public bool TriggerListener(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        var acted = false;
        if (item != null)
        {
            acted = item.Action(gameObject);
            other.gameObject.SetActive(false);//아이템 파괴
        }
        return acted;
    }

}
