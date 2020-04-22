using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
//A little window on backgroundVideo
public class WindowOnVideo : MonoBehaviour {
    public Texture2D marker;
    public Vector2 limits = new Vector2(78, 8.65f);
    public Vector2 size = new Vector2(16, 9);
    public float speed = 1.0f;
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
        //Maximum position on z
        limits.x = background.transform.position.z;
        //Compute minimum position on z
        limits.y = Mathf.Sqrt(Mathf.Pow(size.y / 2, 2) + Mathf.Pow(limits.x, 2)) / (size.y) * transform.parent.localScale.z * transform.localScale.z;
        backgroundPlane = Camera.main.GetComponentInChildren<BackgroundPlaneBehaviour>();
    }

    private void LateUpdate()
    {
        //Set this to be in front of the player
        if (Scenario.Instance.useFingers)
        {
            transform.rotation = Quaternion.Euler(270, 0, 180);
        }
    }

    // Update is called once per frame
    void Update () {
        //Update maximum position z due to BackgroundPlaneBehaviour
        limits.x = background.transform.position.z;
        if (backgroundPlane == null)
            backgroundPlane = Camera.main.GetComponentInChildren<BackgroundPlaneBehaviour>();
        if (backgroundPlane.Material.mainTexture != null && webcam == null)
            webcam = backgroundPlane.Material.mainTexture;
        if (Scenario.Instance.step > (int)Scenario.Steps.Started && render.material.mainTexture != webcam)
        {
            webcam = backgroundPlane.Material.mainTexture;
            render.material.mainTexture = webcam;
        }
        else if (Scenario.Instance.step <= (int) Scenario.Steps.Started && render.material.mainTexture != marker)
            render.material.mainTexture = marker;
        if (Scenario.Instance.step <= (int) Scenario.Steps.Started)
            return;
        //Smooth movement to the center of the screen
        if (Scenario.Instance.step == (int)Scenario.Steps.End && !Scenario.Instance.useFingers
            && (transform.position != Camera.main.transform.position + Camera.main.transform.forward * 4.5f || transform.rotation != Quaternion.Euler(new Vector3(270, 180, 0))))
        {
            float step = speed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(270, 180, 0)), step * 35);
            transform.position += ((Camera.main.transform.position + Camera.main.transform.forward * 4.5f) - transform.position) * step;
        }
        ////compute scale texture
        //if (!Scenario.Instance.useFingers)
        //    scale = limits.y / transform.position.z;
        //else
        //    scale = Mathf.Clamp01(limits.y / transform.parent.position.z);
        //render.material.mainTextureScale = new Vector2(scale, scale);
        ////Compute offset texture
        //if (!Scenario.Instance.useFingers)
        //{
        //    offsetX = 0.5f * (1 - scale);
        //    offsetY = 0.5f * (1 - scale);
        //}
        //else
        //{
        //    //Formulae are some guesswork
        //    offsetX = Mathf.Clamp01(0.5f * (1 - scale) + (transform.position.x * transform.parent.localScale.x) / (size.x * (1 - scale)) + (transform.parent.position.x * transform.localScale.x) / (size.x * (1 - scale)));
        //    offsetY = Mathf.Clamp01(0.5f * (1 - scale) - (transform.position.y * transform.parent.localScale.z) / (size.y * (1 - scale)) - (transform.parent.position.y * transform.localScale.z) / (size.y * (1 - scale)));
        //}
        //render.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }

    
}
