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

    void Start()
    {
        fallSpeed = jumpSpeed * (-1);
        blockTouch = false;
    }

    void FixedUpdate()
    {
        CharacterController controller = GetComponent<CharacterController>();

        movement.x = Input.GetAxis("Left Stick X Axis") * moveSpeed;

        if (controller.isGrounded)
        {
            movement.y = 0;
            if (Input.GetButton("A Button"))
                movement.y = jumpSpeed;
        }

        if ((blockTouch == true) && (controller.isGrounded == false))
        {
            if (Input.GetButton("A Button"))
                movement.y = jumpSpeed;
            print("Wall Jump!");
        }

        movement.y -= gravity * Time.deltaTime;
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
