using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class PickUp : MonoBehaviour {

    private Rigidbody rBody;
    private Collider itemCollider;

    private Vector3 itemMovement = Vector3.zero;

    private bool hitPlayer = false;
    private bool playerFacingRight;
    public bool hasOwner = false;
    public bool thrown = false;
  
    public float armReach = 3.5f;
    public float pickUpSpeed;
    public float throwSpeed = 3f;
    
    public string newOwner;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        itemCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        if (((thrown) && (rBody.velocity.magnitude < pickUpSpeed)) || (hitPlayer))
        {
            thrown = false;
            hitPlayer = false;
            gameObject.layer = 8; //'8' is the hidden object layer
        }
    }

    public void moveWithOwner(bool right)
    {
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
            
            itemCollider.transform.position = itemMovement;
        }
	}

    public void getThrown(bool right)
    {
        hasOwner = false;
        itemCollider.isTrigger = false;
        playerFacingRight = right;
        if (right == true)
            throwSpeed = Mathf.Abs(throwSpeed);
        else
            throwSpeed = Mathf.Abs(throwSpeed) * (-1);
        rBody.AddForce(throwSpeed,0f,0f);
        rBody.freezeRotation = false;
        thrown = true;
        gameObject.layer = 0; //'0' is the default layer
    }

    void OnCollisionStay(Collision otherCollider)
    {
        //Detect whether or not a player got hit by this item
        if (otherCollider.collider.tag == "Player")
        {
            hitPlayer = true;
        }
    }
}
