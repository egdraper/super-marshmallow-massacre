using UnityEngine;
using System.Collections;
using Assets.PlayerMovement;

namespace Assets.Scripts
{
    public class MallowMovement : MonoBehaviour
    {
        private Rigidbody rBody;
        private Collider playerCollider;

        private bool blockTouch;
        private bool facingRight;
        private bool grounded;
        private bool jump;

        public float friction;
        public float jumpSpeed;
        public float maxMoveSpeed;
        public float moveSpeed;

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

        void OnCollisionStay(Collision collider)
        {
            //Detect Wall
            if (collider.gameObject.tag == "Block")
                blockTouch = true;

            //Item interaction
            if (collider.gameObject.tag == "GrabItem")
                Debug.Log("Yay!");
        }
    }
}