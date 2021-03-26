using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
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
        //Debug.Log(transform.name);
        this.trackCheckPoints = trackCheckPoints;
    }
}
