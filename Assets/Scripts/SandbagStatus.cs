using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandbagStatus : MonoBehaviour {
    public Image leftFist;
    public Image rightFist;
    public Image leftFrontKick;
    public Image rightFrontKick;
    public Image leftKneeLift;
    public Image rightKneeLift;
    public Image leftSideKick;
    public Image rightSideKick;

    void resetColor()
    {
        leftFist.color = Color.white;
        rightFist.color = Color.white;
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
	
    public void hit(string motion, float force)
    {
        Image image = null;
        if (motion == "fist_left")
        {
            image = leftFist;
        }
        if (motion == "fist_right")
        {
            image = rightFist;
        }
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
            image.color = new Color(Mathf.Max(0f, image.color.r - force / 50f), Mathf.Max(0f, image.color.g - force / 50f), 1f);
        }
    }

	void Update () {
		
	}
}
