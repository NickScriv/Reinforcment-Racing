using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackCheckPoints : MonoBehaviour
{
    public event EventHandler OnCarCorrectCheckPoint;
    public event EventHandler OnCarWrongCheckPoint;
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
        int nextCheckPointIndex = nextCheckPointIndexList.IndexOf(AICars.IndexOf(AICarTrans));
        if(checkPointList.IndexOf(checkPoint) == nextCheckPointIndex)
        {
            Debug.Log("correct");
            nextCheckPointIndex = (nextCheckPointIndex + 1) % checkPointList.Count;
            OnCarCorrectCheckPoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Wrong");
            OnCarWrongCheckPoint?.Invoke(this, EventArgs.Empty);
        }
    }
}
