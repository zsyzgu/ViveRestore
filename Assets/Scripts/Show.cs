using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Show : MonoBehaviour {
    public GameObject[] cubes;
    public string filePath = null;
    private string motionName = null;
    StreamReader sr;
    
	void Start () {
        
    }

    public void setMotion(string motionName)
    {
        if (this.motionName != motionName)
        {
            sr = null;
        }
        this.motionName = motionName;
    }
	
	void Update () {
        if (sr == null)
        {
            if (motionName == null)
            {
                return;
            }
            sr = File.OpenText("Std/" + motionName + ".txt");
        }
        string str = sr.ReadLine();
        if (str == null)
        {
            sr.Close();
            sr = null;
            return;
        }
        string[] tags = str.Split(' ');
        int cnt = 0;
        for (int i = 1; i < tags.Length; i += 7)
        {
            cubes[cnt].transform.position = new Vector3(float.Parse(tags[i + 0]), float.Parse(tags[i + 1]), float.Parse(tags[i + 2])) + transform.position;
            cubes[cnt].transform.rotation = new Quaternion(float.Parse(tags[i + 3]), float.Parse(tags[i + 4]), float.Parse(tags[i + 5]), float.Parse(tags[i + 6]));
            cnt++;
        }
    }
}
