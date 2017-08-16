using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Transfer : MonoBehaviour {
    public GameObject[] cubes;
    private int speed = 2;
    private string inputDir = "pic/knee_lift_right/";
    private string inputFile = "data/gyz731.txt";
    private string outputFile = "data/gyz731_vec.txt";
    int id = 5;

    StreamReader sr;
    StreamReader sr1;
    StreamReader sr2;
    StreamWriter sw;

    bool isTransfer = false;
    bool isShowPredict = true;

    void transfer()
    {
        sw = File.CreateText(outputFile);
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
        sr = File.OpenText(inputFile);
        if (isTransfer)
        {
            transfer();
        }
    }
	
	void Update () {
        if (isShowPredict)
        {
            if (sr1 == null)
            {
                if (id == 50)
                {
                    return;
                }
                sr1 = File.OpenText(inputDir + id.ToString() + ".txt");
                sr2 = File.OpenText(inputDir + id.ToString() + "_gt.txt");
                id++;
            }
            string str1 = sr1.ReadLine();
            string str2 = sr2.ReadLine();
            str1 = sr1.ReadLine();
            str2 = sr2.ReadLine();
            if (str1 == null || str2 == null)
            {
                sr1.Close();
                sr1 = null;
                sr2.Close();
                sr2 = null;
                return;
            }
            string[] tags = str1.Split(' ');
            int cnt = 0;
            for (int i = 1; i < tags.Length; i += 9)
            {
                cubes[cnt].transform.position = new Vector3(float.Parse(tags[i + 0]), float.Parse(tags[i + 1]), float.Parse(tags[i + 2]));
                cubes[cnt].transform.LookAt(new Vector3(float.Parse(tags[i + 3]), float.Parse(tags[i + 4]), float.Parse(tags[i + 5])));
                //cubes[cnt].transform.LookAt(new Vector3(float.Parse(tags[i + 6]), float.Parse(tags[i + 7]), float.Parse(tags[i + 8])), Vector3.forward);
                cnt++;
            }
            tags = str2.Split(' ');
            for (int i = 1; i < tags.Length; i += 9)
            {
                cubes[cnt].transform.position = new Vector3(float.Parse(tags[i + 0]) + 0.5f, float.Parse(tags[i + 1]), float.Parse(tags[i + 2]));
                cubes[cnt].transform.LookAt(new Vector3(float.Parse(tags[i + 3]) + 0.5f, float.Parse(tags[i + 4]), float.Parse(tags[i + 5])));
                //cubes[cnt].transform.LookAt(new Vector3(float.Parse(tags[i + 6]), float.Parse(tags[i + 7]), float.Parse(tags[i + 8])), Vector3.forward);
                cnt++;
            }
        }
    }
}
