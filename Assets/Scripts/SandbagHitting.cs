using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandbagHitting : MonoBehaviour {
    FightControl fightControl = null;
    private float pulseTime = 0f;
    private float pulseForce = 0f;
    private Vector3 lastPos;
    private Vector3 speed;

    void Start()
    {
        fightControl = transform.parent.GetComponent<FightControl>();
    }

    void Update()
    {
        speed = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;

        if (pulseTime > 0f)
        {
            Utility.leftPulse((int)pulseForce);
            Utility.rightPulse((int)pulseForce);
            pulseTime -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Sandbag")
        {
            pulseTime = 0.2f;
            pulseForce = speed.magnitude * 250f;
            
            obj.GetComponent<Rigidbody>().AddForceAtPosition(speed * 100f, obj.transform.position);
            fightControl.hitSandbag();
        }
    }
}
