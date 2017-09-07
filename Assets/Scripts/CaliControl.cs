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
    private int randomNumber = 0;

    private void initMotions()
    {
        if (sceneId == 1)
        {
            fileName = "soccer";
            motions = new string[2];
            motions[0] = "inner_kick_right";
            motions[1] = "long_kick_right";
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
        File.CreateText("Cali/" + fileName + "_" + randomNumber + ".txt");
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
                recordedMotion.output("Cali/" + fileName + "_" + randomNumber + ".txt", motions[selectedMotionId], finished[selectedMotionId]);
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
        int startIndexTmp = -1, endIndexTmp = -1;
        bool ok = recordedMotion.segmentCheck(out startIndexTmp, out endIndexTmp);
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
        randomNumber = Random.Range(0, 1000000000);

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
}
