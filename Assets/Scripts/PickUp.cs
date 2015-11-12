using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    private bool hasOwner = false;
    private string newOwner;
    private Vector3 itemMovement = Vector3.zero;
    public float armReach = 3.5f;
    private bool thrown = false;
    private bool playerFacingRight;
    public float throwSpeed = 3f;
    public float fallVelocity = 0f;
    public float gravity = 1f;

    void FixedUpdate()
    {
        Collider collider = GetComponent<SphereCollider>();

        if (thrown == true)
        {
            itemMovement.x = itemMovement.x + throwSpeed;
            collider.transform.position = itemMovement;
            fallVelocity += (gravity/50);
            itemMovement.y -= fallVelocity * Time.deltaTime;

        }
    }

    public void moveWithOwner(bool right)
    {
        Collider collider = GetComponent<SphereCollider>();

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
            collider.transform.position = itemMovement;
        }
	}

    public void getThrown(bool right)
    {
        Collider collider = GetComponent<SphereCollider>();
        hasOwner = false;
        thrown = true;
        collider.isTrigger = false;
        playerFacingRight = right;
        if (right == true)
            throwSpeed = Mathf.Abs(throwSpeed);
        else
            throwSpeed = Mathf.Abs(throwSpeed) * (-1);
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
