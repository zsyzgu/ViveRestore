using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FileLogger : MonoBehaviour
{
    private string fileName = "default";
    private static StreamWriter writer;

    void Awake()
    {
        writer = File.CreateText("data/" + fileName + ".txt");
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
        writer.WriteLine(info);
        writer.Flush();
    }
}
