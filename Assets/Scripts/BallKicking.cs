using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKicking : MonoBehaviour {
    private Vector3 lastPos;
    private float speed;

	void Start () {

	}
	
	void Update () {
        speed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
	}

    void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Ball")
        {
            obj.GetComponent<BallControl>().move();
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1.2f * speed, 2f * speed));
        }
    }
}
