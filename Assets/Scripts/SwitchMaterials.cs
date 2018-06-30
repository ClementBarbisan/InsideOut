using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterials : MonoBehaviour {
    public Material[] initialMaterials;
    public Material[] overrideMaterial;
    private Renderer renderer;
    private WindowOnVideo window;
    // Use this for initialization
	void Awake () {
        renderer = GetComponent<Renderer>();
        window = GameObject.FindObjectOfType<WindowOnVideo>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, window.gameObject.transform.position - Camera.main.transform.position);
        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "End")
            renderer.materials = initialMaterials;
        else
            renderer.materials = overrideMaterial;
	}
}
