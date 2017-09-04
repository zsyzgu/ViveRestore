using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKicking : MonoBehaviour {
    private const int N = 10;
    private Vector3 lastPos;
    private int id = 0;
    private float[] speeds = new float[N];

	void Start () {

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
            Debug.Log(speed);
            obj.GetComponent<BallControl>().move();
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f * speed * speed, 10f * speed * speed));
        }
    }
}
