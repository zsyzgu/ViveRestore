using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandbagStatus : MonoBehaviour {
    public Image leftFrontKick;
    public Image rightFrontKick;
    public Image leftKneeLift;
    public Image rightKneeLift;
    public Image leftSideKick;
    public Image rightSideKick;
    private bool canHit = true;

    void resetColor()
    {
        leftFrontKick.color = Color.white;
        rightFrontKick.color = Color.white;
        leftKneeLift.color = Color.white;
        rightKneeLift.color = Color.white;
        leftSideKick.color = Color.white;
        rightSideKick.color = Color.white;
    }

	void Start () {
        resetColor();
	}

    public void setCanHit()
    {
        canHit = true;
    }

    public void hit(string motion)
    {
        if (!canHit)
        {
            return;
        }
        canHit = false;
        Image image = null;
        if (motion == "front_kick_left")
        {
            image = leftFrontKick;
        }
        if (motion == "front_kick_right")
        {
            image = rightFrontKick;
        }
        if (motion == "knee_lift_left")
        {
            image = leftKneeLift;
        }
        if (motion == "knee_lift_right")
        {
            image = rightKneeLift;
        }
        if (motion == "side_kick_left")
        {
            image = leftSideKick;
        }
        if (motion == "side_kick_right")
        {
            image = rightSideKick;
        }
        if (image != null)
        {
            image.color = new Color(Mathf.Max(0f, image.color.r - 0.2f), Mathf.Max(0f, image.color.g - 0.2f), 1f);
        }
    }

	void Update () {
		
	}
}
