using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class FightControl : ControlledHuman {
    public SandbagStatus sandbag;

    public void hitSandbag(float speed)
    {
        if (movingDetect.isMoving())
        {
            sandbag.hit(currMotion);
        }
    }

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        updateHMM();

        if (!movingDetect.isMoving())
        {
            if (leftHand.transform.position.z > rightHand.transform.position.z)
            {
                currMotion = "side_kick_right";
            }
            else
            {
                currMotion = "side_kick_left";
            }
        }
        
        if (movingDetect.isFirstMove())
        {
            sandbag.setCanHit();
        }

        retrieval();
    }
}
