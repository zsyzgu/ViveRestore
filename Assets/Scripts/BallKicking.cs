using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKicking : MonoBehaviour {
    FootController footController = null;

	void Start () {
        footController = transform.parent.GetComponent<FootController>();
	}
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Ball")
        {
            float speed = footController.calnRightFootSpeed();
            obj.GetComponent<BallControl>().move();
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1.2f * speed, 2f * speed));
        }
    }
}
