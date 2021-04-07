using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class TrackCheckPoints : MonoBehaviour
{
    public class CheckPointSystemArgs : EventArgs
    {
        public Transform CarTransform;
        public bool last = false;
        public CheckPoint checkPointObj;
        public int index;
    }

   
    public event EventHandler<CheckPointSystemArgs> OnCarCorrectCheckPoint;
    public event EventHandler<CheckPointSystemArgs> OnCarWrongCheckPoint;
    List<CheckPoint> checkPointList;
    List<int> nextCheckPointIndexList;
    [SerializeField] List<Transform> AICars;
    private void Awake()
    {
        checkPointList = new List<CheckPoint>();
        int i = 0;
        foreach (Transform checkPointTrans in transform)
        {
            CheckPoint checkPoint = checkPointTrans.GetComponent<CheckPoint>();
            checkPoint.SetTrackCheckPoints(this);
            checkPoint.index = i;
            checkPointList.Add(checkPoint);
            i++;
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
            bool lastCheck = false;
            if(checkPointList.Count - 1 == nextCheckPointIndex)
            {

                lastCheck = true;
            }
            nextCheckPointIndexList[AICars.IndexOf(AICarTrans)] = (nextCheckPointIndex + 1) % checkPointList.Count;
            OnCarCorrectCheckPoint?.Invoke(this, new CheckPointSystemArgs { CarTransform = AICarTrans, last = lastCheck, checkPointObj = checkPoint });
 
        }
        else
        {
            //Debug.Log("Wrong");
            OnCarWrongCheckPoint?.Invoke(this, new CheckPointSystemArgs { CarTransform = AICarTrans, checkPointObj = checkPoint});
        }
    }

    public Transform getNextCheckpoint(Transform AICarTrans)
    {
       
       
        int nextCheckpointIndex = nextCheckPointIndexList[AICars.IndexOf(AICarTrans)];
        
        return checkPointList[nextCheckpointIndex].transform;
    }

    public CheckPoint getLastCheckPoint()
    {

        return checkPointList[checkPointList.Count - 1];
    }

    public CheckPoint getNextCheckpoint(CheckPoint check)
    {

        int curCheck = checkPointList.IndexOf(check);
        curCheck = (curCheck + 1) % checkPointList.Count;

        return checkPointList[curCheck];
    }



    public int getNextCheckpointIndex(Transform AICarTrans)
    {

        int nextCheckpointIndex = nextCheckPointIndexList[AICars.IndexOf(AICarTrans)];

        return nextCheckpointIndex;
    }

    public int getCheckpointIndex(CheckPoint check)
    {

        return checkPointList.IndexOf(check);
    }


    public void ResetCheckPoints(Transform AICarTrans)
    {

        nextCheckPointIndexList[AICars.IndexOf(AICarTrans)] = 0;
      

    }

    public void SetCheckPoints(Transform AICarTrans, CheckPoint check)
    {
        int index = checkPointList.IndexOf(check);
        index = (index + 1) % checkPointList.Count;
        nextCheckPointIndexList[AICars.IndexOf(AICarTrans)] = index;


    }
}
