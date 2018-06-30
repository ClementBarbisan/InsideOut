using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class WindowOnVideo : MonoBehaviour {
    public Texture2D marker;
    public Vector2 limits = new Vector2(78, 8.65f);
    public Vector2 size = new Vector2(16, 9);
    private Texture webcam;
    private Renderer renderer = null;
    private BackgroundVideo background;
    private BackgroundPlaneBehaviour backgroundPlane;
    private float scale = 1;
    private float offsetX = 0;
    private float offsetY = 0;
    // Use this for initialization
    void Start () {
        background = Camera.main.GetComponentInChildren<BackgroundVideo>();
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material.mainTexture = marker;  
        size.x = background.gameObject.transform.localScale.x;
        size.y = background.gameObject.transform.localScale.z;
        limits.x = background.transform.position.z;
        limits.y = Mathf.Sqrt(Mathf.Pow(size.y / 2, 2) + Mathf.Pow(limits.x, 2)) / (size.y) * transform.parent.localScale.z * transform.localScale.z;
        backgroundPlane = Camera.main.GetComponentInChildren<BackgroundPlaneBehaviour>(); 
    }
	
	// Update is called once per frame
	void Update () {
        limits.x = background.transform.position.z;
        if (backgroundPlane.Material.mainTexture != null && webcam == null)
            webcam = backgroundPlane.Material.mainTexture;
        if (Scenario.step >= (int)Scenario.Steps.Started && renderer.material.mainTexture != webcam)
        {
            webcam = backgroundPlane.Material.mainTexture;
            renderer.material.mainTexture = webcam;
        }
        else if (Scenario.step < (int)Scenario.Steps.Started && renderer.material.mainTexture != marker)
            renderer.material.mainTexture = marker;
        if (Scenario.step < (int)Scenario.Steps.Started)
            return;
        //compute scale with ease out
        if (transform.parent == Camera.main.transform)
            scale = Mathf.Clamp01(Mathf.Pow(1 - Mathf.Clamp01(transform.position.z - limits.y), 5 * (1 - Mathf.Clamp01(transform.position.z - limits.y))));
        else
            scale = Mathf.Clamp01(Mathf.Pow(1 - Mathf.Clamp01(transform.parent.position.z - limits.y), 5 * (1 - Mathf.Clamp01(transform.parent.position.z - limits.y))));
        renderer.material.mainTextureScale = new Vector2(scale, scale);
        //offsetX = 0.5f * (1 - scale) - transform.parent.position.x / size.x * Mathf.Pow((1 - scale), 5 * (1 - scale));
        //offsetY = 0.5f * (1 - scale) - transform.parent.position.y / size.y * Mathf.Pow((1 - scale), 5 * (1 - scale));
        if (transform.parent == Camera.main.transform)
        {
            return;
        }
        else
        {
            offsetX = 0.5f * (1 - scale) + transform.parent.position.x / transform.localScale.x * Mathf.Pow((1 - scale), 5 * (1 - scale));
            offsetY = 0.5f * (1 - scale) - transform.parent.position.y / transform.localScale.z * Mathf.Pow((1 - scale), 5 * (1 - scale));
            renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
    }
}
