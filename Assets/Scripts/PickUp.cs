using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    private bool hasOwner = false;
    private string newOwner;
    private Vector3 travelWithOwner = Vector3.zero;

    void Update()
    {
        Rigidbody rBody = GetComponent<Rigidbody>();

        if (hasOwner == true)
        {
            travelWithOwner = GameObject.Find(newOwner).transform.position;
            travelWithOwner.x = travelWithOwner.x + 2;
            rBody.position = travelWithOwner;
        }
	}

    void OnTriggerStay(Collider collider)
    {
        print("la dee da");
        if (collider.gameObject.GetComponent<P1Movement>().pickUpItem == true)
        {
            hasOwner = true;
            Debug.Log("Hey!");
            newOwner = collider.gameObject.name;
        }
    }
}
