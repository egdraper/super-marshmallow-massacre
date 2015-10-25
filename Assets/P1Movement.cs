using UnityEngine;
using System.Collections;

public class P1Movement : MonoBehaviour
{
    private Rigidbody player;
    public float moveSpeed;
    public float jumpSpeed;
    private float fallSpeed;
    public float gravity;

    void Start()
    {
        player = GetComponent<Rigidbody>();
        fallSpeed = jumpSpeed * (-1);
    }

    void FixedUpdate()
    {
        CharacterController controller = GetComponent<CharacterController>();

        //Storing value from moving left joystick
        float moveHorizontal = Input.GetAxis("Left Stick X Axis");
        float moveVertical = player.velocity.y;

        //Detecting whether or not jump button was pressed, if the player was touching the ground, and if a jump should take place
        if ((Input.GetButtonDown("A Button")) && (controller.isGrounded))
            moveVertical = jumpSpeed;
        else if (controller.isGrounded)
        {
            moveVertical = 0.001f;
            print("Grounded!");
        }
        else
        {
            moveVertical -= gravity; //crazy gravity equation
            if (moveVertical < fallSpeed)
                moveVertical = fallSpeed;
        }

        //Moving the player
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        player.transform.Translate(movement*Time.deltaTime*moveSpeed);
	}

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Floor")
    //        touchFloor = true;
    //}
}
