using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckSpawn : MonoBehaviour
{
    public LayerMask mask;
 
    public bool CheckSpawnLoc()
    {
    
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 18, mask);
     
        foreach (var hitCollider in hitColliders)
        {
            if((hitCollider.CompareTag("AICar") || hitCollider.CompareTag("Player")) && hitCollider.name != "Collider")
            {
                
                return false;
            }
        }

        return true;
    }






}
