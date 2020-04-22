using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

//Custom class of ImageTargetBehaviour to activate random model in a list
public class ImageTargetBehaviourCustom : ImageTargetBehaviour {
    public GameObject[] models;
    private AttackingScript attackScript;
    private MovingScript moveScript;
    private SkinnedMeshRenderer[] renders;
    private Collider[] colliders;
    private int currentIndex = -1;
	// Use this for initialization
	void Start () {
        attackScript = GetComponent<AttackingScript>();
        moveScript = GetComponent<MovingScript>();
        renders = new SkinnedMeshRenderer[models.Length];
        colliders = new Collider[models.Length];
        for (int i = 0; i < models.Length; i++)
        {
            renders[i] = models[i].GetComponentInChildren<SkinnedMeshRenderer>();
            colliders[i] = models[i].GetComponentInChildren<Collider>();
        }
	}

    public override void OnTrackerUpdate(Status newStatus, StatusInfo infos)
    {
        base.OnTrackerUpdate(newStatus, infos);
        
        if (infos == StatusInfo.NORMAL && currentIndex == -1)
        {
            currentIndex = Random.Range(0, models.Length);
            for (int i = 0; i < models.Length; i++)
            {
                if (i == currentIndex)
                {
                    renders[currentIndex].enabled = true;
                    colliders[currentIndex].enabled = true;
                    attackScript.currentEnemy = models[currentIndex];
                    moveScript.currentEnemy = models[currentIndex];
                }
                else
                {
                    renders[i].enabled = false;
                    colliders[i].enabled = false;
                }
            }
        }
        else if ((infos != StatusInfo.NORMAL) && currentIndex > -1)
        {
            for (int i = 0; i < models.Length; i++)
            {
                renders[i].enabled = false;
                colliders[i].enabled = false;
                attackScript.currentEnemy = null;
                if (moveScript.currentEnemy != null)
                    moveScript.currentEnemy.GetComponent<Animator>().SetBool("Move", false);
                moveScript.currentEnemy = null;
            }
            currentIndex = -1;
        }
    }

    // Update is called once per frame
    void Update () {
        //Fix for extended tracking to not have more than one model at the same time
        if (currentIndex > -1)
        {
            for (int i = 0; i < models.Length; i++)
            {
                if (i == currentIndex)
                {
                    renders[currentIndex].enabled = true;
                    colliders[currentIndex].enabled = true;
                    attackScript.currentEnemy = models[currentIndex];
                    moveScript.currentEnemy = models[currentIndex];
                }
                else
                {
                    renders[i].enabled = false;
                    colliders[i].enabled = false;
                }
            }
        }
	}
}
