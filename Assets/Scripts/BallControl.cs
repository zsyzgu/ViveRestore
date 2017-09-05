using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControl : MonoBehaviour {
    public Text scoreText;
    public SoccerTask soccerTask;
    private Vector3 originPos;
    private Vector3 lastPos;
    private bool stop = true;
    private int stopFrame = 0;

    public void move()
    {
        stop = false;
        stopFrame = 0;
    }

	void Start () {
        originPos = transform.position;
	}
	
	void Update () {
        float dist = Vector3.Distance(originPos, transform.position);
        scoreText.text = dist.ToString("F2") + " m";
        if (stop)
        {
            lastPos = transform.position = originPos;
            transform.eulerAngles = new Vector3();
        } else
        {
            float speed = Vector3.Distance(transform.position, lastPos) / Time.deltaTime;
            if (speed <= 0.5f)
            {
                stopFrame++;
            } else
            {
                stopFrame = 0;
            }
            if (stopFrame >= 10)
            {
                if (!stop)
                {
                    stop = true;
                    soccerTask.setKickDistance(dist);
                }
            }
            lastPos = transform.position;
        }
	}
}
