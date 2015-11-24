using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class WallJump : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
        {
            this.transform.parent.GetComponentInParent<MallowMovement>().wallTouch = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
        {
            this.transform.parent.GetComponentInParent<MallowMovement>().wallTouch = false;
        }
    }
}
