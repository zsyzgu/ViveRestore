using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FootController : ControlledHuman {
    public string[] motionName;
    public GameObject ball;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Data.Motion calibratedMotion = null;
    private Data.Motion stdMotion = null;

    //private string currMotionName = "side_kick_right";
    private string currMotionName = "long_kick_right";

    private void loadStdMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadStdMotion(name);
        }
        calibratedMotion = loadStdMotion("cali_" + currMotionName);
        stdMotion = stdMotions[currMotionName];
    }

    private const float smoothK = 0.8f;
    private void setLowerBody(Data.Y_POS yPos)
    {
        List<GameObject> objs = new List<GameObject>();
        objs.Add(leftFoot);
        objs.Add(rightFoot);
        objs.Add(leftKnee);
        objs.Add(rightKnee);
        objs.Add(waist);
        int cnt = 0;
        for (int i = 0; i < yPos.N; i += 9)
        {
            objs[cnt].transform.position = objs[cnt].transform.position * smoothK + new Vector3(yPos.vec[i + 0], yPos.vec[i + 1], yPos.vec[i + 2]) * (1 - smoothK);
            objs[cnt].transform.LookAt(new Vector3(yPos.vec[i + 3], yPos.vec[i + 4], yPos.vec[i + 5]));
            cnt++;
        }
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

	void Start () {
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
            ball.transform.position = new Vector3(0f, 0.11f, 0.5f);
        }
	}
}
