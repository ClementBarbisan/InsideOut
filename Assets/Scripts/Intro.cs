using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour {
    public Image circle;
    private bool isLoading = false;
    public void StartApp(string name)
    {
        if (isLoading)
            return;
        isLoading = true;
        PlayerPrefs.SetString("version", name);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(1);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (isLoading)
        {
            if (!circle.enabled)
                circle.enabled = true;
            if (circle.fillAmount < 1 && circle.fillClockwise)
                circle.fillAmount += 0.05f;
            else if (circle.fillAmount >= 1 && circle.fillClockwise)
                circle.fillClockwise = false;
            else if (!circle.fillClockwise && circle.fillAmount > 0)
                circle.fillAmount -= 0.05f;
            else
                circle.fillClockwise = true;
        }
	}
}
