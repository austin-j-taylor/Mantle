using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargMobileController : MonoBehaviour {

    private Animator anim;
    private Transform player;
    private UnityEngine.AI.NavMeshAgent mesh;
    
	void Start () {
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	void Update () {
        mesh.SetDestination(player.position);
        if(GetComponent<Rigidbody>().velocity.magnitude > 1f) {
            anim.SetBool("IsMoving", true); 
        } else {
            anim.SetBool("IsMoving", false);
        }

	}
}
