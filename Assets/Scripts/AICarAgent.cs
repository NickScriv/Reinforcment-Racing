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

    private float secondsCount = 0.0f;
    public float AIThrottle = 0.0f;
    public float AIBrake = 0.0f;
    public float AISteer = 0.0f;
    public float movement;
    public float leftSteerAngle = 0f;
    public float rightSteerAngle = 0f;
    public int maxEpisodeTime= 30;
    public LayerMask mask;
    public float cumReward = 0;
    public float speed = 0.0f;
    public bool frontCol = false;
    public bool rearCol = false;
    public float dirDot;
    public Transform check;
    public List<Transform> resets;
    public RearCheck rear;
    public RearCheck front;
    public int num;
    float delay = -1f;
    Rigidbody rb;
    Vector3 lastPos;
    public int laps = 0;
    Transform curReset;
    public CheckPoint lastCheck;
    bool completed = false;
    bool first = true;

    private void Awake()
    {
        carController = GetComponent<RCC_CarControllerV3>();
        rb = GetComponent<Rigidbody>();
        lastPos = transform.position;
        curReset = resets[Random.Range(0, (resets.Count - 1))];

    }

    private void Start()
    {
        checkPointScript.OnCarCorrectCheckPoint += OnCorrectCheckPoint;
        checkPointScript.OnCarWrongCheckPoint += OnWrongCheckPoint;
        check = checkPointScript.getNextCheckpoint(carCollider).transform;
        lastCheck = checkPointScript.getLastCheckPoint();

        for(int i = 1; i <= 5; i++)
        {
            if( i != num)
            {
                mask |= (1 << LayerMask.NameToLayer("TempTest" + i));
         
            }
        }


    }


    void OnCorrectCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        
        if (e.CarTransform == carCollider)
        {
            first = false;
            lastCheck = e.checkPointObj;
            check = checkPointScript.getNextCheckpoint(carCollider).transform;
        
            
            
            secondsCount = 0;

           /* if (e.last)
            {
                laps++;
                if (laps == 1)
                {
                    laps = 0;
                    completed = true;
                    CheckEndEpisode();
                }

            }*/

        }

        
    }

 
    void OnWrongCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        if (e.CarTransform == carCollider)
        {
            lastCheck = e.checkPointObj;
            if (0.0f > Vector3.Dot(e.checkPointObj.transform.forward, rb.velocity.normalized))
            {
                
                
                check = e.checkPointObj.transform;
            }
            else
            {
                check = checkPointScript.getNextCheckpoint(e.checkPointObj).transform;
                

            }
            
        }
    }



    private void FixedUpdate()
    {
       
        secondsCount += Time.fixedDeltaTime;
      
        if (secondsCount >= maxEpisodeTime)
        {
           
            secondsCount = 0;
            
            //CheckEndEpisode();
            Respawn();
        }
        speed = carController.speed;

        rearCol = rear.coll;
        frontCol = front.coll;


        
        if(gameObject.layer == LayerMask.NameToLayer("TempTest" + num) && delay < 0.0f)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4, mask);


            if(hitColliders.Length == 0)
            {
                SetLayerRecursively(gameObject, "RCC");
            }
        }
    

    }

    private void Update()
    {
        if(delay >= 0.0f)
            delay -= Time.deltaTime;
    }

    /* private void OnDrawGizmos()
     {
         Gizmos.color = Color.white;

      Gizmos.DrawWireSphere(transform.position , 4);
     }*/

    public override void CollectObservations(VectorSensor sensor)
    {
  
         dirDot = Vector3.Dot(transform.forward, check.forward); 
         AddReward(dirDot);

        movement = Scale(-0.5f, 0.5f, 0.0f, 200.0f, carController.speed);
        AddReward(Mathf.Clamp(movement, -0.5f, 0.5f));

        sensor.AddObservation(dirDot);

        sensor.AddObservation(check.forward);

        sensor.AddObservation(frontCol);
        sensor.AddObservation(rearCol);

        sensor.AddObservation(Mathf.Clamp01(carController.speed/200f));

        sensor.AddObservation(transform.forward);

     
        sensor.AddObservation(leftSteerAngle / 64f); 
        sensor.AddObservation(rightSteerAngle / 64f);

       



        cumReward = GetCumulativeReward();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
      
        AIThrottle =  vectorAction[0];

        AIBrake = vectorAction[1];

        AISteer = vectorAction[2];





        if (AIThrottle < 0.0f)
        {
            AIThrottle = 0.0f;
        }




        if (AIBrake < 0.0f)
        {
            AIBrake = 0.0f;
        }

        if (frontCol || dirDot <= -0.8)
        {
            AddReward((-AIThrottle + AIBrake) * 0.3f);
        }
        if (rearCol)
        {
            AddReward((AIThrottle + -AIBrake) * 0.3f);
        }


    }


    public override void Heuristic(float[] actionsOut)
    {
      
        actionsOut[0] = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerRightInput);
        actionsOut[1] = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerLeftInput);
        actionsOut[2] = Input.GetAxis(RCC_Settings.Instance.Xbox_horizontalInput);
    }


    public override void OnEpisodeBegin()
    {
        /* rb.velocity = Vector3.zero;
          rb.angularVelocity = Vector3.zero;
         curReset = resets[Random.Range(0, (resets.Count - 1))];
         this.transform.position = curReset.position;
         Quaternion rot = curReset.rotation;
         rot *= Quaternion.Euler(0, Random.Range(0f, 360f), 0);
         this.transform.rotation = rot;


         rb.velocity = Vector3.zero;
         rb.angularVelocity = Vector3.zero;


         checkPointScript.ResetCheckPoints(carCollider);
         first = true;
          check = checkPointScript.getNextCheckpoint(carCollider).transform;
          secondsCount = 0;
          laps = 0;*/

    

    }

    void Respawn()
    {
       
        CheckPoint spawn;
        delay = 2.5f;
        if (first)
        {
            spawn = checkPointScript.getLastCheckPoint();
        }
        else
        {
            spawn = lastCheck;
        }
        checkPointScript.SetCheckPoints(carCollider, spawn);
        check = checkPointScript.getNextCheckpoint(spawn).transform;
        SetLayerRecursively(gameObject, "TempTest" + num);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        secondsCount = 0;
        this.transform.position = spawn.transform.position + (spawn.transform.up);
        this.transform.rotation = spawn.transform.rotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public static void SetLayerRecursively(GameObject go, string layer)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }


    void CheckEndEpisode()
    {
      // EndEpisode();

    }

    private float Scale(float from, float to, float from2, float to2, float val)
    {
        if (val <= from2)
            return from;
        else if (val >= to2)
            return to;
        return (to - from) * ((val - from2) / (to2 - from2)) + from;
    }

}
