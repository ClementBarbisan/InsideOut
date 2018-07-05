using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Move enemies in circles
public class MovingScript : MonoBehaviour {
    public GameObject currentEnemy = null;
    public bool isMoving = false;
    public float speed = 5f;
    public float speedRotation = 10f;
    private Quaternion attendedRotation = Quaternion.identity;
    private AttackingScript attackingScript;
    //Target to follow
    public GameObject target;
    private float angle = 0;
    // Use this for initialization
	void Start () {
        attackingScript = GetComponent<AttackingScript>();
        target = new GameObject();
        //Set target to be in local space
        target.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update () {
        angle += speedRotation * Time.deltaTime;
        //Check if enemy is attacking
        if (!attackingScript.isAttacking && currentEnemy != null)
        {
            //Begin move
            if (!isMoving)
            {
                currentEnemy.GetComponent<Animator>().SetBool("Move", true);
                isMoving = true;
            }
            float step = speed * Time.deltaTime;
            //Calculate the next position
            target.transform.localPosition = new Vector3(0.5f * Mathf.Cos(Mathf.Deg2Rad * angle), 0, 0.5f * Mathf.Sin(Mathf.Deg2Rad * angle));
            //Calculate next position without height axe
            Vector3 nextStep = (Vector3.Scale(new Vector3(1, 0, 1), target.transform.localPosition) - Vector3.Scale(new Vector3(1, 0, 1), currentEnemy.transform.localPosition)) * step;
            //Look at the next position(fix for android)
            attendedRotation = Quaternion.LookRotation(currentEnemy.transform.localPosition - nextStep, currentEnemy.transform.up) * Quaternion.Euler(0, 270, 0);
            //Set rotation on height axe
            currentEnemy.transform.localRotation = Quaternion.Euler(new Vector3(currentEnemy.transform.localRotation.eulerAngles.x, attendedRotation.eulerAngles.y, currentEnemy.transform.localRotation.eulerAngles.z));
            currentEnemy.transform.localPosition += nextStep;
        }
        else if (isMoving)
        {
            if (currentEnemy != null)
                currentEnemy.GetComponent<Animator>().SetBool("Move", false);
            isMoving = false;
        }
    }
}
