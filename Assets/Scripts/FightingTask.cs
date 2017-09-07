using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingTask : TaskBase {
    public Image leftFrontKick;
    public Image rightFrontKick;
    public Image leftKneeLift;
    public Image rightKneeLift;
    public Image leftSideKick;
    public Image rightSideKick;
    private bool canHit = true;

    private void generateTasks()
    {
        for (int i = 0; i < REPEAT; i++)
        {
            tasks.Add("front_kick_left");
            tasks.Add("front_kick_right");
            tasks.Add("side_kick_left");
            tasks.Add("side_kick_right");
            tasks.Add("knee_lift_left");
            tasks.Add("knee_lift_right");
        }

        shuffleTasks();
    }
    
    public void setCanHit()
    {
        canHit = true;
    }

    public void hit(string motion)
    {
        if (!canHit)
        {
            return;
        }
        canHit = false;

        if (currTaskId != -1)
        {
            log(currTaskId + ", " + tasks[currTaskId] + ", " + motion);
        }
    }

    private void updatePanels()
    {
        if (currTaskId != -1 && currTaskId < tasks.Count)
        {
            string task = tasks[currTaskId];
            leftFrontKick.color = (task == "front_kick_left") ? Color.red : Color.white;
            rightFrontKick.color = (task == "front_kick_right") ? Color.red : Color.white;
            leftSideKick.color = (task == "side_kick_left") ? Color.red : Color.white;
            rightSideKick.color = (task == "side_kick_right") ? Color.red : Color.white;
            leftKneeLift.color = (task == "knee_lift_left") ? Color.red : Color.white;
            rightKneeLift.color = (task == "knee_lift_right") ? Color.red : Color.white;
        }
    }

    new void Start()
    {
        base.Start();
        generateTasks();
    }

    new void Update () {
        base.Update();
        updatePanels();
	}
}
