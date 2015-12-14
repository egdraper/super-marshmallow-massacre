using UnityEngine;
using System.Collections;
using Assets.PlayerMovement;

namespace Assets.Scripts
{
    public class MallowMovement : MonoBehaviour
    {
        private Rigidbody rBody;
        private Collider playerCollider;

        private bool currentlyThrowing = false;
        private bool facingRight;
        private bool jump;
        private bool pleaseWait = false;
        private bool readyToThrow = false;
        private bool wallPush = false;
        private bool wallSlide = false;
        public bool gotHit;
        public bool grounded = false;
        public bool pickUpItem;
        public bool thrown;
        public bool wallTouch = false;
        public bool wallTouchLeft = false;
        public bool wallTouchRight = false;

        private float windUpTimer = 0f;
        public float extraGravity;
        public float friction;
        public float jumpForce;
        public float maxVelocity;
        public float moveAcceleration;
        public float nearMaxVelocity;
        public float wallFriction;
        public float wallPushDuration;
        public float wallPushForce;

        private string itemName;
        public string aButton = "A Button P1";
        public string leftJoyXAxis = "Left Stick X Axis P1";
        public string leftJoyYAxis = "Left Stick Y Axis P1";
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

            //Item movement
            if (pickUpItem == true)
                GameObject.Find(itemName).GetComponent<PickUp>().moveWithOwner(facingRight);

            //Player must release x button after initial pick up for the item to be ready to throw
            if ((pickUpItem) && (!readyToThrow) && (Input.GetButtonUp(xButton)))
            {
                readyToThrow = true;
                pleaseWait = true;
            }

            //Set throw force
            if (readyToThrow)
            {
                if (Input.GetButtonDown(xButton))
                    currentlyThrowing = true;

                if (currentlyThrowing)
                    windUpTimer += Time.deltaTime;

                if (windUpTimer > 3)
                    windUpTimer = 3;
            }

            //Throw Item
            if ((pickUpItem) && (Input.GetButtonUp(xButton)) && (!pleaseWait) && (readyToThrow))
            {
                Vector3 throwAngle = new Vector3(Input.GetAxis(leftJoyXAxis), Input.GetAxis(leftJoyYAxis), 0f).normalized;
                Debug.Log(throwAngle.y);
                pickUpItem = false;
                GameObject.Find(itemName).GetComponent<PickUp>().getThrown(facingRight, throwAngle, windUpTimer);
                readyToThrow = false;
                currentlyThrowing = false;
                windUpTimer = 0f;
            }

            //Wait until next update
            pleaseWait = false;
        }

        void FixedUpdate()
        {
            Vector3 moveForce = new Vector3(0f,0f,0f);

            //Get joystick input
            if ((!wallPush) && (!currentlyThrowing))
                moveForce.x = Input.GetAxis(leftJoyXAxis) * moveAcceleration;

            //Which direction is the player facing? 
            if (Input.GetAxis(leftJoyXAxis) > 0.1)
                facingRight = true;
            else if (Input.GetAxis(leftJoyXAxis) < -0.1)
                facingRight = false;
            
            //Jump
            if ((grounded) && ((jump) || (Input.GetButtonDown(aButton)))  && (rBody.velocity.y < .1))
                moveForce.y = jumpForce;

            //Is the player touching a wall?
            if ((wallTouchLeft) || (wallTouchRight))
                wallTouch = true;
            else
                wallTouch = false;

            //Wall slide
            if ((wallTouch) && (!grounded) && (moveForce.x != 0) && (Mathf.Abs(rBody.velocity.x) < .001))
            {
                if ((!wallSlide) && (rBody.velocity.y < 0))
                    rBody.velocity = rBody.velocity / 4;
                wallSlide = true;
            }
            else
                wallSlide = false;

            //Wall jump
            if ((wallTouch) && (!grounded) && (jump))
            {
                rBody.velocity = Vector3.zero;
                moveForce.y = +jumpForce;
                if (wallTouchLeft)
                    wallPushForce = Mathf.Abs(wallPushForce);
                else
                    wallPushForce = Mathf.Abs(wallPushForce) * -1;
                StartCoroutine(WallPushForceTimer(wallPushDuration));
            }

            if (wallPush)
                moveForce.x = +wallPushForce;

            //Limit movement speed
            if ((rBody.velocity.x > maxVelocity) && (moveForce.x > 0)) //right
                moveForce.x = 0;
            else if ((rBody.velocity.x < (maxVelocity * -1)) && (moveForce.x < 0)) //left
                moveForce.x = 0;

            if ((rBody.velocity.y < (maxVelocity * -1)) && (!wallSlide)) //free fall
                moveForce.y =+ (Mathf.Abs(Physics.gravity.y) + extraGravity);
            else if ((rBody.velocity.y < ((maxVelocity * -1) / 4)) && (wallSlide)) //wall slide
                moveForce.y =+ (Mathf.Abs(Physics.gravity.y) + extraGravity);

            //Add some friction and drag
            if ((Input.GetAxis(leftJoyXAxis) == 0) || (((rBody.velocity.x > 1) && (Input.GetAxis(leftJoyXAxis) < 0)) || ((rBody.velocity.x < -1) && (Input.GetAxis(leftJoyXAxis) > 0)))) //$#&@!!!
                moveForce.x = moveForce.x + (rBody.velocity.x * -1) * friction;

            //Add some player gravity
            moveForce.y = moveForce.y - extraGravity;

            //Movement
            rBody.AddForce(moveForce);

            //Reset values
            jump = false;
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

        IEnumerator WallPushForceTimer(float waitTime)
        {
            wallPush = true;
            yield return new WaitForSeconds(waitTime);
            wallPush = false;
        }
    }
}