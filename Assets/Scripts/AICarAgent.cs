using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System;
using Unity.MLAgents.Sensors;


public class AICarAgent : Agent
{
    [SerializeField] private RCC_CarControllerV3 carController;
    [SerializeField] private TrackCheckPoints checkPointScript;
    //[SerializeField] private 

    private void Awake()
    {
        
    }

    private void Start()
    {
        checkPointScript.OnCarCorrectCheckPoint += OnCorrectCheckPoint;
        checkPointScript.OnCarCorrectCheckPoint += OnWrongCheckPoint;

    }

    // When AI agent goes through correct checkpoint, give a reward
    void OnCorrectCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        if(e.CarTransform == transform)
        {
            AddReward(1f);
        }
    }

    // When AI agent goes through the wrong checkpoint, give a penalty
    void OnWrongCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        if (e.CarTransform == transform)
        {
            AddReward(-1f);
        }
    }

    public override void OnEpisodeBegin()
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = checkPointScript.getNextCheckpoint(transform) - transform.position;
        sensor.AddObservation(diff.normalized);

        // Add negative reward here for faster driving
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        carController.AIThrottle = vectorAction[0];
        carController.AIHandBrake = vectorAction[1];
        carController.AIsteer = vectorAction[2];
        carController.AIBrake = vectorAction[3];
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerRightInput);
        actionsOut[1] = Input.GetButton(RCC_Settings.Instance.Xbox_handbrakeKB) ? 1f : 0f;
        actionsOut[2] = Input.GetAxis(RCC_Settings.Instance.Xbox_horizontalInput);
        actionsOut[3] = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerLeftInput);
    }
}
