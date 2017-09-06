using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SoccerTask : TaskBase
{
    private string kickMotion;
    private float kickDistance;

    void generateTasks()
    {
        for (int i = 0; i < REPEAT; i++)
        {
            tasks.Add("long_kick_right" + ", " + "high_strength");
            tasks.Add("long_kick_right" + ", " + "low_strength");
            tasks.Add("inner_kick_right" + ", " + "high_strength");
            tasks.Add("inner_kick_right" + ", " + "low_strength");
        }

        shuffleTasks();
    }

    public void setKickMotion(string motion)
    {
        kickMotion = motion;
    }

    public void setKickDistance(float distance)
    {
        kickDistance = distance;
        if (currTaskId != -1)
        {
            log(currTaskId + ", " + tasks[currTaskId] + ", " + kickMotion + ", " + distance);
        }
    }

	new void Start () {
        base.Start();
        generateTasks();
	}
	
	new void Update () {
        base.Update();
	}


}
