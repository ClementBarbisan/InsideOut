using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakVase : MonoBehaviour {
    private SwitchMaterials materials;
	// Use this for initialization
	void Start () {
        if (!Scenario.useFingers)
            this.enabled = false;
        materials = GetComponent<SwitchMaterials>();
	}
	
	// Update is called once per frame
	void Update () {
        if (materials.isBreakable)
            GetComponentInParent<BreakableObject>().ClickToDestroy();
	}
}
