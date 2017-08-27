using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CaliStdMotion : MonoBehaviour {
    public GameObject[] cubes;
    StreamReader sr = null;
    bool started = false;

    void Start()
    {

    }

    public void setMotion(string motionName)
    {
        if (sr != null)
        {
            sr.Close();
            sr = null;
        }
        sr = File.OpenText("Std/" + motionName + ".txt");
        string str = sr.ReadLine();
        updateMotion(str);
        started = false;
    }

    public void startMotion()
    {
        started = true;
    }

    void updateMotion(string str)
    {
        string[] tags = str.Split(' ');
        int cnt = 0;
        for (int i = 1; i < tags.Length; i += 9)
        {
            cubes[cnt].transform.position = new Vector3(float.Parse(tags[i + 0]), float.Parse(tags[i + 1]), float.Parse(tags[i + 2])) + transform.position;
            cubes[cnt].transform.LookAt(new Vector3(float.Parse(tags[i + 3]), float.Parse(tags[i + 4]), float.Parse(tags[i + 5])) + transform.position);
            cnt++;
        }
    }

    void Update()
    {
        if (sr != null)
        {
            if (started)
            {
                string str = sr.ReadLine();
                if (str == null)
                {
                    sr.Close();
                    sr = null;
                    return;
                }

                updateMotion(str);
            }
        } else
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                cubes[i].transform.position = new Vector3();
                cubes[i].transform.eulerAngles = new Vector3();
            }
        }
    }
}
