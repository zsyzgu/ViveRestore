using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TaskManager : MonoBehaviour
{
    static private TaskManager instance;

    public Text taskScreen;
    public Text errorScreen;
    public RectTransform timePanel;
    public RectTransform timePanelBackground;
    public string[] tasks;
    public int actions;
    public float actionPeriod;
    public GameObject skeleton;
    public GameObject soccer;
    public GameObject sandbag;

    static private int taskId = 12;
    static private int actionId = 0;
    static private bool recording = false;
    static private bool rest = true;
    static private float startTime = 0f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void updateEnvironment()
    {
        if (taskId < 6)
        {
            soccer.SetActive(true);
            sandbag.SetActive(false);
        } else if (taskId < 12)
        {
            soccer.SetActive(false);
            sandbag.SetActive(true);
        } else if (taskId < 14)
        {
            soccer.SetActive(false);
            sandbag.SetActive(false);
        } else
        {
            soccer.SetActive(false);
            sandbag.SetActive(false);
            actionPeriod = 20;
            actions = 1;
        }
    }

    void Update()
    {   
        updateEnvironment();

        if (rest && taskId < tasks.Length && Input.GetKey(KeyCode.S))
        {
            rest = false;
        }

        if (recording == true && getEscapeTime() > actionPeriod)
        {
            recording = false;
        }

        if (recording == false)
        {
            if (rest)
            {
                if (taskId == tasks.Length)
                {
                    taskScreen.text = "Experiment complete.";
                } else
                {
                    taskScreen.text = "Calibration: " + tasks[taskId];
                    skeleton.SetActive(true);
                    skeleton.GetComponent<Show>().setMotion(tasks[taskId]);
                }
            } else
            {
                skeleton.SetActive(false);
                taskScreen.text = "Next action: " + tasks[taskId] + " (" + actionId + ")";
            }
        } else
        {
            skeleton.SetActive(false);
            taskScreen.text = "Recording: " + tasks[taskId] + " (" + actionId + ")";
        }

        float schedule = getEscapeTime() / actionPeriod;
        timePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(timePanelBackground.rect.width * schedule, timePanelBackground.rect.height);
    }

    static public float getEscapeTime()
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
        if (rest == true)
        {

        }
        if (rest == false && recording == false && taskId != instance.tasks.Length)
        {
            if (instance.errorScreen.text == "")
            {
                recording = true;
                startTime = Time.time;
            } else
            {
                if (instance.errorScreen.text == "ok")
                {
                    actionId++;
                }
                instance.errorScreen.text = "";
                if (actionId == instance.actions)
                {
                    actionId = 0;
                    taskId++;
                    rest = true;
                }
            }
        }
    }
    
    static public void restart()
    {
        if (recording == false && (taskId != 0 || actionId != 0))
        {
            actionId--;
            if (actionId == -1)
            {
                actionId = 0;
            }
        }
    }

    static public bool isRecording()
    {
        return recording;
    }

    static public bool isRest()
    {
        return rest;
    }

    static public bool isOneAction()
    {
        return instance.actions == 1;
    }

    static public float getActionPeriod()
    {
        return instance.actionPeriod;
    }

    static public string getTaskInfo()
    {
        return instance.tasks[taskId] + " " + actionId + " " + getEscapeTime();
    }
}
