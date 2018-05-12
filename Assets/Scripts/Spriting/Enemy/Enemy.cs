using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    protected bool isDead;
    protected EnemyHealth health;
    protected BoxCollider[] hitboxes;

	protected void Start () {
        health = GetComponent<EnemyHealth>();
        hitboxes = GetComponentsInChildren<BoxCollider>();
        isDead = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    virtual public void OnHit(float damage) {
        health.Health -= damage;
    }
}
