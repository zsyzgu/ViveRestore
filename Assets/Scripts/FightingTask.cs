using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingTask : MonoBehaviour {
    private int REPEAT = 5;
    public Text taskScreen;
    public Image leftFrontKick;
    public Image rightFrontKick;
    public Image leftKneeLift;
    public Image rightKneeLift;
    public Image leftSideKick;
    public Image rightSideKick;
    private bool canHit = true;
    private List<string> tasks = new List<string>();

    private void generateTasks()
    {
        for (int i = 0; i < REPEAT; i++)
        {
            tasks.Add("front_kick_left");
            tasks.Add("front_kick_right");
            tasks.Add("side_kick_left");
            tasks.Add("side_kick_right");
            tasks.Add("knee_lift_left");
            tasks.Add("knee_lift_right");
        }

        for (int i = 0; i < tasks.Count; i++)
        {
            int j = (int)Random.Range(0f, i + 1f - 1e-6f);
            string tmp = tasks[i];
            tasks[i] = tasks[j];
            tasks[j] = tmp;
        }
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

    void Start()
    {

    }

    void Update () {
		
	}
}
