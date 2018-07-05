using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


//Force activate windowOnVideo when changing parent
public class TrackingContinuity : MonoBehaviour {
    private WindowOnVideo window;
    private ObjectTargetBehaviour objectTarget;
	// Use this for initialization
	void Start () {
        objectTarget = GetComponent<ObjectTargetBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponentInChildren<WindowOnVideo>() && window == null)
            window = GetComponentInChildren<WindowOnVideo>();
        if (window != null && objectTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            window.GetComponent<Renderer>().enabled = true;
            Collider[] colliders = window.GetComponentsInChildren<Collider>();
            foreach (Collider coll in colliders)
            {
                coll.enabled = true;
            }
        }
	}
}
