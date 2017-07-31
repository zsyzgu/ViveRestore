using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TaskManager : MonoBehaviour
{
    static private TaskManager instance;

    public Text taskScreen;
    public RectTransform timePanel;
    public RectTransform timePanelBackground;
    public string[] tasks;
    public int actions;
    public float actionPeriod;

    static private int taskId = 0;
    static private int actionId = 0;
    static private bool recording = false;
    static private float startTime = 0f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        if (recording == true && getEscapeTime() > actionPeriod)
        {
            recording = false;
            actionId++;
            if (actionId == actions * 2)
            {
                actionId = 0;
                taskId++;
            }
        }

        if (recording == false)
        {
            if (taskId == tasks.Length)
            {
                taskScreen.text = "Experiment complete.";
            } else
            {
                taskScreen.text = "Next action : " + tasks[taskId] + " (" + ((actionId % 2 == 0) ? "left" : "right") + " " + actionId / 2 + ")";
            }
        } else
        {
            taskScreen.text = "Recording : " + tasks[taskId] + " (" + ((actionId % 2 == 0) ? "left" : "right") + " " + actionId / 2 + ")";
        }

        float schedule = getEscapeTime() / actionPeriod;
        timePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(timePanelBackground.rect.width * schedule, timePanelBackground.rect.height);
    }

    static private float getEscapeTime()
    {
        if (recording)
        {
            return Time.time - startTime;
        } else
        {
            return 0;
        }
    }

    static public void start()
    {
        if (recording == false && taskId != instance.tasks.Length)
        {
            recording = true;
            startTime = Time.time;
        }
    }
    
    static public void restart()
    {
        if (recording == false && (taskId != 0 || actionId != 0))
        {
            actionId--;
            if (actionId == -1)
            {
                actionId = instance.actions * 2 - 1;
                taskId--;
            }
        }
    }

    static public bool isRecording()
    {
        return recording;
    }

    static public string getTaskInfo()
    {
        return instance.tasks[taskId] + "_" + ((actionId % 2 == 0) ? "left" : "right") + " " + actionId / 2 + " " + getEscapeTime();
    }
}
