using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKicking : MonoBehaviour {
    public SoccerTask soccerTask;
    public GameObject leftHand;
    public GameObject rightHand;
    private const int N = 20;
    private Vector3 lastPosLeft;
    private Vector3 lastPosRight;
    private int id = 0;
    private float[] speeds = new float[N];
    private ControlledHuman controlledHuman;

	void Start () {
        controlledHuman = transform.parent.GetComponent<ControlledHuman>();
	}
	
	void Update () {
        speeds[id++] = ((leftHand.transform.position - lastPosLeft).magnitude + (rightHand.transform.position - lastPosRight).magnitude) / 2f / Time.deltaTime;
        if (id == N)
        {
            id = 0;
        }
        lastPosLeft = leftHand.transform.position;
        lastPosRight = rightHand.transform.position;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Ball")
        {
            float speed = 0;
            for (int i = 0; i < N; i++)
            {
                speed += speeds[i];
            }
            speed /= N;
            Debug.Log(speed);
            obj.GetComponent<BallControl>().move();
            string currMotion = controlledHuman.getCurrMotion();
            soccerTask.setKickMotion(currMotion);
            if (currMotion == "long_kick_right")
            {
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 20f * speed, 30f * speed));
            }
            else
            {
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 2f * speed, 30f * speed));
            }
        }
    }
}
