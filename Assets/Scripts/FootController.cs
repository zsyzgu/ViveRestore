﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FootController : ControlledHuman {
    public string[] motionName;

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private Data.Motion calibratedMotion = null;
    private Data.Motion stdMotion = null;

    private Data.Motion loadStdMotion(string name)
    {
        Data.Motion motion = new Data.Motion();
        string fileName = "Std/" + name + ".txt";
        StreamReader sr = File.OpenText(fileName);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] tags = line.Split(' ');
            motion.readTags(tags);
        }
        motion.preprocess();
        return motion;
    }

    private void loadStdMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadStdMotion(name);
        }
        calibratedMotion = loadStdMotion("calibration");
        stdMotion = stdMotions["long_kick_right"];
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
        }
	}
}
