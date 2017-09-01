using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CaliControl : ControlledHuman {
    public GameObject stdSkeleton;
    public GameObject canvas;
    public Text moveScreen;
    public Text motionScreen;
    public Text errorScreen;
    public Text taskScreen;
    public string[] motionNames;
    private CaliStdMotion caliStdMotion;
    private TimePanelControl timePanelControl;

    private int currMotionId;
    private string currMotionName;
    private float error = 0f;

    new void Start () {
        base.Start();

        caliStdMotion = stdSkeleton.GetComponent<CaliStdMotion>();
        timePanelControl = canvas.GetComponent<TimePanelControl>();
        currMotionId = 0;
        setStdMotion();
        File.CreateText("Cali/train.txt");
    }

    private void updateMoveScreen()
    {
        if (movingDetect.isMoving())
        {
            moveScreen.text = "Moving";
        }
        else
        {
            moveScreen.text = "Stop";
        }
    }

    private int startIndex = 0;
    private int endIndex = 0;
    private Data.Motion currMotion;
    private Data.Motion stdMotion;

    public void timeOut()
    {
        endIndex = record.getIndex();
        currMotion = new Data.Motion();
        string error = currMotion.formMotion(record, startIndex, endIndex);
        //error = Data.Motion.xPosDistance(currMotion, stdMotion);
        if (error == "ok")
        {
            errorScreen.color = Color.green;
        } else
        {
            errorScreen.color = Color.red;
        }
        errorScreen.text = error;
        caliStdMotion.setMotion(currMotionName);
    }

    private void setStdMotion()
    {
        currMotionName = motionNames[currMotionId];
        stdMotion = loadMotion("Std/", currMotionName);
        caliStdMotion.setMotion(currMotionName);
        motionScreen.text = currMotionName;
        motionId = 0;
        minError = 1e9f;
        taskScreen.text = "Complete " + motionId.ToString() + " motions";
    }

    private int motionId = 0;
    private float minError = 1e9f;

    private void updateMotionName()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && currMotionId - 1 >= 0)
        {
            currMotionId--;
            setStdMotion();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && currMotionId + 1 < motionNames.Length)
        {
            currMotionId++;
            setStdMotion();
        }
    }

	new void Update () {
        base.Update();

        updateMotionName();

        if (Utility.isStart())
        {
            startIndex = record.getIndex();
            timePanelControl.startTimeKeeping();
            caliStdMotion.startMotion();
            errorScreen.text = "";
        }

        if (Utility.isRestart() && errorScreen.text == "ok")
        {
            if (error < minError)
            {
                minError = error;
                currMotion.output("Cali/", currMotionName);
            }
            currMotion.output("Cali/", currMotionName, motionId);
            motionId++;
            taskScreen.text = "Complete " + motionId.ToString() + " motions";
        }

        updateMoveScreen();
	}
}
