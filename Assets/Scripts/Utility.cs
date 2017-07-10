using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Utility : MonoBehaviour
{
    static public bool isStart(GameObject objectLeftHand, GameObject objectRightHand)
    {
        if (objectLeftHand != null)
        {
            int index = (int)objectLeftHand.GetComponent<SteamVR_TrackedObject>().index;
            if (index != -1 && SteamVR_Controller.Input(index).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                return true;
            }
        }
        if (objectRightHand != null)
        {
            int index = (int)objectRightHand.GetComponent<SteamVR_TrackedObject>().index;
            if (index != -1 && SteamVR_Controller.Input(index).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                return true;
            }
        }
        return false;
    }

    static public bool isRestart()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }
}
