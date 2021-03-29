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
        
        if(dirDot < 0.1f)
        {
           
            AddReward(-1f);
        }

        sensor.AddObservation(dirDot);
        sensor.AddObservation(dirToTarget);
        sensor.AddObservation(frontCol);
        sensor.AddObservation(Mathf.Clamp01(carController.speed/200f));
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(leftSteerAngle / 64f); 
        sensor.AddObservation(rightSteerAngle / 64f);

       /* if(carController.speed > 15f)
        {
            AddReward(Mathf.Clamp01(carController.speed / 175f) * 0.05f);
        }*/
        cumReward = GetCumulativeReward();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //Debug.Log(vectorAction.Length);
        AIThrottle =  vectorAction[0];

        if (AIThrottle < 0.0f)
        {
            AIThrottle = 0.0f;
        }

        //AIThrottle = ScaleAction(AIThrottle, 0.0f, 1.0f);
        AIBrake = vectorAction[1];

        if (AIBrake < 0.0f)
        {
            AIBrake = 0.0f;
        }

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

        if (frontCol)
        {
            //Debug.Log("PEN");
           
            AddReward((-AIThrottle + AIBrake) * 0.5f);
        }


        /*movement = Scale(-0.005f, 0.0025f, 0.0f, 200.0f, carController.speed);   
        AddReward(Mathf.Clamp(movement, -0.005f, 0.0025f));*/

        if (carController.speed > 15f)
        {
            AddReward(Mathf.Clamp01(carController.speed / 200f) * 0.05f);
        }

        //AddReward(-0.005f);

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
         /*if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "AICar" || collision.gameObject.tag == "Player")
         {
             //Debug.Log("Collision stay penalty");
             AddReward(-1f);
         }*/

    
       
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
            AddReward(-1f);
            
            
            
            //Physics.IgnoreCollision(carCollider.gameObject.GetComponent<MeshCollider>(), collision.collider);
        }
    }

   

     private void OnTriggerEnter(Collider other)
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
     }

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
