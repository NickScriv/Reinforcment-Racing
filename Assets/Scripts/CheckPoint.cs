using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int index = 0;
    TrackCheckPoints trackCheckPoints;
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.CompareTag("AICar") || other.gameObject.CompareTag("Player"))
        {
            trackCheckPoints.ThroughCheckPoint(this, other.transform);
        }
    }

    public void SetTrackCheckPoints(TrackCheckPoints trackCheckPoints)
    {
   
        this.trackCheckPoints = trackCheckPoints;
    }
}
