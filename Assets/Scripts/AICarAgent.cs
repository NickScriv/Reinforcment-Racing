using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System;
using Unity.MLAgents.Sensors;


public class AICarAgent : Agent
{
    RCC_CarControllerV3 carController;
    [SerializeField] private TrackCheckPoints checkPointScript;
    [SerializeField] private Transform carCollider;

    public float AIThrottle;
    public float AIBrake;
    public float AISteer;
    private float rewardScalar = 0.0005f;
    public float leftSteerAngle = 0f;
    public float rightSteerAngle = 0f;
    public Transform reset;
    bool step = true;
    Rigidbody rb;


    //[SerializeField] private 

    private void Awake()
    {
        carController = GetComponent<RCC_CarControllerV3>();
        rb = GetComponent<Rigidbody>();
      
    }

    private void Start()
    {
        checkPointScript.OnCarCorrectCheckPoint += OnCorrectCheckPoint;
        checkPointScript.OnCarWrongCheckPoint += OnWrongCheckPoint;

    }



    // When AI agent goes through correct checkpoint, give a reward
    void OnCorrectCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        if(e.CarTransform == carCollider)
        {
            //Debug.Log("Through checkpoint award");
            AddReward(1f);
        
        }
    }

    // When AI agent goes through the wrong checkpoint, give a penalty
    void OnWrongCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        if (e.CarTransform == carCollider)
        {
            //Debug.Log("Through wrong checkpoint penalty");
            AddReward(-1f);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 forward = checkPointScript.getNextCheckpoint(carCollider).forward;
        float dirDot = Vector3.Dot(transform.forward, forward);
        sensor.AddObservation(dirDot);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(forward);
        //Debug.Log("Right: " + rightSteerAngle + " Left: " + leftSteerAngle);
        sensor.AddObservation(leftSteerAngle);
        sensor.AddObservation(rightSteerAngle);


        // Add negative reward here for faster driving
        AddReward(-0.001f);

        // Reward agent if it is facing in the correct direction
        AddReward(rewardScalar * dirDot);

        //Debug.Log(GetCumulativeReward());
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //Debug.Log(vectorAction.Length);
        AIThrottle = vectorAction[0];
        AIBrake = vectorAction[1];
        AISteer = vectorAction[2];
        
    }

    public override void Heuristic(float[] actionsOut)
    {
         //Debug.Log(actionsOut.Length);
         actionsOut[0] = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerRightInput);
         actionsOut[1] = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerLeftInput);
         actionsOut[2] = Input.GetAxis(RCC_Settings.Instance.Xbox_horizontalInput);
    }

    void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Coll stay");
       /* if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "AICar" || collision.gameObject.tag == "Player")
        {
            //Debug.Log("Collision stay penalty");
            AddReward(-0.1f);
        }*/
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Coll enter");
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "AICar" || collision.gameObject.tag == "Player")
        {
            // Debug.Log("Collision enter penalty");
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            AddReward(-1f);
            EndEpisode();
            //Physics.IgnoreCollision(carCollider.gameObject.GetComponent<MeshCollider>(), collision.collider);
        }
    }

    public override void OnEpisodeBegin()
    {
        step = false;
        this.transform.position = reset.position;
        this.transform.rotation = reset.rotation;
        checkPointScript.ResetCheckPoints(carCollider);
    }


 
}
