using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using Vuforia;

//On front of BackgroundPlaneBehaviour(Doesn't work on Android if deactivate) to change material
public class BackgroundVideo : MonoBehaviour
{
    public Material normal;
    public Material distord;
    private Renderer render = null;
    private BackgroundPlaneBehaviour backgroundPlane;
    // Use this for initialization
    void Start()
    {
        render = gameObject.GetComponent<Renderer>();
        render.material = normal;
        backgroundPlane = Camera.main.GetComponentInChildren<BackgroundPlaneBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        //calcul position to fill the screen (by default backgroundPlane change fov)
        transform.position = new Vector3(0, 0, transform.localScale.z / 2 / Mathf.Tan(Mathf.Deg2Rad * (Camera.main.fieldOfView / 2)) * 10);
        if (backgroundPlane.Material.mainTexture != null && render.material.mainTexture == null)
            render.material.mainTexture = backgroundPlane.Material.mainTexture;
        if (Scenario.Instance.step >= (int)Scenario.Steps.Started && render.material != distord && !Scenario.Instance.debugWindow)
        {
            render.material = distord;
            render.material.mainTexture = backgroundPlane.Material.mainTexture;
        }
        if (Scenario.Instance.step > (int)Scenario.Steps.End && render.material != normal)
        {
            render.material = normal;
            render.material.mainTexture = backgroundPlane.Material.mainTexture;
        }
    }
}