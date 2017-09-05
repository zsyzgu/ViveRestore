using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FootController : ControlledHuman {
    new void Start () {
        base.Start();
	}
	
	new void Update () {
        base.Update();

        updateHMM();
        retrieval();
	}
}
