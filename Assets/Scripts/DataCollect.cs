using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataCollect : MonoBehaviour
{
    public GameObject objectHead;
    public GameObject objectLeftHand;
    public GameObject objectRightHand;
    
    void Start()
    {

    }

    string getDataInfo()
    {
        float shiftDistance = 0.2f;

        Vector3 headPosition = new Vector3();
        Vector3 headPositionUp = new Vector3();
        Vector3 headPositionForward = new Vector3();

        if (objectHead != null)
        {
            headPosition = objectHead.transform.position;
            headPositionUp = headPosition + objectHead.transform.up * shiftDistance;
            headPositionForward = headPosition + objectHead.transform.forward * shiftDistance;
        } else
        {
            Debug.Log("HMD is missing.");
        }

        Vector3 leftHandPosition = new Vector3();
        Vector3 leftHandPositionUp = new Vector3();
        Vector3 leftHandPositionForward = new Vector3();

        if (objectLeftHand != null)
        {
            leftHandPosition = objectLeftHand.transform.position;
            leftHandPositionUp = leftHandPosition + objectLeftHand.transform.up * shiftDistance;
            leftHandPositionForward = leftHandPosition + objectLeftHand.transform.forward * shiftDistance;
        } else
        {
            Debug.Log("Left controller is missing.");
        }

        Vector3 rightHandPosition = new Vector3();
        Vector3 rightHandPositionUp = new Vector3();
        Vector3 rightHandPositionForward = new Vector3();

        if (objectRightHand != null)
        {
            rightHandPosition = objectRightHand.transform.position;
            rightHandPositionUp = rightHandPosition + objectRightHand.transform.up * shiftDistance;
            rightHandPositionForward = rightHandPosition + objectRightHand.transform.forward * shiftDistance;
        } else
        {
            Debug.Log("Right controller is missing.");
        }

        string info = headPosition.x + " " + headPosition.y + " " + headPosition.z;
        info += " " + headPositionUp.x + " " + headPositionUp.y + " " + headPositionUp.z;
        info += " " + headPositionForward.x + " " + headPositionForward.y + " " + headPositionForward.z;
        info += " " + leftHandPosition.x + " " + leftHandPosition.y + " " + leftHandPosition.z;
        info += " " + leftHandPositionUp.x + " " + leftHandPositionUp.y + " " + leftHandPositionUp.z;
        info += " " + leftHandPositionForward.x + " " + leftHandPositionForward.y + " " + leftHandPositionForward.z;
        info += " " + rightHandPosition.x + " " + rightHandPosition.y + " " + rightHandPosition.z;
        info += " " + rightHandPositionUp.x + " " + rightHandPositionUp.y + " " + rightHandPositionUp.z;
        info += " " + rightHandPositionForward.x + " " + rightHandPositionForward.y + " " + rightHandPositionForward.z;

        return info;
    }

    void Update()
    {
        if (Utility.isStart(objectLeftHand, objectRightHand))
        {
            TaskManager.start();
        }

        if (Utility.isRestart())
        {
            TaskManager.restart();
        }

        if (TaskManager.isRecording())
        {
            string taskInfo = TaskManager.getTaskInfo();
            string dataInfo = getDataInfo();
            string info = taskInfo + " " + dataInfo;
            FileLogger.log(info);
        }
    }
}
