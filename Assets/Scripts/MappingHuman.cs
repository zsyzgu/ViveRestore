using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappingHuman : MonoBehaviour {
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject originLeftHand;
    public GameObject originRightHand;

	void Start () {
		
	}
	
	void Update () {
        if (originLeftHand != null)
        {
            leftHand.transform.localPosition = originLeftHand.transform.position;
            leftHand.transform.localEulerAngles = originLeftHand.transform.eulerAngles;
        }
        if (originRightHand != null)
        {
            rightHand.transform.localPosition = originRightHand.transform.position;
            rightHand.transform.localEulerAngles = originRightHand.transform.eulerAngles;
        }
	}
}
