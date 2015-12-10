using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class WallJumpRight : MonoBehaviour
{

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
        {
            this.transform.parent.GetComponentInParent<MallowMovement>().wallTouchRight = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
        {
            this.transform.parent.GetComponentInParent<MallowMovement>().wallTouchRight = false;
        }
    }
}