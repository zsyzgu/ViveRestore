using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TaskBase : MonoBehaviour
{
    protected const int REPEAT = 10;
    public Text taskScreen;
    public GameObject caliSkeleton;
    public string saveFile;
    protected List<string> tasks = new List<string>();
    protected int currTaskId = -1;
    private StreamWriter sw;
    
    protected void shuffleTasks()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            int j = (int)Random.Range(0f, i + 1.0f - 1e-6f);
            string tmp = tasks[i];
            tasks[i] = tasks[j];
            tasks[j] = tmp;
        }
    }

    private void nextTask()
    {
        currTaskId++;
        if (currTaskId < tasks.Count)
        {
            taskScreen.text = "(" + currTaskId + ")" + tasks[currTaskId];
        }
        else
        {
            currTaskId = tasks.Count;
            taskScreen.text = "Game over";
        }
    }

    private void checkLastTask()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (currTaskId > 0)
            {
                currTaskId--;
                taskScreen.text = "(" + currTaskId + ")" + tasks[currTaskId];
            }
        }
    }

    private void checkStart()
    {
        if (currTaskId == -1)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                nextTask();
                caliSkeleton.SetActive(false);
            }
        }
    }

    protected void log(string str)
    {
        sw.WriteLine(str);
        sw.Flush();
        nextTask();
    }

    protected void Start () {
        sw = File.CreateText("Data/" + saveFile + "_" + Random.Range(0, 1000000000) + ".txt");
    }
	
	protected void Update () {
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
