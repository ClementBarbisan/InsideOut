using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingScript : MonoBehaviour {
    public GameObject currentEnemy = null;
    public float distanceAttack = 2.5f;
    public float speed = 1f;
    public bool isAttacking = false;
    private Quaternion attendedRotation = Quaternion.identity;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentEnemy && Vector3.Distance(Camera.main.transform.position, currentEnemy.transform.position) < distanceAttack)
        {
            isAttacking = true;
            if (Mathf.Abs(currentEnemy.transform.rotation.eulerAngles.y - attendedRotation.eulerAngles.y) > 20f)
            {
                float step = speed * Time.deltaTime;
                attendedRotation = Quaternion.LookRotation(Camera.main.transform.position - currentEnemy.transform.parent.position);
                if (currentEnemy.transform.rotation.eulerAngles.y > attendedRotation.y)
                    currentEnemy.transform.Rotate(0, (currentEnemy.transform.rotation.eulerAngles.y - attendedRotation.y) * step, 0);
                else
                    currentEnemy.transform.Rotate(0, (attendedRotation.y - currentEnemy.transform.rotation.eulerAngles.y) * step, 0);
            }
            else
                currentEnemy.GetComponent<Animator>().SetTrigger("Attack");
        }
        else
            isAttacking = false;
	}
}
