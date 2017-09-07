using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FootController : ControlledHuman {
    new void Start () {
        base.Start();
	}

    public float getHandSpeed()
    {
        int frames = record.getIndex() - movingDetect.getStartIndex();
        if (frames > 100)
        {
            frames = 100;
        }
        List<float> speedList = new List<float>();
        
        for (int i = frames; i >= 0; i--)
        {
            Data.X_POS xPos = record.getXSpeedSmooth(i);
            float speed = 0f;
            speed += Mathf.Sqrt(Mathf.Pow(xPos.vec[7], 2f) + Mathf.Pow(xPos.vec[8], 2f) + Mathf.Pow(xPos.vec[9], 2f));
            speed += Mathf.Sqrt(Mathf.Pow(xPos.vec[14], 2f) + Mathf.Pow(xPos.vec[15], 2f) + Mathf.Pow(xPos.vec[16], 2f));
            speed /= 2f;
            speedList.Add(speed);
        }
        speedList.Sort();
        float ret = speedList[(int)(speedList.Count * 0.9f) - 1];

        return ret;
    }
	
	new void Update () {
        base.Update();

        updateHMM();
        retrieval();
	}
}
