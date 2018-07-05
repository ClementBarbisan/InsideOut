using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class Scenario : MonoBehaviour {

    public enum Steps
    {
        Start = 0,
        Started = 1,
        End = 2
    }

    public delegate void Func(RaycastHit hit, bool force = false);
    public static Scenario Instance;

    //Use tracking Object or not
    public bool useFingers = false;
    //Step of scenario
    public int step = 0;
    //Debug windowOnVideo
    public bool debugWindow = false;
    //GameObject to activate at each step
    public GameObject[] activationSteps;
    //Functions calls at each step
    public Func[] stepsAction;
    //Fingers specific behaviour
    private ObjectTargetBehaviour objectTarget;

    //Wait for the vase to be destroyed before activate next step
    IEnumerator WaitToDestroyingItem(BreakableObject objectBreak)
    {
        while (!objectBreak.isDestroy)
            yield return null;
        step++;
        activationSteps[step].SetActive(true);
        if (useFingers)
            activationSteps[step].GetComponentInChildren<WindowOnVideo>().gameObject.transform.SetParent(objectTarget.gameObject.transform);
    }

    void StartInside(RaycastHit hit, bool force = false)
    {
        if (force || hit.collider.tag == ((Steps)step).ToString())
        {
            if (!useFingers)
            {
                BreakableObject objectBreakable = activationSteps[step].GetComponentInChildren<BreakableObject>();
                objectBreakable.ClickToDestroy();
                StartCoroutine(WaitToDestroyingItem(objectBreakable));
            }
            else
            {
                step++;
                activationSteps[step].SetActive(true);
                if (useFingers)
                {
                    //Deactivate boxCollider
                    objectTarget.transform.GetChild(0).gameObject.SetActive(false);
                    WindowOnVideo window = activationSteps[step].GetComponentInChildren<WindowOnVideo>();
                    window.transform.position = new Vector3(-0.75f, 0.5f, 0f);
                    //Set window on video to be tracked by ObjectTrackingBehaviour
                    window.gameObject.transform.SetParent(objectTarget.gameObject.transform, false);
                    window.transform.rotation = Quaternion.Euler(new Vector3(270, 0, 180));
                }
            }
        }
    }

    public void TakeInHand(RaycastHit hit, bool force = false)
    {
        if (force || hit.collider.tag == ((Steps)step).ToString())
        {
            if (!useFingers)
                activationSteps[step].GetComponentInChildren<WindowOnVideo>().gameObject.transform.SetParent(Camera.main.transform);
            step++;
            activationSteps[step].SetActive(true);
        }
    }

    public void End(RaycastHit hit, bool force = false)
    {
        if (force || (hit.collider.tag == ((Steps)step).ToString() && hit.collider.GetComponentInChildren<SwitchMaterials>().isBreakable))
        {
            if (!useFingers)
            {
                activationSteps[step].GetComponentInChildren<BreakableObject>().ClickToDestroy();
                Camera.main.GetComponentInChildren<WindowOnVideo>().gameObject.SetActive(false);
            }
            else
            {
                objectTarget.GetComponentInChildren<WindowOnVideo>().gameObject.SetActive(false);
            }
            step++;
            SceneManager.LoadSceneAsync(2);
        }
    }

    private void Awake()
    {
        Vuforia.VuforiaRuntime.Instance.InitVuforia();
        Instance = this;
        step = 0;
        objectTarget = FindObjectOfType<ObjectTargetBehaviour>();
        //Get version to use
        if (PlayerPrefs.HasKey("version"))
        {
            if (PlayerPrefs.GetString("version") == "Fingers")
                useFingers = true;
            else
            {
                useFingers = false;
            }
        }
    }

    // Use this for initialization
    void Start () {
       
        if (!useFingers)
        {
            objectTarget.gameObject.SetActive(false);
        }
        stepsAction = new Func[3];
        stepsAction[0] = StartInside;
        stepsAction[1] = TakeInHand;
        stepsAction[2] = End;
        //Test for calibrate window
        if (debugWindow)
        {
            stepsAction[step](new RaycastHit(), true);
            stepsAction[step](new RaycastHit(), true);
            return;
        }
        activationSteps[step].SetActive(true);
        for (int i = step + 1; i < activationSteps.Length; i++)
            activationSteps[i].SetActive(false);
    }

    // Update is called once per frame
    void Update () {
#if UNITY_ANDROID
        if (!useFingers)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                    stepsAction[step](hit);
            }
        }
#endif
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
            stepsAction[step](new RaycastHit(), true);
        if (!useFingers)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                    stepsAction[step](hit);
            }
        }
#endif
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
