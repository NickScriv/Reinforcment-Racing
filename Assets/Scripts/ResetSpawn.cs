using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSpawn : MonoBehaviour
{
    public float x = 1f;
    public float y = 1f;
    public float z = 1f;
    Vector3 scale;
    public LayerMask SpawnLayerMask;

    private void Start()
    {
        scale.x = x;
        scale.y = y;
        scale.z = z;
    }



    public bool CheckSpawn()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, scale / 2, Quaternion.identity, SpawnLayerMask);
        if(hitColliders.Length > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



}
