using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterials : MonoBehaviour {
    public Material[] initialMaterials;
    public Material[] overrideMaterial;
    public bool isBreakable = false;
    private Renderer render;
    private WindowOnVideo window;
    // Use this for initialization
	void Awake () {
        render = GetComponent<Renderer>();
        window = GameObject.FindObjectOfType<WindowOnVideo>();
    }

    // Update is called once per frame
    void Update () {
        if (window.insideBox == this.gameObject)
        {
            isBreakable = true;
            render.materials = initialMaterials;
        }
        else
        {
            isBreakable = false;
            render.materials = overrideMaterial;
        }
    }
}
