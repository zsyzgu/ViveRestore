using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaliSkeleton : MonoBehaviour {
    public GameObject leftHand;
    public GameObject rightHand;
    private Data.Motion motion;
    private Data.X_POS xStart;
    private int t = -1;

    public void playMotion(Data.Motion motion, Data.X_POS xStart)
    {
        this.motion = motion;
        this.xStart = xStart;
        t = 0;
    }

	void Start () {
		
	}
	
	void Update () {
        if (t == -1)
        {
            return;
        }

        Data.X_POS xPos = new Data.X_POS(motion.xPos[t] + xStart);
        leftHand.transform.position = new Vector3(xPos.vec[7], xPos.vec[8], xPos.vec[9]);
        leftHand.transform.rotation = new Quaternion(xPos.vec[10], xPos.vec[11], xPos.vec[12], xPos.vec[13]);
        rightHand.transform.position = new Vector3(xPos.vec[14], xPos.vec[15], xPos.vec[16]);
        rightHand.transform.rotation = new Quaternion(xPos.vec[17], xPos.vec[18], xPos.vec[19], xPos.vec[20]);

        t++;
        if (t >= motion.timestamp.Count)
        {
            t = -1;
        }
	}
}
