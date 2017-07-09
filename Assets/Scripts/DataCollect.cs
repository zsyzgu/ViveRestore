using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataCollect : MonoBehaviour
{
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public Text taskScreen;
    public Text timeScreen;
    public RectTransform timePanel;
    public RectTransform timePanelBackground;
    public float timeInterval = 5f;
    public float totalTime = 104f;
    public string[] tasks;

    private StreamWriter sw;
    private int taskIndex = 0;
    private bool recording = false;
    private float startTime = 0f;

    void rotToVec()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            StreamReader sr = File.OpenText("data/" + tasks[i] + ".txt");
            sw = File.CreateText("data2/" + tasks[i] + ".txt");
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                string[] tags = str.Split(' ');
                float elapsedTime = float.Parse(tags[0]);
                head.transform.position = new Vector3(float.Parse(tags[1]), float.Parse(tags[2]), float.Parse(tags[3]));
                head.transform.eulerAngles = new Vector3(float.Parse(tags[4]), float.Parse(tags[5]), float.Parse(tags[6]));
                leftHand.transform.position = new Vector3(float.Parse(tags[7]), float.Parse(tags[8]), float.Parse(tags[9]));
                leftHand.transform.eulerAngles = new Vector3(float.Parse(tags[10]), float.Parse(tags[11]), float.Parse(tags[12]));
                rightHand.transform.position = new Vector3(float.Parse(tags[13]), float.Parse(tags[14]), float.Parse(tags[15]));
                rightHand.transform.eulerAngles = new Vector3(float.Parse(tags[16]), float.Parse(tags[17]), float.Parse(tags[18]));
                Vector3 hPos = head.transform.position / 100;
                Vector3 hPos2 = hPos - head.transform.up * 0.2f;
                Vector3 lPos = leftHand.transform.position / 100;
                Vector3 lPos2 = lPos - leftHand.transform.forward * 0.2f;
                Vector3 rPos = rightHand.transform.position / 100;
                Vector3 rPos2 = rPos - rightHand.transform.forward * 0.2f;
                sw.Write(elapsedTime + " ");
                sw.Write(hPos.x + " " + hPos.y + " " + hPos.z + " ");
                sw.Write(hPos2.x + " " + hPos2.y + " " + hPos2.z + " ");
                sw.Write(lPos.x + " " + lPos.y + " " + lPos.z + " ");
                sw.Write(lPos2.x + " " + lPos2.y + " " + lPos2.z + " ");
                sw.Write(rPos.x + " " + rPos.y + " " + rPos.z + " ");
                sw.Write(rPos2.x + " " + rPos2.y + " " + rPos2.z);
                sw.WriteLine();
            }
            sw.Close();
        }
    }

    void Start()
    {
        //rotToVec();
    }

    void OnDestroy()
    {
        if (sw != null)
        {
            sw.Close();
        }
    }

    bool isTrigger()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        if (rightHand != null)
        {
            int indexLeft = (int)rightHand.GetComponent<SteamVR_TrackedObject>().index;
            if (indexLeft != -1 && SteamVR_Controller.Input(indexLeft).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                return true;
            }
            int indexRight = (int)leftHand.GetComponent<SteamVR_TrackedObject>().index;
            if (indexRight != -1 && SteamVR_Controller.Input(indexRight).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                return true;
            }
        }
        return false;
    }

    Vector3 calibrateAngle(Vector3 rot)
    {
        if (rot.x >= 180)
        {
            rot.x -= 360;
        }
        if (rot.y >= 180)
        {
            rot.y -= 360;
        }
        if (rot.z >= 180)
        {
            rot.z -= 360;
        }
        return rot;
    }

    void outputRot()
    {
        Vector3 headPosition = new Vector3();
        Vector3 headRotation = new Vector3();
        Vector3 leftHandPosition = new Vector3();
        Vector3 leftHandRotation = new Vector3();
        Vector3 rightHandPosition = new Vector3();
        Vector3 rightHandRotation = new Vector3();

        if (head != null)
        {
            headPosition = head.transform.position;
            headRotation = head.transform.eulerAngles;
            headRotation = calibrateAngle(headRotation);
        }

        if (leftHand != null)
        {
            leftHandPosition = leftHand.transform.position;
            leftHandRotation = leftHand.transform.eulerAngles;
            leftHandRotation = calibrateAngle(leftHandRotation);
        }

        if (rightHand != null)
        {
            rightHandPosition = rightHand.transform.position;
            rightHandRotation = rightHand.transform.eulerAngles;
            rightHandRotation = calibrateAngle(rightHandRotation);
        }

        float elapsedTime = Time.time - startTime;
        sw.Write(elapsedTime + " ");
        sw.Write(headPosition.x + " " + headPosition.y + " " + headPosition.z + " ");
        sw.Write(headRotation.x + " " + headRotation.y + " " + headRotation.z + " ");
        sw.Write(leftHandPosition.x + " " + leftHandPosition.y + " " + leftHandPosition.z + " ");
        sw.Write(leftHandRotation.x + " " + leftHandRotation.y + " " + leftHandRotation.z + " ");
        sw.Write(rightHandPosition.x + " " + rightHandPosition.y + " " + rightHandPosition.z + " ");
        sw.Write(rightHandRotation.x + " " + rightHandRotation.y + " " + rightHandRotation.z);
        sw.WriteLine();
    }

    void outputVec()
    {
        Vector3 hPos = new Vector3();
        Vector3 hPos2 = new Vector3();
        Vector3 lPos = new Vector3();
        Vector3 lPos2 = new Vector3();
        Vector3 rPos = new Vector3();
        Vector3 rPos2 = new Vector3();

        if (head != null)
        {
            hPos = head.transform.position;
            hPos2 = hPos - head.transform.up * 0.2f;
        }

        if (leftHand != null)
        {
            lPos = leftHand.transform.position;
            lPos2 = lPos - leftHand.transform.forward * 0.2f;
        }

        if (rightHand != null)
        {
            rPos = rightHand.transform.position;
            rPos2 = rPos - rightHand.transform.forward * 0.2f;
        }

        float elapsedTime = Time.time - startTime;
        sw.Write(elapsedTime + " ");
        sw.Write(hPos.x + " " + hPos.y + " " + hPos.z + " ");
        sw.Write(hPos2.x + " " + hPos2.y + " " + hPos2.z + " ");
        sw.Write(lPos.x + " " + lPos.y + " " + lPos.z + " ");
        sw.Write(lPos2.x + " " + lPos2.y + " " + lPos2.z + " ");
        sw.Write(rPos.x + " " + rPos.y + " " + rPos.z + " ");
        sw.Write(rPos2.x + " " + rPos2.y + " " + rPos2.z);
        sw.WriteLine();
    }

    void Update()
    {
        if (taskIndex == tasks.Length)
        {
            taskScreen.text = "Completed!";
            return;
        }

        if (isTrigger())
        {
            if (recording == false)
            {
                recording = true;
                sw = File.CreateText("data/" + tasks[taskIndex] + ".txt");
                startTime = Time.time;
            } else
            {
                sw.Close();
                sw = File.CreateText("data/" + tasks[taskIndex] + ".txt");
                startTime = Time.time;
            }
        }

        float elapsedTime = Time.time - startTime;
        if (recording && elapsedTime >= totalTime)
        {
            sw.Close();
            taskIndex++;
            recording = false;
        }
        
        if (recording)
        {
            taskScreen.text = "Recording : " + tasks[taskIndex];

            outputVec();

            timeScreen.text = elapsedTime.ToString();

            float schedule = (elapsedTime - (int)(elapsedTime / timeInterval) * timeInterval) / timeInterval;
            timePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(timePanelBackground.rect.width * schedule, timePanelBackground.rect.height);
        } else
        {
            if (taskIndex < tasks.Length)
            {
                taskScreen.text = "Next task : " + tasks[taskIndex];
            }

            timeScreen.text = "";
        }
    }
}
