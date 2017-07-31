using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Transfer : MonoBehaviour {
    public GameObject[] cubes;
    private int speed = 2;

    StreamReader sr;
    StreamWriter sw;

    bool isTransfer = false;
    bool isShow = true;

    void transfer()
    {
        sw = File.CreateText("data/gyz731_vec.txt");
        string str;
        while ((str = sr.ReadLine()) != null)
        {
            string[] tags = str.Split(' ');
            string info = tags[0] + " " + tags[1] + " " + tags[2];

            for (int i = 3; i < tags.Length; i += 6)
            {
                transform.position = new Vector3(float.Parse(tags[i + 0]), float.Parse(tags[i + 1]), float.Parse(tags[i + 2]));
                transform.eulerAngles = new Vector3(float.Parse(tags[i + 3]), float.Parse(tags[i + 4]), float.Parse(tags[i + 5]));
                Vector3 F = transform.position + transform.forward * 0.1f;
                Vector3 U = transform.position + transform.up * 0.1f;
                info += " " + transform.position.x + " " + transform.position.y + " " + transform.position.z;
                info += " " + F.x + " " + F.y + " " + F.z;
                info += " " + U.x + " " + U.y + " " + U.z;
            }
            sw.WriteLine(info);
        }
        sw.Close();
    }

	void Start () {
        sr = File.OpenText("data/gyz731.txt");
        if (isTransfer)
        {
            transfer();
        }
    }
	
	void Update () {
        if (isShow)
        {
            string str = "";
            for (int i = 0; i < speed; i++)
            {
                str = sr.ReadLine();
            }
            string[] tags = str.Split(' ');
            int cnt = 0;
            for (int i = 3; i < tags.Length; i += 6)
            {
                cubes[cnt].transform.position = new Vector3(float.Parse(tags[i + 0]), float.Parse(tags[i + 1]), float.Parse(tags[i + 2]));
                cubes[cnt].transform.eulerAngles = new Vector3(float.Parse(tags[i + 3]), float.Parse(tags[i + 4]), float.Parse(tags[i + 5]));
                cnt++;
            }
        }
    }
}
