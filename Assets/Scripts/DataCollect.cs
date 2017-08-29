using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataCollect : MonoBehaviour
{
    public GameObject[] objects;
    public Text moveScreen;
    public Text homeScreen;
    public Text errorScreen;
    
    void Start()
    {
        Utility.leftHand = objects[1];
        Utility.rightHand = objects[2];
    }

    string getDataInfo()
    {
        string info = "";

        for (int i = 0; i < objects.Length; i++)
        {
            Vector3 position = new Vector3();
            Vector3 F = new Vector3();
            Vector3 U = new Vector3();

            if (objects[i] != null)
            {
                position = objects[i].transform.position;
                F = position + objects[i].transform.forward * 0.1f;
                U = position + objects[i].transform.up * 0.1f;
            } else
            {
                Debug.Log("Device " + i + " is missing.");
            }

            if (i > 0)
            {
                info += " ";
            }

            info += position.x + " " + position.y + " " + position.z + " " + F.x + " " + F.y + " " + F.z + " " + U.x + " " + U.y + " " + U.z;

        }

        return info;
    }

    private Data.POS getPOS(bool isXPos)
    {
        Data.POS pos;
        int s = 0, t = 3;

        if (isXPos)
        {
            pos = new Data.X_POS();
        } else
        {
            pos = new Data.Y_POS();
            s = 3;
            t = 8;
        }

        for (int i = s; i < t; i++)
        {
            Vector3 position = new Vector3();
            Vector3 F = new Vector3();
            Vector3 U = new Vector3();

            if (objects[i] != null)
            {
                position = objects[i].transform.position;
                F = position + objects[i].transform.forward * 0.1f;
                U = position + objects[i].transform.up * 0.1f;
            }

            pos.vec[(i - s) * 9 + 0] = position.x;
            pos.vec[(i - s) * 9 + 1] = position.y;
            pos.vec[(i - s) * 9 + 2] = position.z;
            pos.vec[(i - s) * 9 + 3] = F.x;
            pos.vec[(i - s) * 9 + 4] = F.y;
            pos.vec[(i - s) * 9 + 5] = F.z;
            pos.vec[(i - s) * 9 + 6] = U.x;
            pos.vec[(i - s) * 9 + 7] = U.y;
            pos.vec[(i - s) * 9 + 8] = U.z;
        }

        return pos;
    }

    public Data.X_POS getXPos()
    {
        return (Data.X_POS)getPOS(true);
    }

    public Data.Y_POS getYPos()
    {
        return (Data.Y_POS)getPOS(false);
    }

    void Update()
    {
        motionRecord();
        checkMoving();
        checkHoming();

        if (Utility.isStart())
        {
            if (TaskManager.isRest())
            {
                homePos = getXPos();
            } else
            {
                TaskManager.start();
            }
        }

        if (Utility.isRestart())
        {
            if (errorScreen.text == "")
            {
                TaskManager.restart();
            } else
            {
                errorScreen.text = "";
            }
        }

        if (TaskManager.isRecording())
        {
            string taskInfo = TaskManager.getTaskInfo();
            string dataInfo = getDataInfo();
            string info = taskInfo + " " + dataInfo;
            FileLogger.log(info);
        }
    }

    private List<float> timestamp = new List<float>();
    private List<Data.X_POS> xPos = new List<Data.X_POS>();
    private List<Data.Y_POS> yPos = new List<Data.Y_POS>();
    private string errorInfo = "";
    private float speedThreshold = 0.2f;
    private float beginThreshold = 0.1f;
    private float endThreshold = 0.5f;
    private float stdBeginTimePer = 0.2f;
    private float stdBeginTimeInterval = 1f;
    private float homeThreshold = 0.1f;
    private float movingTime = 0f;
    private Data.X_POS lastXPos = new Data.X_POS();
    private float stopTime = 0;
    private Data.X_POS homePos = new Data.X_POS();

    bool checkMotion()
    {
        int T = timestamp.Count;
        int moving = 0;
        int stop = 0;
        int startTime = -1;
        int endTime = -1;
        bool isMoving = false;
        for (int i = 1; i < T; i++)
        {
            float speed = Data.POS.meanDist(xPos[i], xPos[i - 1]) / (timestamp[i] - timestamp[i - 1]);
            if (speed >= speedThreshold)
            {
                moving++;
                stop = 0;
            } else
            {
                moving = 0;
                stop++;
            }
            if (timestamp[i] - timestamp[i - moving] >= beginThreshold)
            {
                isMoving = true;
            }
            if (timestamp[i] - timestamp[i - stop] >= endThreshold)
            {
                isMoving = false;
            }
            if (isMoving)
            {
                if (startTime == -1)
                {
                    startTime = i;
                }
                if (endTime != -1)
                {
                    errorInfo = "more than one action recorded";
                    return false;
                }
            } else
            {
                if (startTime != -1 && endTime == -1)
                {
                    endTime = i;
                }
            }
        }

        if (startTime == -1)
        {
            errorInfo = "no action recorded";
            return false;
        }

        if (endTime == -1 && !TaskManager.isOneAction())
        {
            errorInfo = "no stop at the end";
            return false;
        }

        if (timestamp[startTime] - TaskManager.getActionPeriod() * stdBeginTimePer > stdBeginTimeInterval / 2)
        {
            errorInfo = "begin too late";
            return false;
        }

        if (timestamp[startTime] - TaskManager.getActionPeriod() * stdBeginTimePer < -stdBeginTimeInterval / 2)
        {
            errorInfo = "begin too early";
            return false;
        }

        float dist = Data.POS.meanDist(xPos[0], homePos);
        if (dist > homeThreshold)
        {
            errorInfo = "not reset at begin";
            return false;
        }

        return true;
    }

    void motionRecord()
    {
        if (TaskManager.isRecording())
        {
            timestamp.Add(TaskManager.getEscapeTime());
            xPos.Add(getXPos());
            yPos.Add(getYPos());
        } else
        {
            if (timestamp.Count > 0)
            {
                if (checkMotion())
                {
                    errorScreen.color = Color.green;
                    errorScreen.text = "ok";
                } else
                {
                    errorScreen.color = Color.red;
                    errorScreen.text = errorInfo;
                }
                timestamp.Clear();
                xPos.Clear();
                yPos.Clear();
            }
        }
    }

    void checkMoving()
    {
        Data.X_POS xPos = getXPos();
        float speed = Data.POS.meanDist(xPos, lastXPos) / Time.deltaTime;
        if (speed >= speedThreshold)
        {
            movingTime += Time.deltaTime;
            stopTime = 0f;
            if (movingTime >= beginThreshold)
            {
                moveScreen.text = "Moving";
            }
        }
        else
        {
            movingTime = 0f;
            stopTime += Time.deltaTime;
            if (stopTime >= endThreshold)
            {
                moveScreen.text = "Stop";
            }
        }
        lastXPos = xPos;
    }

    void checkHoming()
    {
        Data.X_POS xPos = getXPos();
        float dist = Data.POS.meanDist(xPos, homePos);
        if (dist > homeThreshold)
        {
            homeScreen.text = "Away";
        } else
        {
            homeScreen.text = "Home";
        }
    }
}
