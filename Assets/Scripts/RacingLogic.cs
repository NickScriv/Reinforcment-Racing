using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingLogic : MonoBehaviour
{
    [SerializeField] private TrackCheckPoints checkPointScript;
    [SerializeField] private Transform carCollider;
    [HideInInspector]
    public int position = 1;
    public int checkPointCount = 0;
 


    public int laps = 1;


    // Start is called before the first frame update
   public Transform currentCorrectCheckPoint;
   public Transform currentCheckPoint;

    void Start()
    {
        checkPointScript.OnCarCorrectCheckPoint += OnCorrectCheckPoint;
        checkPointScript.OnCarWrongCheckPoint += OnWrongCheckPoint;
        currentCorrectCheckPoint = checkPointScript.getNextCheckpoint(carCollider);
        currentCheckPoint = checkPointScript.getNextCheckpoint(carCollider);
     

    }

    void OnCorrectCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
    
        if (e.CarTransform == carCollider)
        {
            if (e.last)
            {
                laps++;

            }
            currentCorrectCheckPoint = checkPointScript.getNextCheckpoint(carCollider);
            currentCheckPoint = e.checkPointObj.transform;

        }
        


    }

    // When AI agent goes through the wrong checkpoint, give a penalty
    void OnWrongCheckPoint(object sender, TrackCheckPoints.CheckPointSystemArgs e)
    {
        if (e.CarTransform == carCollider)
        {

            currentCheckPoint = e.checkPointObj.transform;
          
         

        }
    }


    public int GetNextCorrectIndexCheckpoint()
    {
        return checkPointScript.getNextCheckpointIndex(carCollider);
    }

    public int GetCurrentIndexCheckpoint()
    {
        return checkPointScript.getCheckpointIndex(currentCheckPoint.GetComponent<CheckPoint>());
    }

    public int GetNextIndexCheckpoint()
    {
        return checkPointScript.getNextCheckpointIndex(carCollider);
    }

    public Transform GetTrans()
    {
        return transform;
    }

    public float GetDistanceToCheckpoint()
    {
        return Vector3.Distance(transform.position, currentCorrectCheckPoint.position);
    }
}
