using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FootController : MonoBehaviour {
    public string[] motionName;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftFoot;
    public GameObject rightFoot;
    public GameObject leftKnee;
    public GameObject rightKnee;
    public GameObject waist;

    class Record
    {
        const int RECORD_FRAMS = 10000;

        private float[] timestamp = new float[RECORD_FRAMS];
        private Data.X_POS[] xPosList = new Data.X_POS[RECORD_FRAMS];
        private Data.Y_POS[] yPosList = new Data.Y_POS[RECORD_FRAMS];
        private int index = -1;

        public void recordData(float t, Data.X_POS xPos, Data.Y_POS yPos)
        {
            index++;
            int i = index % RECORD_FRAMS;
            timestamp[i] = t;
            xPosList[i] = xPos;
            yPosList[i] = yPos;
        }

        public int getIndex()
        {
            return index;
        }

        public float getTimestamp(int frame)
        {
            if (frame > index)
            {
                Debug.Log("Not recorded");
                return 0;
            }
            return timestamp[(index - frame) % RECORD_FRAMS];
        }

        public Data.X_POS getXPos(int frame)
        {
            if (frame > index)
            {
                Debug.Log("Not recorded");
                return new Data.X_POS();
            }
            return xPosList[(index - frame) % RECORD_FRAMS];
        }

        public Data.Y_POS getYPos(int frame)
        {
            if (frame > index)
            {
                Debug.Log("Not recorded");
                return new Data.Y_POS();
            }
            return yPosList[(index - frame) % RECORD_FRAMS];
        }
    }
    Record record = new Record();

    class MovingDetect
    {
        const float SPEED_THRESHOLD = 0.2f;
        const float BEGIN_DURATION = 0.1f;
        const float END_DURATION = 0.5f;

        private Vector3 lastLeftHandPos;
        private Vector3 lastRightHandPos;
        private int moveFrames = 0;
        private int stopFrames = 0;
        private bool moving = false;
        private int startIndex = 0;
        private Vector3 startHeadPos = new Vector3();

        private void startMoving(Record record)
        {
            startIndex = record.getIndex() - moveFrames;
            startHeadPos = record.getXPos(moveFrames).getHeadPos();
        }

        public bool isMoving(Record record)
        {
            if (record.getIndex() >= 1)
            {
                float speed = Data.X_POS.handsDist(record.getXPos(0), record.getXPos(1)) / (record.getTimestamp(0) - record.getTimestamp(1));
                if (speed >= SPEED_THRESHOLD)
                {
                    moveFrames++;
                    stopFrames = 0;
                    if (record.getTimestamp(0) - record.getTimestamp(moveFrames) >= BEGIN_DURATION)
                    {
                        if (moving == false)
                        {
                            moving = true;
                            startMoving(record);
                        }
                    }
                }
                else
                {
                    moveFrames = 0;
                    stopFrames++;
                    if (record.getTimestamp(0) - record.getTimestamp(stopFrames) >= END_DURATION)
                    {
                        moving = false;
                    }
                }
            }
            return moving;
        }

        public int getStartIndex()
        {
            return startIndex;
        }

        public Vector3 getStartHeadPos()
        {
            return startHeadPos;
        }
    }
    MovingDetect movingDetect = new MovingDetect();

    private Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    private void loadStdMotion()
    {
        foreach (string name in motionName)
        {
            Data.Motion motion = new Data.Motion();
            string fileName = "Std/" + name + ".txt";
            StreamReader sr = File.OpenText(fileName);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] tags = line.Split(' ');
                motion.readTags(tags);
            }
            motion.segment();
            stdMotions[name] = motion;
        }
    }

    private void setLowerBody(Data.Y_POS yPos)
    {
        List<GameObject> objs = new List<GameObject>();
        objs.Add(leftFoot);
        objs.Add(rightFoot);
        objs.Add(leftKnee);
        objs.Add(rightKnee);
        objs.Add(waist);
        int cnt = 0;
        for (int i = 0; i < yPos.N; i += 9)
        {
            objs[cnt].transform.position = new Vector3(yPos.vec[i + 0], yPos.vec[i + 1], yPos.vec[i + 2]);
            objs[cnt].transform.LookAt(new Vector3(yPos.vec[i + 3], yPos.vec[i + 4], yPos.vec[i + 5]));
            cnt++;
        }
    }

	void Start () {
        loadStdMotion();
	}
	
	void Update () {
        float timestamp = Time.time;
        Data.X_POS xPos = new Data.X_POS(head, leftHand, rightHand);
        Data.Y_POS yPos = new Data.Y_POS(leftFoot, rightFoot, leftKnee, rightKnee, waist);
        record.recordData(timestamp, xPos, yPos);
        bool moving = movingDetect.isMoving(record);
        if (moving)
        {
            float duration = record.getTimestamp(0) - record.getTimestamp(record.getIndex() - movingDetect.getStartIndex());
            Data.Motion motion = stdMotions["long_kick_right"];
            int t = motion.startIndex;
            while (t + 1 <= motion.endIndex && motion.timestamp[t + 1] - motion.timestamp[motion.startIndex] <= duration)
            {
                t++;
            }
            Data.Y_POS stdYPos = motion.yPos[t];
            setLowerBody(new Data.Y_POS(stdYPos + motion.yStart - motion.xStart.getHeadPos() + movingDetect.getStartHeadPos()));
        } else
        {
            Data.Motion motion = stdMotions["long_kick_right"];
            setLowerBody(new Data.Y_POS(motion.yStart - motion.xStart.getHeadPos() + head.transform.position));
        }
	}
}
