using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Data : MonoBehaviour {
    public class POS
    {
        public int N;
        public float[] vec;
        public void init(int N)
        {
            this.N = N;
            vec = new float[N];
        }

        public static POS operator - (POS p1, POS p2)
        {
            for (int i = 0; i < p1.N; i++)
            {
                p1.vec[i] -= p2.vec[i];
            }
            return p1;
        }

        public static float dist(POS p1, POS p2)
        {
            float sum = 0;
            for (int i = 0; i < p1.N; i++)
            {
                sum += (p1.vec[i] - p2.vec[i]) * (p1.vec[i] - p2.vec[i]);
            }
            float ret = Mathf.Sqrt(sum);
            return ret;
        }

        public static float meanDist(POS p1, POS p2)
        {
            float sum = 0;
            int cnt = 0;
            for (int i = 0; i < p1.N; i += 9)
            {
                float d2 = 0;
                d2 += (p1.vec[i + 0] - p2.vec[i + 0]) * (p1.vec[i + 0] - p2.vec[i + 0]);
                d2 += (p1.vec[i + 1] - p2.vec[i + 1]) * (p1.vec[i + 1] - p2.vec[i + 1]);
                d2 += (p1.vec[i + 2] - p2.vec[i + 2]) * (p1.vec[i + 2] - p2.vec[i + 2]);
                sum += Mathf.Sqrt(d2);
                cnt++;
            }
            sum /= cnt;
            return sum;
        }
    }

    public class X_POS : POS
    {
        public X_POS()
        {
            init(27);
        }
    }

    public class Y_POS : POS
    {
        public Y_POS()
        {
            init(45);
        }
    }

    public class Motion
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
            for (int i = 0; i < x.N; i++)
            {
                x.vec[i] = float.Parse(tags[3 + i]);
            }
            Y_POS y = new Y_POS();
            for (int i = 0; i < y.N; i++)
            {
                y.vec[i] = float.Parse(tags[3 + x.N + i]);
            }
            if (timestamp.Count == 1)
            {
                xStart = x;
                yStart = y;
            }
            x = (X_POS)(x - xStart);
            y = (Y_POS)(y - yStart);
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
