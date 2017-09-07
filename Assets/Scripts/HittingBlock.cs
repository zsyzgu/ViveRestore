using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HittingBlock : MonoBehaviour {
    ParkourControl parkourControl;
    public string saveFile;
    private float pulseTime = 0f;
    private StreamWriter sw;
    private int hitCount = -1;

	void Start () {
        parkourControl = transform.parent.GetComponent<ParkourControl>();
        sw = File.CreateText("Data/" + saveFile + "_" + Random.Range(0, 1000000000) + ".txt");
    }
	
	void Update () {
        if (pulseTime > 0f)
        {
            Utility.leftPulse(500);
            Utility.rightPulse(500);
            pulseTime -= Time.deltaTime;
        }
    }

    void log(string str)
    {
        sw.WriteLine(str);
        sw.Flush();
    }

    void OnTriggerEnter(Collider obj)
    {
        string currMotion = parkourControl.getCurrMotion();
        bool hit = false;
        if (obj.name == "BlockJump" && currMotion != "jumping")
        {
            hit = true;
        }
        if (obj.name == "BlockSquat" && currMotion != "squat")
        {
            hit = true;
        }
        if (obj.name == "BlockRun" && currMotion != "running")
        {
            hit = true;
        }
        if (hit)
        {
            pulseTime = 0.5f;
        }

        if (parkourControl.isPractice() == false)
        {
            hitCount++;
            log(hitCount + ", " + obj.name + ", " + currMotion + ", " + !hit);
        }
    }

    void OnDestroy()
    {
        if (sw != null)
        {
            sw.Close();
        }
    }
}
