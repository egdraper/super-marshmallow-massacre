using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class GroundDetect : MonoBehaviour
{

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
        {
            this.transform.parent.GetComponentInParent<MallowMovement>().grounded = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
        {
            this.transform.parent.GetComponentInParent<MallowMovement>().grounded = false;
        }
    }
}
