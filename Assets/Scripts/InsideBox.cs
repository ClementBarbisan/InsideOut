using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BoxCollider use to activate/deactivate custom shaders
public class InsideBox : MonoBehaviour {
    public Vector3[] sizeCollider;
    private WindowOnVideo window;
    private BoxCollider coll;

	// Use this for initialization
	void Start () {
        window = GetComponentInParent<WindowOnVideo>();
        coll = GetComponent<BoxCollider>();
        if (Scenario.Instance.useFingers)
            coll.size = sizeCollider[1];
        else
            coll.size = sizeCollider[0];
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != this.transform.parent.gameObject)
            window.insideBox = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == window.insideBox)
            window.insideBox = null;
    }
}
