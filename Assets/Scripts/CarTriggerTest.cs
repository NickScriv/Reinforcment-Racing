using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTriggerTest : MonoBehaviour
{
    public float speed = 0.0f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /* private void FixedUpdate()
     {
         speed = rb.velocity.magnitude;
     }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("dhdhd");
        }
    }

    void OnCollisionStay(Collision collision)
    {

        //Debug.Log("Coll enter");
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "AICar" || collision.gameObject.tag == "Player")
        {
            Debug.Log("Coll");
        }
    }


}
