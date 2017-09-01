﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class FightControl : ControlledHuman {
    public string[] motionName;
    public Text motionScreen;
    public GameObject canvas;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Dictionary<string, Data.Motion> caliMotions = new Dictionary<string, Data.Motion>();
    private string currMotion = "side_kick_right";

    private void loadMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadMotion("Std/", name);
            caliMotions[name] = loadMotion("Cali/", name);
        }
    }

    private bool firstMove = true;
    void updateHMM()
    {
        bool moving = movingDetect.isMoving();
        if (moving)
        {
            if (firstMove)
            {
                HmmClient.hmmStart();
                firstMove = false;
                
                for (int i = 0; i < motionName.Length; i++)
                {
                    caliMotions[motionName[i]].resetMotion();
                }
            }
            HmmClient.newFrame(new Data.X_POS((record.getXPos(0) - record.getXPos(1)) / (record.getTimestamp(0) - record.getTimestamp(1))).getHandsVector());
            HmmClient.getAction();
            if (HmmClient.Action != "")
            {
                currMotion = HmmClient.Action;
            }
        }
        else
        {
            firstMove = true;

            if (leftHand.transform.position.z > rightHand.transform.position.z)
            {
                currMotion = "side_kick_right";
            } else
            {
                currMotion = "side_kick_left";
            }
        }

        //motionScreen.text = currMotion;
    }

    void retrieval()
    {
        bool moving = movingDetect.isMoving();

        if (moving)
        {
            float predictFrame = 1f;
            for (int i = 0; i < motionName.Length; i++)
            {
                float t = caliMotions[motionName[i]].predictMotionFrame(record);
                if (motionName[i] == currMotion)
                {
                    predictFrame = t;
                }
            }
            Data.Y_POS predictYPos = stdMotions[currMotion].getYPos(predictFrame);
            setLowerBody(new Data.Y_POS(predictYPos + stdMotions[currMotion].yStart));
        } else
        {
            setLowerBody(stdMotions[currMotion].yStart);
        }
    }

    public void hitSandbag(float speed)
    {
        bool moving = movingDetect.isMoving();

        string motion = "";
        if (moving)
        {
            motion = currMotion;
        } else
        {
            if (currMotion == "side_kick_left")
            {
                motion = "fist_right";
            } else
            {
                motion = "fist_left";
            }
        }

        canvas.GetComponent<SandbagStatus>().hit(motion, speed);
    }

    new void Start()
    {
        base.Start();

        loadMotions();
    }

    new void Update()
    {
        base.Update();

        updateHMM();
        retrieval();
    }

    void OnDestroy()
    {
        Net.closeSocket();
    }
}
