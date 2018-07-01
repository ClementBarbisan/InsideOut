using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingScript : MonoBehaviour {
    public GameObject currentEnemy = null;
    public bool isMoving = false;
    public float speed = 5f;
    public float speedRotation = 10f;
    private Quaternion attendedRotation = Quaternion.identity;
    private AttackingScript attackingScript;
    public GameObject target;
    private float angle = 0;
    // Use this for initialization
	void Start () {
        attackingScript = GetComponent<AttackingScript>();
        target = new GameObject();
        target.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update () {
        angle += speedRotation * Time.deltaTime;
        if (!attackingScript.isAttacking && currentEnemy != null)
        {
            if (!isMoving)
            {
                currentEnemy.GetComponent<Animator>().SetBool("Move", true);
                isMoving = true;
            }
            float step = speed * Time.deltaTime;
            target.transform.localPosition = new Vector3(0.5f * Mathf.Cos(Mathf.Deg2Rad * angle), 0, 0.5f * Mathf.Sin(Mathf.Deg2Rad * angle));
            Vector3 nextStep = (Vector3.Scale(new Vector3(1, 0, 1), target.transform.localPosition) - Vector3.Scale(new Vector3(1, 0, 1), currentEnemy.transform.localPosition)) * step;
            attendedRotation = Quaternion.LookRotation(currentEnemy.transform.localPosition - nextStep, currentEnemy.transform.up) * Quaternion.Euler(0, 270, 0);
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
