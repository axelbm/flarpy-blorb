using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public ControllerScript controllerScript;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controllerScript.Jump();
            controllerScript.Continue();
        }

        if (Input.GetMouseButtonDown(0))
        {
            controllerScript.Jump();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            controllerScript.Pause();

    }
}
