using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastToBreak : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(Camera.main.transform.position, transform.position - Camera.main.transform.position);
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(Camera.main.transform.position, transform.position - Camera.main.transform.position);
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<BreakableObject>())
            hit.collider.gameObject.GetComponent<BreakableObject>().ClickToDestroy();
	}
}
