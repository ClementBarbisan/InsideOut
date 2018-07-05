using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroy automatically last vase when tracked and on normal material
public class BreakVase : MonoBehaviour {
    private SwitchMaterials materials;
	// Use this for initialization
	void Start () {
        if (!Scenario.Instance.useFingers)
            this.enabled = false;
        materials = GetComponent<SwitchMaterials>();
	}
	
	// Update is called once per frame
	void Update () {
        if (materials.isBreakable)
            GetComponentInParent<BreakableObject>().ClickToDestroy();
	}
}
