using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkourControl : ControlledHuman {
    private const int REPEAT = 5;
    public GameObject environment;
    private int currTaskId = -1;
    private float forward = 0f;

    public bool isPractice()
    {
        return currTaskId == -1;
    }

    private void updateUsualTech()
    {
        if (HmmClient.Action == "")
        {
            if (Utility.leftPress())
            {
                currMotion = "squat";
            }
            if (Utility.rightPress())
            {
                currMotion = "jumping";
            }
        }
    }

    private void checkStart()
    {
        if (currTaskId == -1)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                currTaskId = 0;
                caliSkeleton.gameObject.SetActive(false);
                forward = 0f;
            }
        }
    }

    private void setCircularDtw()
    {
        foreach (string name in motionName)
        {
            if (name == "running" || name == "walking")
            {
                for (int i = 0; i < CALI_NUM; i++)
                {
                    caliMotions[name][i].setCircularDtw(true);
                }
            }
        }
    }

    private void checkSquat()
    {
        if (movingDetect.isMoving())
        {
            int frames = record.getIndex() - movingDetect.getStartIndex();
            float originHeight = record.getXPos(frames).vec[1];
            float height = record.getXPos(0).vec[1];
            if (originHeight - height > 0.1f)
            {
                currMotion = "squat";
            }
        }
    }

    private void updateForward()
    {
        if (currTaskId < REPEAT)
        {
            forward += 2f * Time.deltaTime;

            if (forward > 45f)
            {
                if (currTaskId != -1 && currTaskId != REPEAT)
                {
                    currTaskId++;
                }
                forward = 0f;
            }
            environment.transform.position = new Vector3(0f, 0f, -forward);
        }
    }

    new void Start()
    {
        base.Start();
        checkStart();
        resetCaliMotions();
        setCircularDtw();
        currMotion = "walking";
    }

    new void Update()
    {
        base.Update();
        checkStart();

        updateHMM();
        //updateUsualTech();

        checkSquat();
        if (!movingDetect.isMoving())
        {
            currMotion = "walking";
        }
        
        retrieval();
        updateForward();
    }
}
