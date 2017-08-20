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

            pos.vec[i * 9 + 0] = position.x;
            pos.vec[i * 9 + 1] = position.y;
            pos.vec[i * 9 + 2] = position.z;
            pos.vec[i * 9 + 3] = F.x;
            pos.vec[i * 9 + 4] = F.y;
            pos.vec[i * 9 + 5] = F.z;
            pos.vec[i * 9 + 6] = U.x;
            pos.vec[i * 9 + 7] = U.y;
            pos.vec[i * 9 + 8] = U.z;
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
