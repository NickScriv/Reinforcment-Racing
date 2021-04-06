using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearCheck : MonoBehaviour
{


    public bool coll = false;

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            
            coll = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            coll = false;
        }
    }
}
