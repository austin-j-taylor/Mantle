using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targ : Enemy {

    protected Animator anim;
    private Rigidbody rb;

    // Use this for initialization
    new protected void Start () {
        base.Start();

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    protected void Update() {
        if (!isDead) {
            if (rb.velocity.magnitude > .1f) {
                anim.SetBool("IsMoving", true);
            } else {
                anim.SetBool("IsMoving", false);
            }
            if (health.Health <= 0) {
                // die
                isDead = true;
                anim.SetTrigger("Killed");
            }
        }
    }

    public override void OnHit(float damage) {
        base.OnHit(damage);
    }
}
