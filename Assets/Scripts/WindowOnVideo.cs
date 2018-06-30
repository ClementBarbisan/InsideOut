using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowOnVideo : MonoBehaviour {
    public Vector2 limits = new Vector2(78, 8.65f);
    public Vector2 size = new Vector2(16, 9);
    private Renderer renderer = null;
    private float scale = 1;
    private float offsetX = 0;
    private float offsetY = 0;
    // Use this for initialization
    void Start () {
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material.mainTexture = GameObject.FindObjectOfType<BackgroundVideo>().cam;
    }
	
	// Update is called once per frame
	void Update () {
        //compute scale with ease out
        scale = Mathf.Clamp01(Mathf.Pow(1 - (transform.position.z - limits.y) / limits.x, 6 * (1 - (transform.position.z - limits.y) / limits.x)));
        renderer.material.mainTextureScale = new Vector2(scale, scale);
        offsetX = 0.5f * (1 - scale) + transform.position.x / size.x * Mathf.Pow(scale, 4 * scale);
        offsetY = 0.5f * (1 - scale) + transform.position.y / size.y * Mathf.Pow(scale, 4 * scale);
        renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
