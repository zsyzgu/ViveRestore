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

    private void updateUsualTech()
    {
        if (HmmClient.Action == "")
        {
            if (Utility.leftPress())
            {
                currMotion = "side_kick_left";
            }
            if (Utility.rightPress())
            {
                currMotion = "side_kick_right";
            }
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
        
        updateUsualTech();
        
        if (movingDetect.isFirstMove())
        {
            fightingTask.setCanHit();
        }

        retrieval();
    }
}
