using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CaliControl : ControlledHuman {
    public GameObject stdSkeleton;
    public GameObject canvas;
    public Text moveScreen;
    private CaliStdMotion caliStdMotion;
    private TimePanelControl timePanelControl;
    public string currMotionName = "long_kick_right";

	void Start () {
        caliStdMotion = stdSkeleton.GetComponent<CaliStdMotion>();
        timePanelControl = canvas.GetComponent<TimePanelControl>();
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

    public void timeOut()
    {
        endIndex = record.getIndex();
        currMotion = new Data.Motion();
        currMotion.formMotion(record, startIndex, endIndex);
    }
	
	new void Update () {
        base.Update();

        if (Utility.isStart(leftHand, rightHand))
        {
            startIndex = record.getIndex();
            timePanelControl.startTimeKeeping();
            caliStdMotion.startMotion(currMotionName);
        }

        if (Utility.isRestart())
        {
            currMotion.output(currMotionName);
        }

        updateMoveScreen();
	}
}
