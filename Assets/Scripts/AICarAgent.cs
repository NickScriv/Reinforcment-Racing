//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
//using System;
using Unity.MLAgents.Sensors;


public class AICarAgent : Agent
{
    RCC_CarControllerV3 carController;
    [SerializeField] private TrackCheckPoints checkPointScript;
    [SerializeField] private Transform carCollider;

    private float secondsCount;
    private int minuteCount;

    public float AIThrottle = 0.0f;
    public float AIBrake = 0.0f;
    public float AISteer = 0.0f;
    public float movement;
    public float leftSteerAngle = 0f;
    public float rightSteerAngle = 0f;
    public int maxEpisodeTime= 240;
    public float cumReward = 0;
    public float speed = 0.0f;
    public List<Transform> resets;
    Rigidbody rb;
    const float BRAKEPEN = 0.008f;
    Vector3 lastPos;
    int laps = 0;
    int collisionCount = 0;
    bool frontCol = false;
    bool first = true;



    //[SerializeField] private 

    private void Awake()
    {
        carController = GetComponent<RCC_CarControllerV3>();
        rb = GetComponent<Rigidbody>();
        lastPos = transform.position;

      
    }

    private void Start()
    {
        checkPointScript.OnCarCorrectCheckPoint += OnCorrectCheckPoint;
        checkPointScript.OnCarWrongCheckPoint += OnWrongCheckPoint;
    }



    // When AI agent goes through correct checkpoint, give a reward
    void OnCorrectCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        first = false;
        if (e.CarTransform == carCollider)
        {
            

            AddReward(1f);
            
            //Debug.Log("Through checkpoint award");
            
            secondsCount = 0;

            if (e.last)
            {
                laps++;
                if (laps == 3)
                {
                    Debug.Log("Finished Race");
                    EndEpisode();
                }

            }

        }
    }

    // When AI agent goes through the wrong checkpoint, give a penalty
    void OnWrongCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
       /* if (e.CarTransform == carCollider)
        {
            //Debug.Log("Through wrong checkpoint penalty");
            AddReward(-1f);
        }*/
    }

    private void FixedUpdate()
    {
        secondsCount += Time.fixedDeltaTime;
        ///Debug.Log(secondsCount);
        if (secondsCount >= maxEpisodeTime)
        {
            secondsCount = 0;
            Debug.Log("Took to long");
            //AddReward(-1f);
            
            EndEpisode();
        }
        speed = carController.speed;
    }

    public override void CollectObservations(VectorSensor sensor)
    {


        Vector3 checkPos = checkPointScript.getNextCheckpoint(carCollider).position;
        Vector3 dirToTarget = (checkPos - transform.position).normalized;
        float dirDot = Vector3.Dot(transform.forward, dirToTarget);
        sensor.AddObservation(dirDot);
        if(dirDot < 0.0f)
        {
           
            AddReward(-0.1f);
        }
        sensor.AddObservation(dirToTarget);
       //sensor.AddObservation(carController.speed);
        //sensor.AddObservation(rb.velocity);
        //sensor.AddObservation(this.transform.InverseTransformPoint(checkPos));
        sensor.AddObservation(transform.forward);
      
       /* Debug.Log("Rigid: " + rb.velocity.normalized);
        Debug.Log("forward: " + transform.forward);*/
        sensor.AddObservation(leftSteerAngle / 64f); 
        sensor.AddObservation(rightSteerAngle / 64f);


        // Add negative reward here for faster driving
        //AddReward(-0.005f);

        // Reward agent if it is facing in the correct direction
        /* if(dirDot < 0)
         {
             dirDot = 0;
         }

         AddReward(rewardScalar * dirDot);*/
        //AddReward(-0.005f);
        cumReward = GetCumulativeReward();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //Debug.Log(vectorAction.Length);
        AIThrottle =  vectorAction[0];
        //AIThrottle = ScaleAction(AIThrottle, 0.0f, 1.0f);
        AIBrake = vectorAction[1];
        //AIBrake = ScaleAction(AIBrake, 0.0f, 1.0f);
        AISteer = vectorAction[2];



        /*if (AIBrake > 0.0f && carController.speed < 20.0f )
        {
            AddReward(-BRAKEPEN);
        }*/
        /* else if(frontCol && AIThrottle > 0.0f)
         {
             //Debug.Log("PEN");
             AddReward((-AIThrottle + AIBrake) * BRAKEPEN);
         }*/


        /*movement = Scale(-0.005f, 0.0025f, 0.0f, 180.0f, carController.speed);   
        AddReward(Mathf.Clamp(movement, -0.005f, 0.0025f));*/

        //AddReward(-0.001f);

       /* if(first)
        {
            AIBrake  = 0.0f;
            AIThrottle = 1.0f;
        }*/



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
         if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "AICar" || collision.gameObject.tag == "Player")
         {
             //Debug.Log("Collision stay penalty");
             AddReward(-0.1f);
         }

    
       
    }

    void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log("Coll enter");
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "AICar" || collision.gameObject.tag == "Player")
        {
            /*collisionCount++;
            if(collisionCount == 5)
            {
                collisionCount = 0;
                

            }*/
            // Debug.Log("Collision enter penalty");
            /*rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            EndEpisode();
            secondsCount = 0;*/
            AddReward(-0.5f);
            
            
            
            //Physics.IgnoreCollision(carCollider.gameObject.GetComponent<MeshCollider>(), collision.collider);
        }
    }

   

   /*  private void OnTriggerEnter(Collider other)
     {
         if (other.gameObject.CompareTag("Wall"))
         {
             frontCol = true;
         }
     }

     private void OnTriggerExit(Collider other)
     {
         if (other.gameObject.CompareTag("Wall"))
         {
             frontCol = false;
         }
     }*/

    public override void OnEpisodeBegin()
    {
        // step = false;
        Transform reset = resets[Random.Range(0, (resets.Count - 1))];
        this.transform.position = reset.position;
        this.transform.rotation = reset.rotation;
        checkPointScript.ResetCheckPoints(carCollider);
        secondsCount = 0;
        laps = 0;
    }

    private float Scale(float from, float to, float from2, float to2, float val)
    {
        if (val <= from2)
            return from;
        else if (val >= to2)
            return to;
        return (to - from) * ((val - from2) / (to2 - from2)) + from;
    }
    //.0075
    // -0.005, 0.0025 




}
