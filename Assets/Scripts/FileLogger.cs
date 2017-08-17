using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FileLogger : MonoBehaviour
{
    private static StreamWriter writer = null;
    public string userName = "default";
    static FileLogger logger = null;

    void Awake()
    {
        logger = this;
    }

    void Start()
    {

    }
	
	void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
            writer = null;
        }
	}

    static public void log(string info)
    {
        if (writer == null)
        {
            writer = File.CreateText("data/" + logger.userName + ".txt");
        }
        writer.WriteLine(info);
        writer.Flush();
    }
}
