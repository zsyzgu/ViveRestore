using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class FightControl : ControlledHuman {
    public string[] motionName;
    public Text motionScreen;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Dictionary<string, Data.Motion> caliMotions = new Dictionary<string, Data.Motion>();

    private void loadMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadStdMotion(name);
            caliMotions[name] = loadStdMotion("cali_" + name);
        }
    }

    new void Start()
    {
        base.Start();

        loadMotions();
    }

    private bool moved = false;
    new void Update()
    {
        base.Update();

        bool moving = movingDetect.isMoving();
        if (moving)
        {
            if (moved == false)
            {
                HmmClient.hmmStart();
            }
            moved = true;
            HmmClient.newFrame(new Data.X_POS((record.getXPos(0) - record.getXPos(1)) / (record.getTimestamp(0) - record.getTimestamp(1))).getHandsVector());
            HmmClient.getAction();
            motionScreen.text = HmmClient.Action;
        } else
        {
            moved = false;
        }
        
        if (motionScreen.text != "")
        {
            if (moving)
            {
                float predictFrame = caliMotions[HmmClient.Action].predictMotionFrame(record);
                Data.Y_POS predictYPos = stdMotions[HmmClient.Action].getYPos(predictFrame);
                setLowerBody(new Data.Y_POS(predictYPos + stdMotions[HmmClient.Action].yStart));
            }
            else
            {
                caliMotions[HmmClient.Action].resetMotion();
                setLowerBody(stdMotions[HmmClient.Action].yStart);
            }
        }
    }
}
