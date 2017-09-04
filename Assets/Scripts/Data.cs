using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Data : MonoBehaviour {
    public class POS
    {
        public int N;
        public float[] vec;

        public POS()
        {

        }

        public POS(int n)
        {
            init(n);
        }

        public void init(int N)
        {
            this.N = N;
            vec = new float[N];
        }

        public static POS operator - (POS p1, POS p2)
        {
            POS p = new POS(p1.N);
            for (int i = 0; i < p.N; i++)
            {
                p.vec[i] = p1.vec[i] - p2.vec[i];
            }
            return p;
        }

        public static POS operator + (POS p1, POS p2)
        {
            POS p = new POS(p1.N);
            for (int i = 0; i < p.N; i++)
            {
                p.vec[i] = p1.vec[i] + p2.vec[i];
            }
            return p;
        }

        public static POS operator / (POS p1, float k)
        {
            POS p = new POS(p1.N);
            for (int i = 0; i < p.N; i++)
            {
                p.vec[i] = p1.vec[i] / k;
            }
            return p;
        }

        public static POS operator * (POS p1, float k)
        {
            POS p = new POS(p1.N);
            for (int i = 0; i < p.N; i++)
            {
                p.vec[i] = p1.vec[i] * k;
            }
            return p;
        }

        public static POS operator + (POS p, Vector3 v)
        {
            POS ret = new POS(p.N);
            for (int i = 0; i < ret.N; i += 3)
            {
                ret.vec[i + 0] = p.vec[i + 0] + v.x;
                ret.vec[i + 1] = p.vec[i + 1] + v.y;
                ret.vec[i + 2] = p.vec[i + 2] + v.z;
            }
            return ret;
        }

        public static POS operator - (POS p, Vector3 v)
        {
            POS ret = new POS(p.N);
            for (int i = 0; i < ret.N; i += 3)
            {
                ret.vec[i + 0] = p.vec[i + 0] - v.x;
                ret.vec[i + 1] = p.vec[i + 1] - v.y;
                ret.vec[i + 2] = p.vec[i + 2] - v.z;
            }
            return ret;
        }
        
        public static float dist(POS p1, POS p2)
        {
            float sum = 0;
            int cnt = 0;
            for (int i = 0; i < p1.N; i += 7)
            {
                float d2 = 0;
                d2 += Mathf.Pow(p1.vec[i + 0] - p2.vec[i + 0], 2f);
                d2 += Mathf.Pow(p1.vec[i + 1] - p2.vec[i + 1], 2f);
                d2 += Mathf.Pow(p1.vec[i + 2] - p2.vec[i + 2], 2f);
                sum += Mathf.Sqrt(d2);
                cnt++;
            }
            sum /= cnt;
            return sum;
        }

        protected void formPos(List<GameObject> objs)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                {
                    vec[i * 7 + 0] = objs[i].transform.position.x;
                    vec[i * 7 + 1] = objs[i].transform.position.y;
                    vec[i * 7 + 2] = objs[i].transform.position.z;
                    vec[i * 7 + 3] = objs[i].transform.rotation.x;
                    vec[i * 7 + 4] = objs[i].transform.rotation.y;
                    vec[i * 7 + 5] = objs[i].transform.rotation.z;
                    vec[i * 7 + 6] = objs[i].transform.rotation.w;
                }
            }
        }
    }

    public class X_POS : POS
    {
        public X_POS()
        {
            init(21);
        }

        public X_POS(POS p)
        {
            init(21);
            for (int i = 0; i < N; i++)
            {
                vec[i] = p.vec[i];
            }
        }

        public X_POS(GameObject head, GameObject leftHand, GameObject rightHand)
        {
            init(21);
            formXPos(head, leftHand, rightHand);
        }

        public void formXPos(GameObject head, GameObject leftHand, GameObject rightHand)
        {
            List<GameObject> objs = new List<GameObject>();
            objs.Add(head);
            objs.Add(leftHand);
            objs.Add(rightHand);
            formPos(objs);
        }

        public static float handsDistToHead(X_POS p1, X_POS p2)
        {
            float sum = 0;
            int cnt = 0;
            for (int i = 7; i < p1.N; i += 7)
            {
                float d2 = 0;
                d2 += Mathf.Pow((p1.vec[i + 0] - p1.vec[0]) - (p2.vec[i + 0] - p2.vec[0]), 2f);
                d2 += Mathf.Pow((p1.vec[i + 1] - p1.vec[1]) - (p2.vec[i + 1] - p2.vec[1]), 2f);
                d2 += Mathf.Pow((p1.vec[i + 2] - p1.vec[2]) - (p2.vec[i + 2] - p2.vec[2]), 2f);
                sum += Mathf.Sqrt(d2);
                cnt++;
            }
            sum /= cnt;
            return sum;
        }

        public static float handsDist(X_POS p1, X_POS p2)
        {
            float sum = 0;
            int cnt = 0;
            for (int i = 7; i < p1.N; i += 7)
            {
                float d2 = 0;
                d2 += Mathf.Pow(p1.vec[i + 0] - p2.vec[i + 0], 2f);
                d2 += Mathf.Pow(p1.vec[i + 1] - p2.vec[i + 1], 2f);
                d2 += Mathf.Pow(p1.vec[i + 2] - p2.vec[i + 2], 2f);
                sum += Mathf.Sqrt(d2);
                cnt++;
            }
            sum /= cnt;
            return sum;
        }

        public float[] getHandsVector()
        {
            float[] ret = new float[14];
            for (int i = 0; i < 14; i++)
            {
                ret[i] = vec[i + 7];
            }
            return ret;
        }
    }

    public class Y_POS : POS
    {
        public Y_POS()
        {
            init(35);
        }

        public Y_POS(POS p)
        {
            init(35);
            for (int i = 0; i < N; i++)
            {
                vec[i] = p.vec[i];
            }
        }

        public Y_POS(GameObject leftFoot, GameObject rightFoot, GameObject leftKnee, GameObject rightKnee, GameObject waist)
        {
            init(35);
            formYPos(leftFoot, rightFoot, leftKnee, rightKnee, waist);
        }

        public void formYPos(GameObject leftFoot, GameObject rightFoot, GameObject leftKnee, GameObject rightKnee, GameObject waist)
        {
            List<GameObject> objs = new List<GameObject>();
            objs.Add(leftFoot);
            objs.Add(rightFoot);
            objs.Add(leftKnee);
            objs.Add(rightKnee);
            objs.Add(waist);
            formPos(objs);
        }
    }

    public class Motion
    {
        const float SPEED_THRESHOLD = 0.2f;
        const float BEGIN_DURATION = 0.1f;
        const float END_DURATION = 0.5f;
        public List<float> timestamp = new List<float>();
        public List<X_POS> xPos = new List<X_POS>();
        public List<Y_POS> yPos = new List<Y_POS>();
        public X_POS xStart = new X_POS();
        public Y_POS yStart = new Y_POS();
        private List<X_POS> xSpeed = new List<X_POS>();
        private List<Y_POS> ySpeed = new List<Y_POS>();
        private float predictFrame = 0f;
        private float[] dtw;

        public void readTags(string[] tags, int baseId = 0)
        {
            timestamp.Add(float.Parse(tags[baseId + 0]));
            X_POS x = new X_POS();
            for (int i = 0; i < x.N; i++)
            {
                x.vec[i] = float.Parse(tags[baseId + 1 + i]);
            }
            Y_POS y = new Y_POS();
            for (int i = 0; i < y.N; i++)
            {
                y.vec[i] = float.Parse(tags[baseId + 1 + x.N + i]);
            }
            xPos.Add(x);
            yPos.Add(y);
        }

        public void formMotion(ControlledHuman.Record record, int startIndex, int endIndex)
        {
            int currIndex = record.getIndex();
            for (int i = startIndex; i <= endIndex; i++)
            {
                timestamp.Add(record.getTimestamp(currIndex - i) - record.getTimestamp(currIndex - startIndex));
                xPos.Add(record.getXPos(currIndex - i));
                yPos.Add(record.getYPos(currIndex - i));
            }
        }

        private bool segment()
        {
            int startIndex = -1;
            int endIndex = -1;

            int T = timestamp.Count;
            int moveFrame = 0;
            int stopFrame = 0;
            bool moving = false;
            for (int t = 1; t < T; t++)
            {
                float speed = POS.dist(xPos[t], xPos[t - 1]) / (timestamp[t] - timestamp[t - 1]);
                if (speed >= SPEED_THRESHOLD)
                {
                    moveFrame++;
                    stopFrame = 0;
                    if (timestamp[t] - timestamp[t - moveFrame] >= BEGIN_DURATION)
                    {
                        moving = true;
                    }
                } else
                {
                    moveFrame = 0;
                    stopFrame++;
                    if (timestamp[t] - timestamp[t - stopFrame] >= END_DURATION)
                    {
                        moving = false;
                    }
                }
                if (moving)
                {
                    if (startIndex == -1)
                    {
                        startIndex = t - moveFrame;
                    }
                } else
                {
                    if (startIndex != -1 && endIndex == -1)
                    {
                        endIndex = t - stopFrame;
                    }
                }
            }

            bool ok = true;
            if (startIndex == -1)
            {
                Debug.Log("segment error (begin)");
                startIndex = 0;
                ok = false;
            } else if (Mathf.Abs(timestamp[startIndex] - timestamp[0] - 1f) > 0.5f)
            {
                ok = false;
            }
            if (endIndex == -1)
            {
                Debug.Log("segment error (end)");
                endIndex = timestamp.Count - 1;
                ok = false;
            }

            timestamp = timestamp.GetRange(startIndex, endIndex - startIndex + 1);
            xPos = xPos.GetRange(startIndex, endIndex - startIndex + 1);
            yPos = yPos.GetRange(startIndex, endIndex - startIndex + 1);
            xStart = xPos[0];
            yStart = yPos[0];
            T = timestamp.Count;
            float startTime = timestamp[0];
            for (int t = 0; t < T; t++)
            {
                xPos[t] = new X_POS(xPos[t] - xStart);
                yPos[t] = new Y_POS(yPos[t] - yStart);
                timestamp[t] -= startTime;
            }

            return ok;
        }

        private void calnSpeed()
        {
            xSpeed.Add(new X_POS());
            for (int t = 1; t < timestamp.Count; t++)
            {
                xSpeed.Add(new X_POS((xPos[t] - xPos[t - 1]) / (timestamp[t] - timestamp[t - 1])));
            }
            ySpeed.Add(new Y_POS());
            for (int t = 1; t < timestamp.Count; t++)
            {
                ySpeed.Add(new Y_POS((yPos[t] - yPos[t - 1]) / (timestamp[t] - timestamp[t - 1])));
            }
        }

        public bool preprocess()
        {
            bool ok = segment();
            calnSpeed();
            return ok;
        }

        private X_POS getXSpeed(float t)
        {
            if (t >= timestamp.Count - 1)
            {
                return xSpeed[timestamp.Count - 1];
            }
            int intT = Mathf.FloorToInt(t);
            return new X_POS(xSpeed[intT] * (t - intT) + xSpeed[intT + 1] * (intT + 1 - t));
        }

        public Y_POS getYPos(float t)
        {
            if (t >= timestamp.Count - 1)
            {
                return yPos[timestamp.Count - 1];
            }
            int intT = Mathf.FloorToInt(t);
            return new Y_POS(yPos[intT] * (t - intT) + yPos[intT + 1] * (intT + 1 - t));
        }

        public void resetMotion()
        {
            predictFrame = 1f;
            dtw = new float[timestamp.Count];
        }

        public float predictMotionFrame(ControlledHuman.Record record, out float score)
        {
            int dtwFrame = calnFrame(record);
            score = dtw[dtwFrame];
            predictFrame += 1f;
            if (dtwFrame > predictFrame + 1.0f)
            {
                predictFrame += 1.0f;
            } else if (dtwFrame < predictFrame - 0.5f)
            {
                predictFrame -= 0.5f;
            }
            return predictFrame;
        }

        private int calnFrame(ControlledHuman.Record record)
        {
            if (record.getIndex() < 1)
            {
                return 0;
            }
            X_POS xSpeed = new X_POS((record.getXPos(0) - record.getXPos(1)) / (record.getTimestamp(0) - record.getTimestamp(1)));
            if (dtw[1] == 0f)
            {
                dtw[1] = X_POS.handsDist(xSpeed, getXSpeed(1));
                return 1;
            }
            float[] nDtw = new float[timestamp.Count];
            for (int t = 1; t < timestamp.Count; t++)
            {
                if (nDtw[t - 1] != 0f && (nDtw[t] == 0f || nDtw[t - 1] < nDtw[t]))
                {
                    nDtw[t] = nDtw[t - 1];
                }
                if (dtw[t - 1] != 0f && (nDtw[t] == 0f || dtw[t - 1] < nDtw[t]))
                {
                    nDtw[t] = dtw[t - 1];
                }
                if (dtw[t] != 0f && (nDtw[t] == 0f || dtw[t] < nDtw[t]))
                {
                    nDtw[t] = dtw[t];
                }
                nDtw[t] += X_POS.handsDist(xSpeed, getXSpeed(t));
            }
            dtw = nDtw;
            int frame = 1;
            for (int t = 2; t< timestamp.Count; t++)
            {
                if (dtw[t] != 0 && dtw[t] < dtw[frame])
                {
                    frame = t;
                }
            }
            return frame;
        }

        public void ts()
        {
            StreamWriter sw = File.CreateText("Cali/cali.txt");

            for (int t = 0; t < timestamp.Count; t++)
            {
                string info = timestamp[t].ToString();

                for (int i = 0; i < xPos[t].N; i++)
                {
                    info += " " + xPos[t].vec[i].ToString();
                }

                for (int i = 0; i < yPos[t].N; i++)
                {
                    info += " " + yPos[t].vec[i].ToString();
                }

                sw.WriteLine(info);
            }

            sw.Close();
        }

        public void output(string fileName, string motionName, int motionId)
        {
            StreamWriter sw = File.AppendText(fileName);
            
            for (int t = 0; t < timestamp.Count; t++)
            {
                string info = motionName + " " + motionId.ToString() + " " + timestamp[t].ToString();
                
                for (int i = 0; i < xPos[t].N; i++)
                {
                    info += " " + xPos[t].vec[i].ToString();
                }

                for (int i = 0; i < yPos[t].N; i++)
                {
                    info += " " + yPos[t].vec[i].ToString();
                }

                sw.WriteLine(info);
            }

            sw.Close();
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

	}
	
	void Update () {

	}
}
