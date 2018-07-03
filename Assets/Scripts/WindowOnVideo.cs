using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class WindowOnVideo : MonoBehaviour {
    public Texture2D marker;
    public Vector2 limits = new Vector2(78, 8.65f);
    public Vector2 size = new Vector2(16, 9);
    public float speed = 1.0f;
    public BoxCollider colliderGameObject = null;
    public GameObject insideBox = null;
    private Texture webcam;
    private Renderer render = null;
    private BackgroundVideo background;
    private BackgroundPlaneBehaviour backgroundPlane;
    private float scale = 1;
    private float offsetX = 0;
    private float offsetY = 0;
    // Use this for initialization
    void Start () {
        background = Camera.main.GetComponentInChildren<BackgroundVideo>();
        render = gameObject.GetComponent<Renderer>();
        render.material.mainTexture = marker;  
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
        if (Scenario.step > (int)Scenario.Steps.Started && render.material.mainTexture != webcam)
        {
            webcam = backgroundPlane.Material.mainTexture;
            render.material.mainTexture = webcam;
        }
        else if (Scenario.step <= (int) Scenario.Steps.Started && render.material.mainTexture != marker)
            render.material.mainTexture = marker;
        if (Scenario.step <= (int) Scenario.Steps.Started)
            return;
        if (Scenario.step == (int)Scenario.Steps.End && !Scenario.useFingers)
        {
            float step = speed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(270, 180, 0)), step * 35);
            transform.position += ((Camera.main.transform.position + Camera.main.transform.forward * 4.5f) - transform.position) * step;
        }
        //compute scale with ease out
        if (!Scenario.useFingers)
            scale = Mathf.Clamp01(Mathf.Pow((limits.x - (Mathf.Pow(transform.position.z, 3) - limits.y)) / limits.x, 3 * (limits.x - (Mathf.Pow(transform.position.z, 3) - limits.y)) / limits.x));
        else
            scale = Mathf.Pow((limits.x - (Mathf.Pow(transform.parent.position.z, 2) - limits.y)) / limits.x, 7 * (limits.x - (Mathf.Pow(transform.parent.position.z, 2) - limits.y)) / limits.x);
        render.material.mainTextureScale = new Vector2(scale, scale);
        if (!Scenario.useFingers)
        {
            offsetX = 0.5f * (1 - scale);
            offsetY = 0.5f * (1 - scale);
        }
        else
        {
            offsetX = 0.5f * (1 - scale) + transform.parent.position.x / (size.x * (1 - scale));
            offsetY = 0.5f * (1 - scale) - transform.parent.position.y / (size.y * (1 - scale));
        }
        render.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }

    
}
