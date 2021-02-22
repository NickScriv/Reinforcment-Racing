using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector]
    public float verticalInput;
    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public bool brakePressed = false;
    [HideInInspector]
    public bool brakeReleased = false;
    [HideInInspector]
    public bool brake = false;
    [HideInInspector]
    public bool reversePressed = false;
    [HideInInspector]
    public bool reverseReleased = false;

    public string stringValue;
    public int indexValue;

    
    private bool lastReverseDownCheck = false;
    private bool lastReverseUpCheck = true;

    bool InputDown(string input)
    {
        return Input.GetAxisRaw("Throttle") == -1;
    }

    public bool GetReverse()
    {
        return InputDown("Throttle");
    }

    public bool GetReversePressed()
    {
        if (InputDown("Throttle"))
        {
            if (InputDown("Throttle") != lastReverseDownCheck)
            {
                lastReverseDownCheck = InputDown("Throttle");
                return true;
            }

        }
        lastReverseDownCheck = InputDown("Throttle");
        return false;
    }

    public bool GetReverseReleased()
    {
        if (!InputDown("Throttle"))
        {
            if (!InputDown("Throttle") != lastReverseUpCheck)
            {
                lastReverseUpCheck = !InputDown("Throttle");
                return true;
            }

        }
        lastReverseUpCheck = !InputDown("Throttle");
        return false;
    }


    private void Update()
    {
        verticalInput = Input.GetAxis("Throttle");
        horizontalInput = Input.GetAxis("Turn");

       

        brakePressed = Input.GetButtonDown("Brake");
        brakeReleased = Input.GetButtonUp("Brake");
        brake = Input.GetButton("Brake");
    }
}
