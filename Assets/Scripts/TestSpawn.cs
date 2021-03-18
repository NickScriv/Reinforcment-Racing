using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    RCC_CarControllerV3 script;
    public float AIThrottle = 0f;
    public float AIBrake = 0f;
    public float AISteer = 0f;

    private void Start()
    {
        script = GetComponent<RCC_CarControllerV3>();
    }
    public Transform car;
    // Update is called once per frame
    void Update()
    {
        AIThrottle = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerRightInput);
        AIBrake = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerLeftInput);
        AISteer = Input.GetAxis(RCC_Settings.Instance.Xbox_horizontalInput);

    }
}
