using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargStationaryController : Enemy {

    private Animator anim;

    new void Start() {
        anim = GetComponentInChildren<Animator>();
        health.MaxHealth = 5;
        health.Health = health.MaxHealth;
    }

    void Update() {
        if (!isDead) {
            if (health.Health <= 0) {
                // die

            } else if (GetComponent<Rigidbody>().velocity.magnitude > 1f) {
                anim.SetBool("IsMoving", true);
            }
        }
    }

    public override void OnHit(float damage) {
        base.OnHit(damage);

    }
}
