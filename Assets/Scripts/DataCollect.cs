using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataCollect : MonoBehaviour
{
    public GameObject[] objects;
    
    void Start()
    {

    }

    string getDataInfo()
    {
        string info = "";

        for (int i = 0; i < objects.Length; i++)
        {
            Vector3 position = new Vector3();
            Vector3 roatation = new Vector3();

            if (objects[i] != null)
            {
                position = objects[i].transform.position;
                roatation = objects[i].transform.eulerAngles;
            } else
            {
                Debug.Log("Device " + i + " is missing.");
            }

            if (i > 0)
            {
                info += " ";
            }

            info += position.x + " " + position.y + " " + position.z + " " + roatation .x + " " + roatation.y + " " + roatation.z;
        }

        return info;
    }

    void Update()
    {
        if (Utility.isStart(objects[1], objects[2]))
        {
            TaskManager.start();
        }

        if (Utility.isRestart())
        {
            TaskManager.restart();
        }

        if (TaskManager.isRecording())
        {
            string taskInfo = TaskManager.getTaskInfo();
            string dataInfo = getDataInfo();
            string info = taskInfo + " " + dataInfo;
            FileLogger.log(info);
        }
    }
}
