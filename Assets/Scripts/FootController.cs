using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FootController : ControlledHuman {
    public CaliSkeleton caliSkeleton;
    private List<Data.Motion> caliMotions = new List<Data.Motion>();
    private Data.Motion stdMotion = null;

    private void loadStdMotions()
    {
        stdMotion = loadStdMotion("Std/long_kick_right.txt");
        for (int i = 0; i < CALI_NUM; i++)
        {
            caliMotions.Add(loadCaliMotion("Cali/soccer.txt", "long_kick_right", i));
        }
    }

	new void Start () {
        base.Start();

        loadStdMotions();
	}
	
	new void Update () {
        base.Update();
        
        if (movingDetect.isMoving())
        {
            if (movingDetect.isFirstMove())
            {
                for (int i = 0; i < CALI_NUM; i++)
                {
                    caliMotions[i].resetMotion();
                }
                caliSkeleton.playMotion(caliMotions[0], record.getXPos(0));
            }
            float minScore = 1e9f;
            float predictFrame = 1f;
            for (int i = 0; i < CALI_NUM; i++)
            {
                float score = 0f;
                float frame = caliMotions[i].predictMotionFrame(record, out score);
                if (score < minScore)
                {
                    minScore = score;
                    predictFrame = frame;
                }
            }
            Data.Y_POS predictYPos = stdMotion.getYPos(predictFrame);
            setLowerBody(new Data.Y_POS(predictYPos + stdMotion.yStart));
        } else
        {
            setLowerBody(stdMotion.yStart);
        }
	}
}
