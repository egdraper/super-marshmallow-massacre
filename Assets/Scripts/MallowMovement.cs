using UnityEngine;
using System.Collections;
using Assets.PlayerMovement;

namespace Assets.Scripts
{
    public class MallowMovement : MonoBehaviour
    {
        private Rigidbody rBody;
        private Collider playerCollider;

        private bool facingRight;
        private bool grounded;
        private bool jump;
        private bool pleaseWait = false;
        public bool gotHit;
        public bool pickUpItem;
        public bool thrown;
        public bool wallTouch = false;

        public float friction;
        public float jumpSpeed;
        public float maxMoveSpeed;
        public float moveSpeed;
        public float wallFriction;

        private string itemName;
        public string aButton = "A Button P1";
        public string leftJoyXAxis = "Left Stick X Axis P1";
        public string xButton = "X Button P1";

        void Start()
        {
            rBody = GetComponent<Rigidbody>();
            playerCollider = GetComponent<BoxCollider>();
        }

        void Update()
        {
            if (Input.GetButtonDown(aButton))
                jump = true;
            else
                jump = false;

            //Item movement
            if (pickUpItem == true)
                GameObject.Find(itemName).GetComponent<PickUp>().moveWithOwner(facingRight);
            
            //Throw Item
            if ((pickUpItem == true) && (Input.GetButtonDown(xButton)) && (pleaseWait == false))
            {
                pickUpItem = false;
                GameObject.Find(itemName).GetComponent<PickUp>().getThrown(facingRight);
            }

            //Wait until next update
            pleaseWait = false;
        }

        void FixedUpdate()
        {
            Vector3 movement = new Vector3(0f,0f,0f);

            //Get joystick input
            movement.x = Input.GetAxis(leftJoyXAxis) * moveSpeed;

            //Which direction is the player facing?
            if (rBody.velocity.x > 0)
                facingRight = true;
            else if (rBody.velocity.x < 0)
                facingRight = false;

            //Is the player touching the ground?
            if (Mathf.Abs(rBody.velocity.y) < 0.001)
                grounded = true;
            else
                grounded = false;
            
            //Jump
            if ((grounded) && ((jump) || (Input.GetButtonDown(aButton))))
                movement.y = jumpSpeed;
            
            //Wall jump and wall slide
            if ((wallTouch) && (!grounded))
            {
                if ((movement.x != 0) && (Mathf.Abs(rBody.velocity.x) < .001))
                    movement.y = +wallFriction;
                if (jump)
                {
                    movement.y = jumpSpeed;
                    if((movement.x != 0) && (Mathf.Abs(rBody.velocity.x) < .001))
                        movement.y =+ wallFriction;
                }
            }

            //Limit movement speed
            if ((rBody.velocity.x > maxMoveSpeed) && (movement.x > 0))
                movement.x = 0;
            else if ((rBody.velocity.x < (maxMoveSpeed * -1)) && (movement.x < 0))
                movement.x = 0;

            //Add some friction and drag
            if (Input.GetAxis(leftJoyXAxis) == 0)
                movement.x = (rBody.velocity.x * -1) * friction;

            //Movement
            rBody.AddForce(movement);
        }

        void OnTriggerStay(Collider otherCollider)
        {
            //Item interaction
            if (otherCollider.gameObject.tag == "GrabItem")
            {
                //Pick up item
                if ((Input.GetButton(xButton)) && (otherCollider.GetComponentInParent<PickUp>().thrown == false))
                {
                    pleaseWait = true;
                    pickUpItem = true;
                    itemName = otherCollider.transform.parent.name;

                    //Change item's info
                    otherCollider.GetComponentInParent<PickUp>().hasOwner = true;
                    otherCollider.GetComponentInParent<PickUp>().newOwner = gameObject.name;
                }

                //Get hit by item
                if (otherCollider.GetComponentInParent<PickUp>().thrown == true)
                    gotHit = true;
            }
        }
    }
}