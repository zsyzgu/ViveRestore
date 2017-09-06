using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SoccerTask : MonoBehaviour {
    private const int REPEAT = 5;
    public Text taskScreen;
    public GameObject caliSkeleton;
    private bool practice = true;
    private List<string> tasks = new List<string>();
    private int currTaskId = -1;
    private string kickMotion;
    private float kickDistance;
    private StreamWriter sw;

    private void checkStart()
    {
        if (practice == true)
        {
            if (Input.GetKey(KeyCode.S))
            {
                practice = false;
                nextTask();
                caliSkeleton.SetActive(false);
            }
        }
    }

    private void checkLastTask()
    {
        if (Input.GetKey(KeyCode.B))
        {
            lastTask();
        }
    }

    private void generateTasks()
    {
        for (int i = 0; i < REPEAT; i++)
        {
            tasks.Add("long_kick_right" + ", " + "high_strength");
            tasks.Add("long_kick_right" + ", " + "low_strength");
            tasks.Add("inner_kick_right" + ", " + "high_strength");
            tasks.Add("inner_kick_right" + ", " + "low_strength");
        }
        
        for (int i = 0; i < tasks.Count; i++)
        {
            int j = (int)Random.Range(0f, i + 1.0f - 1e-6f);
            Debug.Log(i + ", " + j);
            string tmp = tasks[i];
            tasks[i] = tasks[j];
            tasks[j] = tmp;
        }
    }

    private void nextTask()
    {
        if (currTaskId < tasks.Count)
        {
            currTaskId++;
            taskScreen.text = tasks[currTaskId];
        } else
        {
            taskScreen.text = "Game over";
        }
    }

    private void lastTask()
    {
        if (currTaskId > 0)
        {
            currTaskId--;
            taskScreen.text = tasks[currTaskId];
        }
    }

    public void setKickMotion(string motion)
    {
        kickMotion = motion;
    }

    public void setKickDistance(float distance)
    {
        kickDistance = distance;
        if (currTaskId != -1)
        {
            sw.WriteLine(currTaskId + ", " + tasks[currTaskId] + ", " + kickMotion + ", " + distance);
            sw.Flush();
            nextTask();
        }
    }

	void Start () {
        generateTasks();
        sw = File.CreateText("Data/soccer_" + Random.Range(0, 1000000000) + ".txt");
	}
	
	void Update () {
        checkStart();
        checkLastTask();
	}

    void OnDestroy()
    {
        if (sw != null)
        {
            sw.Close();
        }
    }
}
