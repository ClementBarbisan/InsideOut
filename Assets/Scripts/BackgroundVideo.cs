using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class BackgroundVideo : MonoBehaviour
{
    public Material normal;
    public Material distord;
    public WebCamTexture cam = null;
    private Renderer renderer = null;
    // Use this for initialization
    void Awake()
    {
        cam = new WebCamTexture();
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material = normal;
        renderer.material.mainTexture = cam;
        cam.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}