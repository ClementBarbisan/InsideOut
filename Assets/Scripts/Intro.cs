using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

    public void StartApp(string name)
    {
        PlayerPrefs.SetString("version", name);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
