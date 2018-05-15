using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargMobileController : Targ {
    
    private Transform player;
    private NavMeshAgent mesh;

    new void Start() {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        mesh = GetComponent<NavMeshAgent>();
        health.MaxHealth = 15;
    }
    new void Update() {
        if (!isDead) {
            if (mesh.velocity.magnitude > .1f) {
                anim.SetBool("IsMoving", true);
            } else {
                anim.SetBool("IsMoving", false);
            }
            if (health.Health <= 0) {
                // die
                mesh.isStopped = true;
                isDead = true;
                anim.SetTrigger("Killed");
            } else {
                if (mesh.enabled) {
                    mesh.SetDestination(player.position);
                }
            }
        }
    }
}
