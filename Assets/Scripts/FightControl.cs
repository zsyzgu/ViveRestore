using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class FightControl : ControlledHuman {
    public FightingTask fightingTask;

    public void hitSandbag()
    {
        fightingTask.hit(currMotion);
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
            fightingTask.setCanHit();
        }

        retrieval();
    }
}
