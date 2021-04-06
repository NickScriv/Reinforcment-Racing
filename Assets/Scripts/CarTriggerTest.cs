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




}
