using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargMobileController : Enemy {

    private Animator anim;
    private Transform player;
    private NavMeshAgent mesh;
    private Rigidbody rb;

    new void Start() {
        base.Start();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mesh = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        health.MaxHealth = 10;
        health.Health = health.MaxHealth;
    }
    void Update() {
        if (!isDead) {
            if (health.Health <= 0) {
                // die
                mesh.enabled = false;
                isDead = true;
                anim.SetTrigger("Killed");
            } else {
                if (mesh.enabled) {
                    Debug.Log("setting dest");
                    mesh.SetDestination(player.position);
                    if (GetComponent<Rigidbody>().velocity.magnitude > 1f) {
                        anim.SetBool("IsMoving", true);
                    } else {
                        anim.SetBool("IsMoving", false);
                    }
                }
            }
        }
    }

    public override void OnHit(float damage) {
        base.OnHit(damage);
        // temporarily disable navmesh
        //StartCoroutine(DisableNavAgent(1));

    }

    private IEnumerator DisableNavAgent(float time) {
        mesh.enabled = false;
        yield return new WaitForSeconds(time);
        if (!isDead)
            mesh.enabled = true;
    }
}
