﻿using UnityEngine;
using System.Collections;
using Assets.PlayerMovement;

namespace Assets.Scripts
{
    public class MallowMovement : MonoBehaviour
    {
        private Rigidbody rBody;
        private Collider playerCollider;

        private bool facingRight;
        public bool grounded = false;
        private bool jump;
        private bool pleaseWait = false;
        public bool gotHit;
        public bool pickUpItem;
        public bool thrown;
        public bool wallTouch = false;

        public float extraGravity;
        public float friction;
        public float jumpForce;
        public float maxVelocity;
        public float moveAcceleration;
        public float nearMaxVelocity;
        public float wallFriction;

        private string itemName;
        public string aButton = "A Button P1";
        public string leftJoyXAxis = "Left Stick X Axis P1";
        public string xButton = "X Button P1";

        void Start()
        {
            rBody = GetComponent<Rigidbody>();
            playerCollider = GetComponent<BoxCollider>();

            jumpForce = jumpForce * 10f;
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
            Vector3 moveForce = new Vector3(0f,0f,0f);

            //Get joystick input
            moveForce.x = Input.GetAxis(leftJoyXAxis) * moveAcceleration;

            //Which direction is the player facing?
            if (rBody.velocity.x > 0.1)
                facingRight = true;
            else if (rBody.velocity.x < -0.1)
                facingRight = false;
            
            //Jump
            if ((grounded) && ((jump) || (Input.GetButtonDown(aButton)))  && (rBody.velocity.y < .1))
                moveForce.y = jumpForce;
            
            //Wall jump and wall slide
            if ((wallTouch) && (!grounded))
            {
                if ((moveForce.x != 0) && (Mathf.Abs(rBody.velocity.x) < .001))
                    moveForce.y = +wallFriction;
                if (jump)
                {
                    moveForce.y = jumpForce;
                    if((moveForce.x != 0) && (Mathf.Abs(rBody.velocity.x) < .001))
                        moveForce.y =+ wallFriction;
                }
            }

            //Limit movement speed
            if ((rBody.velocity.x > maxVelocity) && (moveForce.x > 0)) //right
            {
                moveForce.x = 0;
            }
            else if ((rBody.velocity.x < (maxVelocity * -1)) && (moveForce.x < 0)) //left
                moveForce.x = 0;

            //Add some friction and drag
            if ((Input.GetAxis(leftJoyXAxis) == 0) || (((rBody.velocity.x > 1) && (Input.GetAxis(leftJoyXAxis) < 0)) || ((rBody.velocity.x < -1) && (Input.GetAxis(leftJoyXAxis) > 0)))) //$#&@!!!
                moveForce.x = moveForce.x + (rBody.velocity.x * -1) * friction;

            //Add some player gravity
            moveForce.y = moveForce.y - extraGravity;

            //Movement
            rBody.AddForce(moveForce);
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