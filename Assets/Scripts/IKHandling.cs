using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandling : MonoBehaviour {
    Animator anim;

    public float IKWeight = 1f;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform leftKnee;
    public Transform rightKnee;
    public Transform waist;
    public float height = 1.58f;
    private float stdHeight = 1.58f;
    
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	void Update () {
        transform.localScale = new Vector3(height / stdHeight, height / stdHeight, height / stdHeight);
	}

    void OnAnimatorIK()
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, IKWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, IKWeight);
        anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot.position);
        anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot.position);
        anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFoot.rotation);
        anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFoot.rotation);

        anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, IKWeight);
        anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, IKWeight);
        anim.SetIKHintPosition(AvatarIKHint.LeftKnee, leftKnee.position);
        anim.SetIKHintPosition(AvatarIKHint.RightKnee, rightKnee.position);

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, IKWeight);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);

        anim.bodyPosition = waist.position;
        //anim.bodyRotation = waist.rotation;
    }
}
