using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class PickUp : MonoBehaviour {

    private bool hasOwner = false;
    private string newOwner;
    private Vector3 itemMovement = Vector3.zero;
    public float armReach = 3.5f;
    private bool playerFacingRight;
    public float throwSpeed = 3f;

    public void moveWithOwner(bool right)
    {
        Collider collider = GetComponent<SphereCollider>();
        Rigidbody rBody = GetComponent<Rigidbody>();

        //Which way is the player facing?  This will alter the position of the item.
        if (right == true)
            armReach = Mathf.Abs(armReach);
        else
            armReach = Mathf.Abs(armReach) * (-1);

        //Move this item in respect to player's current position.
        if (hasOwner == true)
        {
            itemMovement = GameObject.Find(newOwner).transform.position;
            itemMovement.x = itemMovement.x + armReach;
            itemMovement.z = 0f;
            rBody.freezeRotation = true;
            
            collider.transform.position = itemMovement;
        }
	}

    public void getThrown(bool right)
    {
        Collider collider = GetComponent<SphereCollider>();
        Rigidbody rBody = GetComponent<Rigidbody>();
        hasOwner = false;
        collider.isTrigger = false;
        playerFacingRight = right;
        if (right == true)
            throwSpeed = Mathf.Abs(throwSpeed);
        else
            throwSpeed = Mathf.Abs(throwSpeed) * (-1);
        rBody.useGravity = true;
        rBody.AddForce(throwSpeed,0f,0f);
        rBody.freezeRotation = false;
    }

    void OnTriggerStay(Collider otherCollider)
    {
        Rigidbody rBody = GetComponent<Rigidbody>();
        Collider collider = GetComponent<SphereCollider>();

        //Detect whether or not a player is range to pick up this item.
        if (otherCollider.tag == "Player")
        {
            if (otherCollider.gameObject.GetComponent<Movement>().pickUpItem == true)
            {
                collider.isTrigger = true;
                hasOwner = true;
                newOwner = otherCollider.gameObject.name;
                rBody.useGravity = false;
            }
        }
    }
}
