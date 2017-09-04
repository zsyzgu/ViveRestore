using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class FightControl : ControlledHuman {
    public string[] motionName;
    public Text motionScreen;
    public SandbagStatus sandbag;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Dictionary<string, List<Data.Motion>> caliMotions = new Dictionary<string, List<Data.Motion>>();
    private string currMotion = "side_kick_right";

    private void loadMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadStdMotion("Std/" + name + ".txt");
            caliMotions[name] = new List<Data.Motion>();
            for (int i = 0; i < CALI_NUM; i++)
            {
                caliMotions[name].Add(loadCaliMotion("Cali/fighting.txt", name, i));
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
                
                foreach (string name in motionName)
                {
                    for (int i = 0; i < CALI_NUM; i++)
                    {
                        caliMotions[name][i].resetMotion();
                    }
                }

                sandbag.setCanHit();
            }
            HmmClient.newFrame(new Data.X_POS((record.getXPos(0) - record.getXPos(1)) / (record.getTimestamp(0) - record.getTimestamp(1))).getHandsVector());
            HmmClient.getAction();
            if (HmmClient.Action != "")
            {
                currMotion = HmmClient.Action;
                Debug.Log(currMotion);
            }
        }
        else
        {
            if (leftHand.transform.position.z > rightHand.transform.position.z)
            {
                currMotion = "side_kick_right";
            } else
            {
                currMotion = "side_kick_left";
            }
        }
    }

    void retrieval()
    {
        if (movingDetect.isMoving())
        {
            float minScore = 1e9f;
            float predictFrame = 1f;
            foreach (string name in motionName)
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
            setLowerBody(new Data.Y_POS(predictYPos + stdMotions[currMotion].yStart));
        } else
        {
            setLowerBody(stdMotions[currMotion].yStart);
        }
    }

    public void hitSandbag(float speed)
    {
        if (movingDetect.isMoving())
        {
            sandbag.hit(currMotion);
        }

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
