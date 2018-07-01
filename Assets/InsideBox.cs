using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideBox : MonoBehaviour {
    private WindowOnVideo window;
	// Use this for initialization
	void Start () {
        window = GetComponentInParent<WindowOnVideo>();
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
