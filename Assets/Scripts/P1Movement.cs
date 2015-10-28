using UnityEngine;
using System.Collections;

public class P1Movement : MonoBehaviour
{
    private Vector3 movement = Vector3.zero;
    public float moveSpeed;
    public float jumpSpeed;
    private float fallSpeed;
    public float gravity;
    private bool blockTouch;
    public float wallThrust;
    private int wallJumpTime;
    public float wallJumpTimeDuration;

    void Start()
    {
        fallSpeed = jumpSpeed * (-1);
        blockTouch = false;
        wallJumpTime = 0;
    }

    void FixedUpdate()
    {
        CharacterController controller = GetComponent<CharacterController>();
        Rigidbody rBody = GetComponent<Rigidbody>();

        if ((wallJumpTime == 0) || (wallJumpTime > wallJumpTimeDuration))
        {
            movement.x = Input.GetAxis("Left Stick X Axis") * moveSpeed;
            wallJumpTime = 0;
        }
        else
            wallJumpTime++;

        //Jump
        if (controller.isGrounded)
        {
            movement.y = 0;
            if (Input.GetButton("A Button"))
                movement.y = jumpSpeed;
        }

        //Wall Jump and Wall Slide
        if ((blockTouch == true) && (controller.isGrounded == false))
        {
            if (Input.GetButton("A Button"))
            {
                if (movement.x > 0) //Player is going right
                {
                    movement.y = jumpSpeed;
                    movement.x = moveSpeed * (-1);
                    wallJumpTime = 1;
                }
                else if (movement.x < 0) //Player is going left
                {
                    movement.y = jumpSpeed;
                    movement.x = moveSpeed;
                    wallJumpTime = 1;
                }
            }
            if ((movement.x != 0) && (controller.velocity.x == 0))
                fallSpeed = (jumpSpeed * (-1)) / 4; //Wall slide speed equation
            else
                fallSpeed = jumpSpeed * (-1);
        }
        else
            fallSpeed = jumpSpeed * (-1);

        //Movement
        if ((controller.velocity.y == 0) && (controller.isGrounded == false))
            movement.y = 0;

        movement.y -= gravity * Time.deltaTime;

        if (movement.y < fallSpeed)
            movement.y = fallSpeed;

        controller.Move(movement * Time.deltaTime);
	}

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Block")
            blockTouch = true;
    }

    void OnTriggerExit (Collider collider)
    {
        if (collider.gameObject.tag == "Block")
            blockTouch = false;
    }
}
