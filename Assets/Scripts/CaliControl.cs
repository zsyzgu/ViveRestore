using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CaliControl : ControlledHuman {
    public TimePanelControl timePanelControl;
    public int sceneId = 0;
    public Text moveScreen;
    public Text taskScreen;
    public Text errorScreen;
    public GameObject[] stdSkeletons;
    public GameObject mySkeleton;

    private string[] motions;
    private int[] finished;
    private string fileName;
    private int selectedMotionId = 0;
    private int startIndex;
    private Data.Motion recordedMotion;

    private void initMotions()
    {
        if (sceneId == 1)
        {
            fileName = "soccer";
            motions = new string[1];
            motions[0] = "long_kick_right";
        } else if (sceneId == 2)
        {
            fileName = "fighting";
            motions = new string[6];
            motions[0] = "front_kick_left";
            motions[1] = "front_kick_right";
            motions[2] = "side_kick_left";
            motions[3] = "side_kick_right";
            motions[4] = "knee_lift_left";
            motions[5] = "knee_lift_right";
        } else if (sceneId == 3)
        {
            fileName = "parkour";
            motions = new string[4];
            motions[0] = "jumping";
            motions[1] = "squat";
            motions[2] = "running";
            motions[3] = "walking";
        } else
        {
            Debug.Log("SceneId Error");
        }

        for (int i = 0; i < motions.Length; i++)
        {
            stdSkeletons[i].GetComponent<CaliStdMotion>().loadMotion("Std/" + motions[i] + ".txt");
        }

        finished = new int[motions.Length];
        File.CreateText("Cali/" + fileName + ".txt");
    }

    private void setStdSkeletons()
    {
        for (int i = 0; i < motions.Length; i++)
        {
            stdSkeletons[i].transform.position = new Vector3(1.5f * (i - selectedMotionId), 0f, 3f);
        }
    }

    private void updateSelectedMotion()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedMotionId + 1 < motions.Length)
            {
                selectedMotionId++;
                setStdSkeletons();
                updateTaskScreen();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedMotionId - 1 >= 0)
            {
                selectedMotionId--;
                setStdSkeletons();
                updateTaskScreen();
            }
        }
    }

    private void checkStartMotion()
    {
        if (Utility.isStart())
        {
            if (errorScreen.text != "")
            {
                errorScreen.text = "";
            } else
            {
                startIndex = record.getIndex();
                timePanelControl.startTimeKeeping();
                for (int i = 0; i < motions.Length; i++)
                {
                    stdSkeletons[i].GetComponent<CaliStdMotion>().startMotion();
                }
            }
        }
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

    private void updateTaskScreen()
    {
        taskScreen.text = motions[selectedMotionId] + " (" + finished[selectedMotionId] + "/3)";
    }

    private void checkSaveMotion()
    {
        if (errorScreen.text == "ok")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                mySkeleton.SetActive(true);
                mySkeleton.GetComponent<CaliStdMotion>().loadMotion("Cali/cali.txt");
                mySkeleton.GetComponent<CaliStdMotion>().startMotion(true);
                stdSkeletons[selectedMotionId].GetComponent<CaliStdMotion>().startMotion();
            }
            if (Utility.isRestart())
            {
                recordedMotion.preprocess();
                recordedMotion.output("Cali/" + fileName + ".txt", motions[selectedMotionId], finished[selectedMotionId]);
                finished[selectedMotionId]++;
                updateTaskScreen();
                errorScreen.text = "";
            }
        }
    }

    public void timeOut()
    {
        recordedMotion = new Data.Motion();
        recordedMotion.formMotion(record, startIndex, record.getIndex());
        recordedMotion.ts();
        bool ok = recordedMotion.preprocess();
        if (ok)
        {
            errorScreen.color = Color.green;
            errorScreen.text = "ok";
        } else
        {
            errorScreen.color = Color.red;
            errorScreen.text = "error";
        }
    }

    new void Start()
    {
        base.Start();

        movingDetect.setNeedPressing(false);
        initMotions();
        setStdSkeletons();
        updateTaskScreen();
    }

    new void Update()
    {
        base.Update();
        updateSelectedMotion();
        checkStartMotion();
        updateMoveScreen();
        checkSaveMotion();
    }

    /*private int currMotionId;
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
	}*/
}
