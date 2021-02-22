using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform target;
    public float Zoomed;

    public float RotateSpeed;
    public float ZoomSpeed;

    public Vector3 TargetPosition;

    private void Start()
    {
        Zoomed = -5f;
    }

    private void FixedUpdate()
    {
        Mathf.Clamp(Zoomed, -5f, 5f);

        if(Zoomed < 5f && Input.GetAxis("Vertical") > 0f)
        {
            transform.position += transform.forward * Input.GetAxis("Vertical") * ZoomSpeed;
            Zoomed += Input.GetAxis("Vertical");
        }
        if(Zoomed > -5f && Input.GetAxis("Vertical") < 0f)
        {
            transform.position += transform.forward * Input.GetAxis("Vertical") * ZoomSpeed;
            Zoomed += Input.GetAxis("Vertical");
        }

        //Rotation

        transform.LookAt(target);

        transform.RotateAround(TargetPosition, Vector3.up, RotateSpeed*Input.GetAxis("Horizontal")*-1f);


    }
}
