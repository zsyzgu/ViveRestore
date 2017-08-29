﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Utility : MonoBehaviour
{
    static public GameObject leftHand;
    static public GameObject rightHand;

    static public bool isStart()
    {
        if (leftHand != null)
        {
            int index = (int)leftHand.GetComponent<SteamVR_TrackedObject>().index;
            if (index != -1 && SteamVR_Controller.Input(index).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                return true;
            }
        }
        if (rightHand != null)
        {
            int index = (int)rightHand.GetComponent<SteamVR_TrackedObject>().index;
            if (index != -1 && SteamVR_Controller.Input(index).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                return true;
            }
        }
        return false;
    }

    static public bool isPress()
    {
        if (leftHand != null)
        {
            int index = (int)leftHand.GetComponent<SteamVR_TrackedObject>().index;
            if (index != -1 && SteamVR_Controller.Input(index).GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                return true;
            }
        }
        if (rightHand != null)
        {
            int index = (int)rightHand.GetComponent<SteamVR_TrackedObject>().index;
            if (index != -1 && SteamVR_Controller.Input(index).GetPress(SteamVR_Controller.ButtonMask.Trigger))
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
