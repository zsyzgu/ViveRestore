using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkourControl : ControlledHuman {
    public HPCounter hpCounter;

    private float forwardSpeed = 0.2f;
    private float forward = 0f;

    public void damage()
    {
        hpCounter.demage(0.1f);
    }

    private void updateForward()
    {
        if (currMotion != "walking")
        {
            forwardSpeed = Mathf.Min(forwardSpeed + 1f * Time.deltaTime, 3.0f);
        }
        else
        {
            forwardSpeed = Mathf.Max(forwardSpeed - 1f * Time.deltaTime, 1.0f);
        }
        forward += forwardSpeed * Time.deltaTime;

        if (forward > 50f)
        {
            forward = 0f;
        }
        transform.position = new Vector3(0f, 0f, forward);
    }

    new void Start()
    {
        base.Start();
        resetCaliMotions();
        currMotion = "walking";
    }

    new void Update()
    {
        base.Update();

        updateHMM();

        if (!movingDetect.isMoving())
        {
            currMotion = "walking";
        }

        retrieval();
        updateForward();
    }
}
