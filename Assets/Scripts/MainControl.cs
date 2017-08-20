using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainControl : MonoBehaviour {
    private float speedThreshold = 0.2f;
    DataCollect dataCollect = null;
    Data.X_POS lastXPos = new Data.X_POS();
    float movingTime = 0;
    float stopTime = 0;
    public Text text;

    void Awake()
    {
        dataCollect = GetComponent<DataCollect>();
    }

	void Start () {
		
	}
	
	void Update () {
        Data.X_POS xPos = dataCollect.getXPos();
        float speed = Data.POS.meanDist(xPos, lastXPos) / Time.deltaTime;
        if (speed >= speedThreshold)
        {
            movingTime += Time.deltaTime;
            stopTime = 0f;
            if (movingTime >= 0.1f)
            {
                text.text = "Moving";
            }
        } else
        {
            movingTime = 0f;
            stopTime += Time.deltaTime;
            if (stopTime >= 0.5f)
            {
                text.text = "Stop";
            }
        }
        lastXPos = xPos;
	}
}
