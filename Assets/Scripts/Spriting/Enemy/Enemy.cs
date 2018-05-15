using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //protected float hitstun;
    //private float lastHitTime;
    protected bool isDead;
    protected EnemyHealth health;
    protected BoxCollider[] hitboxes;
    private bool hitThisFrame;

    virtual protected void Start() {
        health = GetComponent<EnemyHealth>();
        hitboxes = GetComponentsInChildren<BoxCollider>();
        isDead = false;
        hitThisFrame = false;
        //hitstun = 0f;
        //lastHitTime = 0;
    }
    protected void LateUpdate() {
        hitThisFrame = false;
    }

    virtual public void OnHit(float damage) {
        //if (lastHitTime + hitstun < Time.time) {
        if (!hitThisFrame) {
            health.Health -= damage;
            hitThisFrame = true;
        }
        //    lastHitTime = Time.time;
        //}
    }
}
