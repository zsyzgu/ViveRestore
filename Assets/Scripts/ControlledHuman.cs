using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ControlledHuman : MonoBehaviour
{
    protected const int CALI_NUM = 3;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftFoot;
    public GameObject rightFoot;
    public GameObject leftKnee;
    public GameObject rightKnee;
    public GameObject waist;
    public string[] motionName;
    public string caliFileName;
    public CaliSkeleton caliSkeleton;

    protected Dictionary<string, Data.Motion> stdMotions = new Dictionary<string, Data.Motion>();
    protected Dictionary<string, List<Data.Motion>> caliMotions = new Dictionary<string, List<Data.Motion>>();
    protected string currMotion;

    public string getCurrMotion()
    {
        return currMotion;
    }

    public class Record
    {
        const int RECORD_FRAMS = 10000;

        private float[] timestamp = new float[RECORD_FRAMS];
        private Data.X_POS[] xPosList = new Data.X_POS[RECORD_FRAMS];
        private Data.Y_POS[] yPosList = new Data.Y_POS[RECORD_FRAMS];
        private Data.X_POS[] xSpeedList = new Data.X_POS[RECORD_FRAMS];
        private Data.Y_POS[] ySpeedList = new Data.Y_POS[RECORD_FRAMS];
        private Data.X_POS[] xPosSmooth = new Data.X_POS[RECORD_FRAMS];
        private Data.Y_POS[] yPosSmooth = new Data.Y_POS[RECORD_FRAMS];
        private Data.X_POS[] xSpeedSmooth = new Data.X_POS[RECORD_FRAMS];
        private Data.Y_POS[] ySpeedSmooth = new Data.Y_POS[RECORD_FRAMS];
        private int index = -1;

        private Data.X_POS xRollingMean(Data.X_POS[] posList)
        {
            int cnt = 1;
            Data.POS pos = posList[index % RECORD_FRAMS];
            Data.POS maxPos = pos;
            Data.POS minPOS = pos;
            for (int i = index - 1; i > index - 9 && i >= 0; i--)
            {
                cnt++;
                Data.POS thisPos = posList[i % RECORD_FRAMS];
                pos = pos + thisPos;
                maxPos = Data.POS.max(maxPos, thisPos);
                minPOS = Data.POS.min(minPOS, thisPos);
            }
            if (cnt > 2)
            {
                pos = (pos - maxPos - minPOS) / (cnt - 2);
            } else
            {
                pos = pos / cnt;
            }
            return new Data.X_POS(pos);
        }

        private Data.Y_POS yRollingMean(Data.Y_POS[] posList)
        {
            int cnt = 1;
            Data.POS pos = posList[index % RECORD_FRAMS];
            Data.POS maxPos = pos;
            Data.POS minPOS = pos;
            for (int i = index - 1; i > index - 9 && i >= 0; i--)
            {
                cnt++;
                Data.POS thisPos = posList[i % RECORD_FRAMS];
                pos = pos + thisPos;
                maxPos = Data.POS.max(maxPos, thisPos);
                minPOS = Data.POS.min(minPOS, thisPos);
            }
            if (cnt > 2)
            {
                pos = (pos - maxPos - minPOS) / (cnt - 2);
            }
            else
            {
                pos = pos / cnt;
            }
            return new Data.Y_POS(pos);
        }

        public void recordData(float t, Data.X_POS xPos, Data.Y_POS yPos)
        {
            index++;
            int i = index % RECORD_FRAMS;
            timestamp[i] = t;
            xPosList[i] = xPos;
            yPosList[i] = yPos;
            if (index - 1 >= 0)
            {
                float timeInterval = timestamp[i] - timestamp[(index - 1) % RECORD_FRAMS];
                xSpeedList[i] = new Data.X_POS((xPosList[i] - xPosList[(index - 1) % RECORD_FRAMS]) / timeInterval);
                ySpeedList[i] = new Data.Y_POS((yPosList[i] = yPosList[(index - 1) % RECORD_FRAMS]) / timeInterval);
            } else
            {
                xSpeedList[i] = new Data.X_POS();
                ySpeedList[i] = new Data.Y_POS();
            }
            xPosSmooth[i] = xRollingMean(xPosList);
            yPosSmooth[i] = yRollingMean(yPosList);
            xSpeedSmooth[i] = xRollingMean(xSpeedList);
            ySpeedSmooth[i] = yRollingMean(ySpeedList);
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

        public float[] getHMMVector()
        {
            float[] vec = new float[14];
            for (int i = 0; i < 14; i++)
            {
                vec[i] = xSpeedList[index % RECORD_FRAMS].vec[i + 7];
                /*if (i % 7 < 3)
                {
                    vec[i] = xSpeedSmooth[index % RECORD_FRAMS].vec[i + 7];
                } else
                {
                    vec[i] = xPosSmooth[index % RECORD_FRAMS].vec[i + 7];
                }*/
            }
            return vec;
        }

        public Data.X_POS getXSpeedSmooth(int frame)
        {
            if (frame > index)
            {
                Debug.Log("Not recorded");
                return new Data.X_POS();
            }
            return xSpeedSmooth[(index - frame) % RECORD_FRAMS];
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
        private bool firstMove = false;
        private int startIndex = 0;
        private bool pressing = false;
        private bool needPressing = true;

        private void startMoving(Record record)
        {
            startIndex = record.getIndex() - moveFrames;
        }

        public void setNeedPressing(bool value)
        {
            needPressing = value;
        }

        public void update(Record record)
        {
            firstMove = false;
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
                float speed = Mathf.Min(Data.X_POS.handsDistToHead(record.getXPos(0), record.getXPos(1)), Data.X_POS.handsDist(record.getXPos(0), record.getXPos(1))) / (record.getTimestamp(0) - record.getTimestamp(1));
                
                if (pressing || needPressing == false)
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
                                firstMove = true;
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

        public bool isFirstMove()
        {
            return firstMove;
        }

        public int getStartIndex()
        {
            return startIndex;
        }
    }
    protected MovingDetect movingDetect = new MovingDetect();

    private int randomNumber;
    public void logCurrMotion(int id)
    {
        if (movingDetect.isMoving())
        {
            Data.Motion motion = new Data.Motion();
            motion.formMotion(record, movingDetect.getStartIndex(), record.getIndex());
            motion.output("Track/track" + randomNumber + "_.txt", currMotion, id);
        }
    }

    protected void Start()
    {
        randomNumber = Random.Range(0, 1000000000);
        Utility.leftHand = leftHand;
        Utility.rightHand = rightHand;
        loadMotions();
        if (motionName.Length > 0)
        {
            currMotion = motionName[0];
        }
    }

    protected void Update()
    {
        float timestamp = Time.time;
        Data.X_POS xPos = new Data.X_POS(head, leftHand, rightHand);
        Data.Y_POS yPos = new Data.Y_POS(leftFoot, rightFoot, leftKnee, rightKnee, waist);
        record.recordData(timestamp, xPos, yPos);
        movingDetect.update(record);
        updateCaliSkeleton();
    }

    protected Data.Motion loadStdMotion(string fileName)
    {
        Data.Motion motion = new Data.Motion();
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

    protected Data.Motion loadCaliMotion(string fileName, string motionName, int id)
    {
        Data.Motion motion = new Data.Motion();
        StreamReader sr = File.OpenText(fileName);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] tags = line.Split(' ');
            if (tags[0] == motionName && int.Parse(tags[1]) == id)
            {
                motion.readTags(tags, 2);
            }
        }
        motion.preprocess();
        return motion;
    }

    private const float smoothK = 0.5f;
    protected void setLowerBody(Data.Y_POS yPos)
    {
        List<GameObject> objs = new List<GameObject>();
        objs.Add(leftFoot);
        objs.Add(rightFoot);
        objs.Add(leftKnee);
        objs.Add(rightKnee);
        objs.Add(waist);
        int cnt = 0;
        for (int i = 0; i < yPos.N; i += 7)
        {
            objs[cnt].transform.position = objs[cnt].transform.position * smoothK + new Vector3(yPos.vec[i + 0], yPos.vec[i + 1], yPos.vec[i + 2]) * (1 - smoothK);
            objs[cnt].transform.rotation = new Quaternion(yPos.vec[i + 3], yPos.vec[i + 4], yPos.vec[i + 5], yPos.vec[i + 6]);
            cnt++;
        }
    }

    protected void loadMotions()
    {
        foreach (string name in motionName)
        {
            stdMotions[name] = loadStdMotion("Std/" + name + ".txt");
            caliMotions[name] = new List<Data.Motion>();
            for (int i = 0; i < CALI_NUM; i++)
            {
                caliMotions[name].Add(loadCaliMotion("Cali/" + caliFileName + ".txt", name, i));
            }
        }
    }

    protected void resetCaliMotions()
    {
        foreach (string name in motionName)
        {
            for (int i = 0; i < CALI_NUM; i++)
            {
                caliMotions[name][i].resetMotion();
            }
        }
    }

    private void updateCaliSkeleton()
    {
        if (caliSkeleton != null)
        {
            caliSkeleton.setMotion(stdMotions[currMotion]);
            if (movingDetect.isFirstMove())
            {
                caliSkeleton.playMotion(record.getXPos(0));
            }
        }
    }

    protected void updateHMM()
    {
        if (movingDetect.isMoving())
        {
            if (movingDetect.isFirstMove())
            {
                HmmClient.hmmStart();
            }
            HmmClient.newFrame(record.getHMMVector());
            HmmClient.getAction();
            if (HmmClient.Action != "")
            {
                currMotion = HmmClient.Action;
            }
        }
    }

    protected void retrieval()
    {
        if (movingDetect.isMoving() || currMotion == "walking")
        {
            if (movingDetect.isFirstMove())
            {
                resetCaliMotions();
            }
            float minScore = 1e9f;
            float predictFrame = 1f;
            foreach (string name in motionName)
            {
                for (int i = 0; i < CALI_NUM; i++)
                {
                    float score = 0f;
                    float frame = caliMotions[name][i].predictMotionFrame(record, out score);
                    if (currMotion == name && score < minScore)
                    {
                        minScore = score;
                        predictFrame = frame / caliMotions[name][i].timestamp.Count * stdMotions[currMotion].timestamp.Count;
                    }
                    if (name == "walking" || name == "running")
                    {
                        break;
                    }
                }
            }
            Data.Y_POS predictYPos = stdMotions[currMotion].getYPos(predictFrame);
            setLowerBody(new Data.Y_POS(predictYPos + stdMotions[currMotion].yStart));
        }
        else
        {
            setLowerBody(stdMotions[currMotion].yStart);
        }
    }
    
    void OnDestroy()
    {
        Net.closeSocket();
    }
}
