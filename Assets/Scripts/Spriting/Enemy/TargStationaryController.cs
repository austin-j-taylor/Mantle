using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargStationaryController : MonoBehaviour {

    private Animator anim;
    
	void Start () {
        anim = GetComponentInChildren<Animator>();
	}
	
	void Update () {
        if(GetComponent<Rigidbody>().velocity.magnitude > 1f) {
            anim.SetBool("IsMoving", true); 
        }
	}
}
