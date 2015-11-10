using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    private bool hasOwner = false;
    private string newOwner;
    private Vector3 travelWithOwner = Vector3.zero;
    public float armReach = 3.5f;

    public void moveWithOwner(bool right)
    {
        Rigidbody rBody = GetComponent<Rigidbody>();

        //Which way is the player facing?  This will alter the position of the item.
        if (right == true)
            armReach = Mathf.Abs(armReach);
        else
            armReach = Mathf.Abs(armReach) * (-1);

        //Move this item in respect to player's current position.
        if (hasOwner == true)
        {
            travelWithOwner = GameObject.Find(newOwner).transform.position;
            travelWithOwner.x = travelWithOwner.x + armReach;
            rBody.transform.position = travelWithOwner;
        }
	}

    void OnTriggerStay(Collider collider)
    {
        //Detect whether or not a player is range to pick up this item.
        if (collider.tag == "Player")
        {
            if (collider.gameObject.GetComponent<P1Movement>().pickUpItem == true)
            {
                hasOwner = true;
                newOwner = collider.gameObject.name;
            }
        }
    }
}
