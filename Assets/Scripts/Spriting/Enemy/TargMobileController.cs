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
        health.MaxHealth = 15;
        health.Health = health.MaxHealth;
    }
    void Update() {
        if (!isDead) {
            speed = mesh.velocity.magnitude;
            vel = mesh.velocity;
            if (mesh.velocity.magnitude > .01f) {
                anim.SetBool("IsMoving", true);
            } else {
                anim.SetBool("IsMoving", false);
            }
            if (health.Health <= 0) {
                // die
                mesh.enabled = false;
                isDead = true;
                anim.SetTrigger("Killed");
            } else {
                if (mesh.enabled) {
                    mesh.SetDestination(player.position);
                }
            }
        }
    }
    public float speed;
    public Vector3 vel;

    public override void OnHit(float damage) {
        base.OnHit(damage);
        // temporarily disable navmesh
        //StartCoroutine(WaitForFrame());
        StartCoroutine(DisableNavAgent());

    }

    private IEnumerator DisableNavAgent() {
        mesh.enabled = false;
        yield return new WaitForSeconds(.1f);
        if (!isDead)
            mesh.enabled = true;
    }
    //private IEnumerator WaitForFrame() {
    //    mesh.enabled = false;
    //    yield return 100000;
    //    if (!isDead)
    //        mesh.enabled = true;
    //}
}
