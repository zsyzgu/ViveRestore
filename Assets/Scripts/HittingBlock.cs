using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingBlock : MonoBehaviour {
    ParkourControl parkourControl;
    private float pulseTime = 0f;

	void Start () {
        parkourControl = transform.parent.GetComponent<ParkourControl>();
        if (parkourControl == null)
        {
            parkourControl = transform.parent.parent.GetComponent<ParkourControl>();
        }
	}
	
	void Update () {
        if (pulseTime > 0f)
        {
            Utility.leftPulse(500);
            Utility.rightPulse(500);
            pulseTime -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Block")
        {
            Debug.Log(gameObject.name);
            pulseTime = 0.5f;
            parkourControl.damage();
        }
    }
}
