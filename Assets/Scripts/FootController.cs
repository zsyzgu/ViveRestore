using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FootController : ControlledHuman {
    public string[] motionName;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Data.Motion calibratedMotion = null;
    private Data.Motion stdMotion = null;
    
    private string currMotionName = "long_kick_right";

    private void loadStdMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadMotion("Std/", name);
        }
        calibratedMotion = loadMotion("Cali/", currMotionName);
        stdMotion = stdMotions[currMotionName];
    }

    public float calnRightFootSpeed()
    {
        if (record.getIndex() < 1)
        {
            return 0f;
        }
        Data.Y_POS yPos = new Data.Y_POS((record.getYPos(0) - record.getYPos(1)) / (record.getTimestamp(0) - record.getTimestamp(1)));
        float speed = yPos.vec[9] * yPos.vec[9] + yPos.vec[10] * yPos.vec[10] + yPos.vec[11] * yPos.vec[11];
        return speed;
    }

	new void Start () {
        base.Start();

        loadStdMotions();
	}
	
	new void Update () {
        base.Update();

        bool moving = movingDetect.isMoving();
        if (moving)
        {
            float predictFrame = calibratedMotion.predictMotionFrame(record);
            Data.Y_POS predictYPos = stdMotion.getYPos(predictFrame);
            setLowerBody(new Data.Y_POS(predictYPos + stdMotion.yStart));
        } else
        {
            calibratedMotion.resetMotion();
            setLowerBody(stdMotion.yStart);
        }
	}
}
