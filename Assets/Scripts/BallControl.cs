using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControl : MonoBehaviour {
    public Text scoreText;
    private Vector3 originPos;
    private Vector3 lastPos;
    private bool stop = true;
    private int stopFrame = 0;

    public void move()
    {
        stop = false;
    }

	void Start () {
        originPos = transform.position;
	}
	
	void Update () {
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
                stop = true;
            }
            lastPos = transform.position;
        }
        scoreText.text = Vector3.Distance(originPos, transform.position).ToString() + " m";
	}
}
