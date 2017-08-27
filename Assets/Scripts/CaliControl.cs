using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CaliControl : ControlledHuman {
    public GameObject stdSkeleton;
    public GameObject canvas;
    public Text moveScreen;
    public Text debugScreen;
    private CaliStdMotion caliStdMotion;
    private TimePanelControl timePanelControl;

    //private string currMotionName = "side_kick_right";
    private string currMotionName = "long_kick_right";

    void Start () {
        caliStdMotion = stdSkeleton.GetComponent<CaliStdMotion>();
        timePanelControl = canvas.GetComponent<TimePanelControl>();
        stdMotion = loadStdMotion(currMotionName);
        caliStdMotion.setMotion(currMotionName);
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
        debugScreen.text = Data.Motion.xPosDistance(currMotion, stdMotion).ToString();
        caliStdMotion.setMotion(currMotionName);
    }
	
	new void Update () {
        base.Update();

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
