using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class WallJumpLeft : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
        {
            this.transform.parent.GetComponentInParent<MallowMovement>().wallTouchLeft = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
        {
            this.transform.parent.GetComponentInParent<MallowMovement>().wallTouchLeft = false;
        }
    }
}
