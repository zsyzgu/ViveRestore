using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkourControl : ControlledHuman {
    public string[] motionName;
    public Text distScreen;
    public GameObject canvas;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Dictionary<string, Data.Motion> caliMotions = new Dictionary<string, Data.Motion>();
    private string currMotion = "walking";
    private float totalDist = 0f;

    private void loadMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadStdMotion("Std/" + name + ".txt");
            caliMotions[name] = loadCaliMotion("Cali/parkour.txt", name);
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
        if (!moving)
        {
            bool shouldReset = false;
            if (predictFrame > stdMotions[currMotion].timestamp.Count - 1)
            {
                shouldReset = true;
            } else
            {
                if (Data.X_POS.handsDistRelatedToHead(record.getXPos(0), stdMotions[currMotion].xStart) < 0.2f && record.getXPos(0).vec[11] < record.getXPos(1).vec[11])
                {
                    shouldReset = true;
                }
            }
            if (shouldReset)
            {
                for (int i = 0; i < motionName.Length; i++)
                {
                    caliMotions[motionName[i]].resetMotion();
                }
            }
        }

        if (currMotion != "walking")
        {
            forwardSpeed = Mathf.Min(forwardSpeed + 1f * Time.deltaTime, 3.0f);
        } else
        {
            forwardSpeed = Mathf.Max(forwardSpeed - 1f * Time.deltaTime, 1.0f);
        }
        forward += forwardSpeed * Time.deltaTime;
        if (canvas.GetComponent<HPCounter>().gameOver() == false)
        {
            totalDist += forwardSpeed * Time.deltaTime;
            distScreen.text = totalDist.ToString("F2") + " m";
        }
        if (forward > 50f)
        {
            forward = 0f;
        }
        transform.position = new Vector3(0f, 0f, forward);
    }

    public void damage()
    {
        canvas.GetComponent<HPCounter>().demage(0.1f);
    }

    public string getCurrMotion()
    {
        return currMotion;
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
