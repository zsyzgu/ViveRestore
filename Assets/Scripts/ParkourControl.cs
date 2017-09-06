using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkourControl : ControlledHuman {
    public GameObject environment;
    public HPCounter hpCounter;
    private float forward = 0f;

    public void damage()
    {
        hpCounter.demage(0.1f);
    }

    private void setCircularDtw()
    {
        foreach (string name in motionName)
        {
            if (name == "running" || name == "walking")
            {
                for (int i = 0; i < CALI_NUM; i++)
                {
                    caliMotions[name][i].setCircularDtw(true);
                }
            }
        }
    }

    private void updateForward()
    {
        if (hpCounter.gameOver() == false)
        {
            forward += 2f * Time.deltaTime;

            if (forward > 45f)
            {
                forward = 0f;
            }
            environment.transform.position = new Vector3(0f, 0f, -forward);
        }
    }

    new void Start()
    {
        base.Start();
        resetCaliMotions();
        setCircularDtw();
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
