using UnityEngine;
using System.Collections;

public class P1Movement : MonoBehaviour
{
    public float speed = 5;
    public float gravity;

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Left Stick X Axis");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        GetComponent<Rigidbody>().velocity = movement * speed;
	}
}
