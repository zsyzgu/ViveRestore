using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ControlledHuman : MonoBehaviour {
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftFoot;
    public GameObject rightFoot;
    public GameObject leftKnee;
    public GameObject rightKnee;
    public GameObject waist;

    public class Record
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
    protected Record record = new Record();

    public class MovingDetect
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
        private bool pressing = false;

        private void startMoving(Record record)
        {
            startIndex = record.getIndex() - moveFrames;
        }

        public void update(Record record)
        {
            if (Utility.isStart())
            {
                pressing = true;
            }
            if (Utility.isEnd())
            {
                pressing = false;
            }

            if (record.getIndex() >= 1)
            {
                float speed = Mathf.Min(Data.X_POS.handsDistRelatedToHead(record.getXPos(0), record.getXPos(1)), Data.X_POS.handsDistInWorldSpace(record.getXPos(0), record.getXPos(1))) / (record.getTimestamp(0) - record.getTimestamp(1));
                
                if (pressing)
                {
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
                } else
                {
                    moveFrames = stopFrames = 0;
                    moving = false;
                }
            }
        }

        public bool isMoving()
        {
            return moving;
        }

        public int getStartIndex()
        {
            return startIndex;
        }
    }
    protected MovingDetect movingDetect = new MovingDetect();

    protected void Start()
    {
        Utility.leftHand = leftHand;
        Utility.rightHand = rightHand;
    }

    protected void Update()
    {
        float timestamp = Time.time;
        Data.X_POS xPos = new Data.X_POS(head, leftHand, rightHand);
        Data.Y_POS yPos = new Data.Y_POS(leftFoot, rightFoot, leftKnee, rightKnee, waist);
        record.recordData(timestamp, xPos, yPos);
        movingDetect.update(record);
    }

    protected Data.Motion loadMotion(string path, string name)
    {
        Data.Motion motion = new Data.Motion();
        string fileName = path + name + ".txt";
        StreamReader sr = File.OpenText(fileName);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] tags = line.Split(' ');
            motion.readTags(tags);
        }
        motion.preprocess();
        return motion;
    }

    private const float smoothK = 0.8f;
    protected void setLowerBody(Data.Y_POS yPos)
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
            objs[cnt].transform.position = objs[cnt].transform.position * smoothK + new Vector3(yPos.vec[i + 0], yPos.vec[i + 1], yPos.vec[i + 2]) * (1 - smoothK);
            objs[cnt].transform.LookAt(new Vector3(yPos.vec[i + 3], yPos.vec[i + 4], yPos.vec[i + 5]));
            cnt++;
        }
    }   
}
