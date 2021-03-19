using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class TrackCheckPoints : MonoBehaviour
{
    public class CheckPointSystemArgs : EventArgs
    {
        public Transform CarTransform; 
    }

   
    public event EventHandler<CheckPointSystemArgs> OnCarCorrectCheckPoint;
    public event EventHandler<CheckPointSystemArgs> OnCarWrongCheckPoint;
    List<CheckPoint> checkPointList;
    List<int> nextCheckPointIndexList;
    [SerializeField] List<Transform> AICars;
    private void Awake()
    {
        checkPointList = new List<CheckPoint>();
        foreach (Transform checkPointTrans in transform)
        {
            CheckPoint checkPoint = checkPointTrans.GetComponent<CheckPoint>();
            checkPoint.SetTrackCheckPoints(this);
            checkPointList.Add(checkPoint);
        }
        nextCheckPointIndexList = new List<int>();

        foreach (Transform car in AICars)
        {
           
            nextCheckPointIndexList.Add(0);
        }
     
    }

    public void ThroughCheckPoint(CheckPoint checkPoint, Transform AICarTrans)
    {
        //Debug.Log(AICars.IndexOf(AICarTrans));
        int nextCheckPointIndex = nextCheckPointIndexList[AICars.IndexOf(AICarTrans)];

        
        if (checkPointList.IndexOf(checkPoint) == nextCheckPointIndex)
        {
            //Debug.Log(("correct"));
            nextCheckPointIndexList[AICars.IndexOf(AICarTrans)] = (nextCheckPointIndex + 1) % checkPointList.Count;
            OnCarCorrectCheckPoint?.Invoke(this, new CheckPointSystemArgs { CarTransform = AICarTrans });
 
        }
        else
        {
            //Debug.Log("Wrong");
            OnCarWrongCheckPoint?.Invoke(this, new CheckPointSystemArgs { CarTransform = AICarTrans});
        }
    }

    public Transform getNextCheckpoint(Transform AICarTrans)
    {
       
       
        int nextCheckpointIndex = nextCheckPointIndexList[AICars.IndexOf(AICarTrans)];
        
        return checkPointList[nextCheckpointIndex].transform;
    }

    public void ResetCheckPoints(Transform AICarTrans)
    {

        nextCheckPointIndexList[AICars.IndexOf(AICarTrans)] = 0;
      

    }
}
