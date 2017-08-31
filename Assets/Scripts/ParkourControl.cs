using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourControl : ControlledHuman {
    public string[] motionName;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Dictionary<string, Data.Motion> caliMotions = new Dictionary<string, Data.Motion>();
    private string currMotion = "walking";

    private void loadMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadMotion("Std/", name);
            caliMotions[name] = loadMotion("Cali/", name);
        }
    }

    private bool firstMove = true;
    private float firstMoveTime = 0f;
    void updateHMM()
    {
        bool moving = movingDetect.isMoving();
        if (moving)
        {
            if (firstMove)
            {
                HmmClient.hmmStart();
                firstMove = false;
                firstMoveTime = Time.time;

                for (int i = 0; i < motionName.Length; i++)
                {
                    caliMotions[motionName[i]].resetMotion();
                }
            }
            if (Time.time - firstMoveTime <= 0.2f)
            {
                HmmClient.newFrame(new Data.X_POS((record.getXPos(0) - record.getXPos(1)) / (record.getTimestamp(0) - record.getTimestamp(1))).getHandsVector());
                HmmClient.getAction();
                if (HmmClient.Action != "")
                {
                    currMotion = HmmClient.Action;
                }
            }
        }
        else
        {
            firstMove = true;

            if ((leftHand.transform.position.y + rightHand.transform.position.y) / 2 > waist.transform.position.y)
            {
                currMotion = "running";
            }
            else
            {
                currMotion = "walking";
            }
        }
    }

    private float forwardSpeed = 0.2f;
    private float forward = 0f;
    void retrieval()
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
        setLowerBody(new Data.Y_POS(predictYPos + stdMotions[currMotion].yStart + new Vector3(head.transform.position.x, 0f, forward)));
        
        bool moving = movingDetect.isMoving();
        if (!moving && predictFrame > 200f)
        {
            for (int i = 0; i < motionName.Length; i++)
            {
                caliMotions[motionName[i]].resetMotion();
            }
        }

        if (currMotion == "running")
        {
            forwardSpeed = Mathf.Min(forwardSpeed + 1f * Time.deltaTime, 3.0f);
        } else
        {
            forwardSpeed = Mathf.Max(forwardSpeed - 2f * Time.deltaTime, 0.5f);
        }
        forward += forwardSpeed * Time.deltaTime;
        if (forward > 50f)
        {
            forward = 0f;
        }
        transform.position = new Vector3(0f, 0f, forward);
    }

    new void Start()
    {
        base.Start();

        loadMotions();
        for (int i = 0; i < motionName.Length; i++)
        {
            caliMotions[motionName[i]].resetMotion();
        }
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
