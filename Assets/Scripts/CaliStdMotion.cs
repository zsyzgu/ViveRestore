using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CaliStdMotion : MonoBehaviour {
    public GameObject[] cubes;
    private List<string> motion = new List<string>();
    private int index = -1;
    private bool shouldDestroy = false;

    void Start()
    {

    }

    public void loadMotion(string fileName)
    {
        motion.Clear();
        StreamReader sr = File.OpenText(fileName);

        string str = "";
        while ((str = sr.ReadLine()) != null)
        {
            motion.Add(str);
        }

        sr.Close();
    }

    public void startMotion(bool shouldDestroy = false)
    {
        index = 0;
        this.shouldDestroy = shouldDestroy;
    }

    void updateMotion(int id)
    {
        if (id < motion.Count)
        {
            string[] tags = motion[id].Split(' ');

            int cnt = 0;
            for (int i = 1; i < tags.Length; i += 7)
            {
                cubes[cnt].transform.position = new Vector3(float.Parse(tags[i + 0]), float.Parse(tags[i + 1]), float.Parse(tags[i + 2])) + transform.position;
                cubes[cnt].transform.rotation = new Quaternion(float.Parse(tags[i + 3]), float.Parse(tags[i + 4]), float.Parse(tags[i + 5]), float.Parse(tags[i + 6]));
                cnt++;
            }
        }
    }

    void Update()
    {
        if (index != -1)
        {
            index++;
            if (index >= motion.Count)
            {
                index = -1;
                if (shouldDestroy)
                {
                    gameObject.SetActive(false);
                }
            } else
            {
                updateMotion(index);
            }
        } else
        {
            updateMotion(0);
        }
    }
}
