using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKicking : MonoBehaviour {
    public SoccerTask soccerTask;
    private FootController controller;

	void Start () {
        controller = transform.parent.GetComponent<FootController>();
	}
	
	void Update () {

    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Ball")
        {
            obj.GetComponent<BallControl>().move();
            string currMotion = controller.getCurrMotion();
            soccerTask.setKickMotion(currMotion);
            float speed = controller.getHandSpeed();
            if (currMotion == "long_kick_right")
            {
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 12f * speed, 20f * speed));
            }
            else
            {
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 2f * speed, 20f * speed));
            }
        }
    }
}
