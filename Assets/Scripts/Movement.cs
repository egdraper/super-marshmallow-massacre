using UnityEngine;
using System.Collections;
using Assets.PlayerMovement;

namespace Assets.Scripts
{
    public class Movement : MonoBehaviour
    {
        public IMovement _currentState;
        public IMovement _deadState;
        public IMovement _fallingState;
        public IMovement _gettingHitState;
        public IMovement _jumpingState;
        public IMovement _motionlessState;
        public IMovement _runningState;
        public IMovement _walkingState;
        public IMovement _throwingState;

        private Rigidbody rBody;
        private Collider playerCollider;

        private Vector3 movement = Vector3.zero;
        public float moveSpeed;
        public float maxMoveSpeed;
        public float jumpSpeed;
        private float fallSpeed;
        private bool blockTouch;
        public float wallThrust;
        private int wallJumpTime;
        public float wallJumpTimeDuration;
        public  bool pickUpItem = false;
        private bool facingRight;
        private bool pleaseWait = false;  //To force to wait until next update frame.
        public bool gotHit = false;
        private bool grounded = false;

        private string itemName;

        public string leftJoyXAxis = "Left Stick X Axis P1";
        public string aButton = "A Button P1";
        public string xButton = "X Button P1";

        public Movement()
        {
            _deadState = new DeadState(this);
            _fallingState = new FallingState(this);
            _gettingHitState = new GettingHitState(this);
            _jumpingState = new JumpingState(this);
            _motionlessState = new MotionlessState(this);
            _runningState = new RunningState(this);
            _walkingState = new WalkingState(this);
            _throwingState = new ThrowingState(this);

            _currentState = _motionlessState;
        }

        void Start()
        {
            rBody = GetComponent<Rigidbody>();
            playerCollider = GetComponent<BoxCollider>();

            fallSpeed = jumpSpeed * (-1);
            blockTouch = false;
            wallJumpTime = 0;
        }

        void FixedUpdate()
        {
            //if ((wallJumpTime == 0) || (wallJumpTime > wallJumpTimeDuration))
                movement.x = Input.GetAxis(leftJoyXAxis) * moveSpeed;

            if (Mathf.Abs(rBody.velocity.y) < 0.001)
                grounded = true;
            else
                grounded = false;

            //Determine which way the player is facing.
            if (rBody.velocity.x > 0)
                facingRight = true;
            else if (rBody.velocity.x < 0)
                facingRight = false;

            //Is the player currently stunned?
            if (gotHit)
            {
                StartCoroutine(GotHitTimer(3f));
                gotHit = false;
            }

            //Jump
            if ((grounded) && (Input.GetButtonDown(aButton)))
                rBody.AddForce(0f,jumpSpeed,0f);

            //Wall Jump and Wall Slide
            if ((blockTouch) && (!grounded))
            {
                if (Input.GetButtonDown(aButton))
                {
                    movement.y = jumpSpeed;
                    if (facingRight) //Player is going right
                    {
                        movement.x = moveSpeed * (-1);
                        wallJumpTime = 1;
                    }
                    else if (!facingRight) //Player is going left
                    {
                        movement.x = moveSpeed;
                        wallJumpTime = 1;
                    }
                }
                if ((movement.x != 0) && (rBody.velocity.x == 0))
                    fallSpeed = (jumpSpeed * (-1)) / 4; //Wall slide speed equation
                else
                    fallSpeed = jumpSpeed * (-1);
            }
            else
                fallSpeed = jumpSpeed * (-1);

            //Movement
            //if (Mathf.Abs(movement.y) > fallSpeed)
            //    movement.y = 0;

            if (grounded)
                movement.y = 0;

            if ((rBody.velocity.x > maxMoveSpeed) && (Input.GetAxis(leftJoyXAxis) > 0))
                movement.x = 0;
            else if ((rBody.velocity.x < (maxMoveSpeed * -1)) && (Input.GetAxis(leftJoyXAxis) < 0))
                movement.x = 0;

            rBody.AddForce(movement);

            //Item Movement
            if (pickUpItem == true)
                GameObject.Find(itemName).GetComponent<PickUp>().moveWithOwner(facingRight);

            //Throw Item
            if ((pickUpItem == true) && (Input.GetButtonDown(xButton)) && (pleaseWait == false))
            {
                pickUpItem = false;
                GameObject.Find(itemName).GetComponent<PickUp>().getThrown(facingRight);
            }

            pleaseWait = false;
            Debug.Log(blockTouch);
        }

        void OnCollisionStay(Collision collider)
        {
             //Detect Wall
            if (collider.gameObject.tag == "Block")
                blockTouch = true;

            //Item interaction
            if (collider.gameObject.tag == "GrabItem")
            {
                //Pick up item
                if ((Input.GetButton(xButton)) && (collider.collider.GetComponent<PickUp>().thrown == false))
                {
                    pleaseWait = true;
                    pickUpItem = true;
                    itemName = collider.collider.name;
                }

                //Get hit by item
                if (collider.collider.GetComponent<PickUp>().thrown == true)
                    gotHit = true;
            }
        }

        void OnCollisionExit(Collision collider)
        {
            if (collider.gameObject.tag == "Block")
                blockTouch = false;
        }

        IEnumerator GotHitTimer(float waitTime)
        {
            playerCollider.isTrigger = false;
            rBody.isKinematic = false;
            rBody.useGravity = true;
            yield return new WaitForSeconds(waitTime);
            rBody.useGravity = false;
            rBody.isKinematic = true;
            playerCollider.isTrigger = true;
        }

        //IEnumerator WallJumpWait(float waitTime)
        //{

        //}
    }
}
