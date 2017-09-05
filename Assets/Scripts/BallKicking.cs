using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKicking : MonoBehaviour {
    private const int N = 10;
    private Vector3 lastPos;
    private int id = 0;
    private float[] speeds = new float[N];
    private ControlledHuman controlledHuman;

	void Start () {
        controlledHuman = transform.parent.GetComponent<ControlledHuman>();
	}
	
	void Update () {
        speeds[id++] = (transform.position - lastPos).magnitude / Time.deltaTime;
        if (id == N)
        {
            id = 0;
        }
        lastPos = transform.position;
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
            obj.GetComponent<BallControl>().move();
            if (controlledHuman.getCurrMotion() == "long_kick_right")
            {
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f * speed, 20f * speed));
            }
            else
            {
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 2f * speed, 15f * speed));
            }
        }
    }
}
