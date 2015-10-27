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
    public int wallJumpTime;

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

        movement.x = Input.GetAxis("Left Stick X Axis") * moveSpeed;

        //Jump
        if (controller.isGrounded)
        {
            movement.y = 0;
            if (Input.GetButton("A Button"))
                movement.y = jumpSpeed;
        }

        //Wall Jump
        if ((blockTouch == true) && (controller.isGrounded == false))
        {
            if (Input.GetButton("A Button"))
            {
                if (movement.x > 0) //Player is going right
                {
                    movement.y = jumpSpeed;
                }
                else if (movement.x < 0) //Player is going left
                {
                    movement.y = jumpSpeed;
                }
            }
        }

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
