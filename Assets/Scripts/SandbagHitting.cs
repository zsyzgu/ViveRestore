﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandbagHitting : MonoBehaviour {
    FightControl fightControl = null;
    private Vector3 lastPos;
    private Vector3 speed;
    private float leftPulseTime = 0f;
    private float leftPulseForce = 0f;
    private float rightPulseTime = 0f;
    private float rightPulseForce = 0f;

    void Start()
    {
        fightControl = transform.parent.GetComponent<FightControl>();
    }

    void Update()
    {
        speed = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
        if (leftPulseTime > 0f)
        {
            Utility.leftPulse((int)leftPulseForce);
            leftPulseTime -= Time.deltaTime;
        }
        if (rightPulseTime > 0f)
        {
            Utility.rightPulse((int)rightPulseForce);
            rightPulseTime -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Sandbag")
        {
            leftPulseTime = rightPulseTime = 0.2f;
            leftPulseForce = rightPulseForce = speed.magnitude * 500f;
            Debug.Log(gameObject.name);
            obj.GetComponent<Rigidbody>().AddForceAtPosition(speed * 100f, obj.transform.position);
            
            fightControl.hitSandbag(speed.magnitude);
        }
    }
}
