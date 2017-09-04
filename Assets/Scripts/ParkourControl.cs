using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkourControl : ControlledHuman {
    public string[] motionName;
    public Text distScreen;
    public GameObject canvas;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Dictionary<string, List<Data.Motion>> caliMotions = new Dictionary<string, List<Data.Motion>>();
    private string currMotion = "walking";
    private float totalDist = 0f;

    private void resetMotions()
    {
        foreach (string name in motionName)
        {
            for (int i = 0; i < CALI_NUM; i++)
            {
                caliMotions[name][i].resetMotion();
            }
        }
    }

    private void loadMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadStdMotion("Std/" + name + ".txt");
            caliMotions[name] = new List<Data.Motion>();
            for (int i = 0; i < CALI_NUM; i++)
            {
                caliMotions[name].Add(loadCaliMotion("Cali/parkour.txt", name, i));
            }
        }
    }
    
    void updateHMM()
    {
        if (movingDetect.isMoving())
        {
            if (movingDetect.isFirstMove())
            {
                HmmClient.hmmStart();
                resetMotions();
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
        float minScore = 1e9f;
        float predictFrame = 1f;
        foreach(string name in motionName)
        {
            for (int i = 0; i < CALI_NUM; i++)
            {
                float score = 0f;
                float frame = caliMotions[name][i].predictMotionFrame(record, out score);
                if (name == currMotion && score < minScore)
                {
                    minScore = score;
                    predictFrame = frame;
                }
            }
        }
        Data.Y_POS predictYPos = stdMotions[currMotion].getYPos(predictFrame);
        setLowerBody(new Data.Y_POS(predictYPos + stdMotions[currMotion].yStart + new Vector3(head.transform.position.x, 0f, forward)));
        
        if (movingDetect.isMoving() == false)
        {
            bool shouldReset = false;
            if (predictFrame > stdMotions[currMotion].timestamp.Count - 1)
            {
                shouldReset = true;
            } else
            {
                if (Data.X_POS.handsDistToHead(record.getXPos(0), stdMotions[currMotion].xStart) < 0.2f && record.getXPos(0).vec[11] < record.getXPos(1).vec[11])
                {
                    shouldReset = true;
                }
            }
            if (shouldReset)
            {
                resetMotions();
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
        resetMotions();
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
