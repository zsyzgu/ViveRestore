using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadData : MonoBehaviour {
    class X_POS
    {
        public static int N = 27;
        public float[] vec = new float[N];
        public static X_POS operator - (X_POS x1, X_POS x2)
        {
            for (int i = 0; i < N; i++)
            {
                x1.vec[i] -= x2.vec[i];
            }
            return x1;
        }
    }

    class Y_POS
    {
        public static int N = 45;
        public float[] vec = new float[N];
        public static Y_POS operator - (Y_POS y1, Y_POS y2)
        {
            for (int i = 0; i < N; i++)
            {
                y1.vec[i] -= y2.vec[i];
            }
            return y1;
        }
    }

    class Motion
    {
        public List<float> timestamp = new List<float>();
        public List<X_POS> xPos = new List<X_POS>();
        public List<Y_POS> yPos = new List<Y_POS>();
        public X_POS xStart = new X_POS();
        public Y_POS yStart = new Y_POS();

        public void readTags(string[] tags)
        {
            timestamp.Add(float.Parse(tags[2]));
            X_POS x = new X_POS();
            for (int i = 0; i < X_POS.N; i++)
            {
                x.vec[i] = float.Parse(tags[3 + i]);
            }
            Y_POS y = new Y_POS();
            for (int i = 0; i < Y_POS.N; i++)
            {
                y.vec[i] = float.Parse(tags[3 + X_POS.N + i]);
            }
            if (timestamp.Count == 1)
            {
                xStart = x;
                yStart = y;
            }
            x = x - xStart;
            y = y - yStart;
            xPos.Add(x);
            yPos.Add(y);
        }
    }

    static Dictionary<string, List<Motion>> motions = new Dictionary<string, List<Motion>>();

    public void readFile(string fileName)
    {
        StreamReader sr = File.OpenText(fileName);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] tags = line.Split(' ');
            string name = tags[0];
            int id = int.Parse(tags[1]);
            float timestamp = float.Parse(tags[2]);
            if (!motions.ContainsKey(name))
            {
                motions[name] = new List<Motion>();
            }
            if (timestamp == 0f)
            {
                while (id >= motions[name].Count)
                {
                    motions[name].Add(new Motion());
                }
            }
            motions[name][id].readTags(tags);
        }
    }


	void Start () {
        readFile("Data/gyz.txt");
        Debug.Log(motions["knee_lift_left"][0].timestamp[2]);
	}
	
	void Update () {
		
	}
}
