using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour {
    public delegate void Func(RaycastHit hit, bool force = false);
    public bool debugWindow = true;
    public enum Steps
    {
        Start = 0,
        Started = 1,
        End = 2
    }
    public static int step = 0;
    public GameObject[] activationSteps;
    private Func[] stepsAction;

    IEnumerator WaitToDestroyingItem(BreakableObject objectBreak)
    {
        while (!objectBreak.isDestroy)
            yield return null;
        //activationSteps[step].SetActive(false);
        step++;
        activationSteps[step].SetActive(true);
    }

    void StartInside(RaycastHit hit, bool force = false)
    {
        if (force || hit.collider.tag == ((Steps)step).ToString())
        {
            BreakableObject objectBreakable = activationSteps[step].GetComponentInChildren<BreakableObject>();
            objectBreakable.ClickToDestroy();
            StartCoroutine(WaitToDestroyingItem(objectBreakable));
        }
    }

    void TakeInHand(RaycastHit hit, bool force = false)
    {
        if (force || hit.collider.tag == ((Steps)step).ToString())
        {
            activationSteps[step].GetComponentInChildren<WindowOnVideo>().gameObject.transform.SetParent(Camera.main.transform);
            step++;
            activationSteps[step].SetActive(true);
        }
    }

    void End(RaycastHit hit, bool force = false)
    {
        if (force || (hit.collider.tag == ((Steps)step).ToString() && hit.collider.GetComponentInChildren<SwitchMaterials>().isBreakable))
        {
            activationSteps[step].GetComponentInChildren<BreakableObject>().ClickToDestroy();
            //activationSteps[step].SetActive(false);
            Camera.main.GetComponentInChildren<WindowOnVideo>().gameObject.SetActive(false);
            step++;
        }
    }

    // Use this for initialization
    void Start () {
        Camera.main.fieldOfView = 60;
        stepsAction = new Func[3];
        stepsAction[0] = StartInside;
        stepsAction[1] = TakeInHand;
        stepsAction[2] = End;
        if (debugWindow)
        {
            stepsAction[step](new RaycastHit(), true);
            step = 1;
            //stepsAction[step](new RaycastHit(), true);
            return;
        }
        activationSteps[step].SetActive(true);
        for (int i = step + 1; i < activationSteps.Length; i++)
            activationSteps[i].SetActive(false);
    }

    // Update is called once per frame
    void Update () {
#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                stepsAction[step](hit);
        }
#endif
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
            stepsAction[step](new RaycastHit(), true);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                stepsAction[step](hit);
        }
#endif
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
