using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour {
    public GameObject End0;
    public GameObject End1;
	
	void Update () {
        Vector3 p0 = End0.transform.position;
        Vector3 p1 = End1.transform.position;

        transform.position = (p1 - p0) / 2.0f + p0;
        Vector3 v3T = transform.localScale;
        v3T.y = (p1 - p0).magnitude / 2.0f;
        transform.localScale = v3T;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, p1 - p0);
    }
}
