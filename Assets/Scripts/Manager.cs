using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public GameObject child;
	// Use this for initialization
	void Start () {
	}

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update () {
        child.transform.position = new Vector3(transform.position.x / 5, transform.position.y / 5, 0.4f);
        child.transform.rotation = transform.rotation;
    }
}
