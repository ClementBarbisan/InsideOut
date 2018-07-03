using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public GameObject childCollider;
    public GameObject window;
    // Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update () {
        childCollider.transform.localPosition = new Vector3(transform.position.x / 5, transform.position.y / 5, transform.position.z / 10);
    }
}
