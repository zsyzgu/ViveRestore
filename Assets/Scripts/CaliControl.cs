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
    public string[] motionNames;
    private CaliStdMotion caliStdMotion;
    private TimePanelControl timePanelControl;

    private int currMotionId;
    private string currMotionName;

    void Start () {
        caliStdMotion = stdSkeleton.GetComponent<CaliStdMotion>();
        timePanelControl = canvas.GetComponent<TimePanelControl>();
        currMotionId = 0;
        setStdMotion();
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
        currMotion.formMotion(record, startIndex, endIndex);
        //debugScreen.text = Data.Motion.xPosDistance(currMotion, stdMotion).ToString();
        caliStdMotion.setMotion(currMotionName);
    }

    private void setStdMotion()
    {
        currMotionName = motionNames[currMotionId];
        stdMotion = loadStdMotion(currMotionName);
        caliStdMotion.setMotion(currMotionName);
        motionScreen.text = currMotionName;
    }
	
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

        if (Utility.isStart(leftHand, rightHand))
        {
            startIndex = record.getIndex();
            timePanelControl.startTimeKeeping();
            caliStdMotion.startMotion();
        }

        if (Utility.isRestart())
        {
            currMotion.output(currMotionName);
        }

        updateMoveScreen();
	}
}
