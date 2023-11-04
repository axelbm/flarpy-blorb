using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public BirdScript birdScript;
    public LogicScript logicScript;

    private string mode = "game";

    public string Mode
    {
        get { return mode; }
        set
        {
            if (mode == value)
                return;

            mode = value;
            lastModeChangeTime = Time.realtimeSinceStartup;
        }
    }

    private float lastModeChangeTime = 0;

    public void Jump()
    {
        if (mode == "game")
        {
            birdScript.Flap();
        }

        else if (mode == "sleep")
        {
            logicScript.AwakeGame();
            birdScript.Flap();
        }
    }

    public void Continue()
    {
        if (mode == "gameOver")
        {
            if (Time.realtimeSinceStartup - lastModeChangeTime > logicScript.timeBeforeRestart)
                logicScript.RestartGame();
        }
    }

    public void Pause()
    {
        if (mode == "game")
            logicScript.PauseGame();

        else if (mode == "pause")
            logicScript.ResumeGame();
    }
}
