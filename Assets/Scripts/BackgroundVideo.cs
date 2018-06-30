using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using Vuforia;

public class BackgroundVideo : MonoBehaviour
{
    public Material normal;
    public Material distord;
    private Renderer renderer = null;
    private BackgroundPlaneBehaviour backgroundPlane;
    // Use this for initialization
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material = normal;
        backgroundPlane = Camera.main.GetComponentInChildren<BackgroundPlaneBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0, transform.localScale.z / 2 / Mathf.Tan(Mathf.Deg2Rad * (Camera.main.fieldOfView / 2)) * 10);
        if (backgroundPlane.Material.mainTexture != null && renderer.material.mainTexture == null)
            renderer.material.mainTexture = backgroundPlane.Material.mainTexture;
        if (Scenario.step == (int)Scenario.Steps.Started && renderer.material != distord)
        {
            renderer.material = distord;
            renderer.material.mainTexture = backgroundPlane.Material.mainTexture;
        }
        if (Scenario.step > (int)Scenario.Steps.End && renderer.material != normal)
        {
            renderer.material = normal;
            renderer.material.mainTexture = backgroundPlane.Material.mainTexture;
        }
    }
}