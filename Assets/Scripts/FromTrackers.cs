using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromTrackers : MonoBehaviour {
    public Transform head;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform leftKnee;
    public Transform rightKnee;
    public Transform leftHand;
    public Transform rightHand;
    public Transform waist;

    public Transform headTarget;
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform leftKneeTarget;
    public Transform rightKneeTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform wasitTarget;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        head.localPosition = headTarget.localPosition;
        head.localRotation = headTarget.localRotation;
        leftFoot.localPosition = leftFootTarget.localPosition;
        leftFoot.localRotation = leftFootTarget.localRotation;
        rightFoot.localPosition = rightFootTarget.localPosition;
        rightFoot.localRotation = rightFootTarget.localRotation;
        leftKnee.localPosition = leftKneeTarget.localPosition;
        leftKnee.localRotation = leftKneeTarget.localRotation;
        rightKnee.localPosition = rightKneeTarget.localPosition;
        rightKnee.localRotation = rightKneeTarget.localRotation;
        leftHand.localPosition = leftHandTarget.localPosition;
        leftHand.localRotation = leftHandTarget.localRotation;
        rightHand.localPosition = rightHandTarget.localPosition;
        rightHand.localRotation = rightHandTarget.localRotation;
        waist.localPosition = wasitTarget.localPosition;
        waist.localRotation = wasitTarget.localRotation;
    }
}
