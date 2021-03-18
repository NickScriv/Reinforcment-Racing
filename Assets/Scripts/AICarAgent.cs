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

    //[SerializeField] private 

    private void Awake()
    {
        carController = GetComponent<RCC_CarControllerV3>();
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
            Debug.Log("Through checkpoint award");
            AddReward(1f);
        }
    }

    // When AI agent goes through the wrong checkpoint, give a penalty
    void OnWrongCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        if (e.CarTransform == carCollider)
        {
            Debug.Log("Through wrong checkpoint penalty");
            AddReward(-1f);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = checkPointScript.getNextCheckpoint(carCollider) - carCollider.position;
        sensor.AddObservation(diff.normalized);

        // Add negative reward here for faster driving
        AddReward(-0.001f);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //Debug.Log(vectorAction.Length);
        carController.AIThrottle = vectorAction[0];
        carController.AIBrake = vectorAction[1];
        carController.AIsteer = vectorAction[2];
        
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
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "AICar" || collision.gameObject.tag == "Player")
        {
            Debug.Log("Collision stay penalty");
            AddReward(-0.1f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "AICar" || collision.gameObject.tag == "Player")
        {
            Debug.Log("Collision enter penalty");
            AddReward(-0.5f);
        }
    }
}
