using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public bool inHand = true;
    public bool InHand {
        get { return inHand; }
        set { inHand = value; }
    }

    private Transform hit;
    private bool stuck = false;
    private bool stuckLastFrame = true;

    private Quaternion lastRotation;
    private Vector3 lastPosition;
    private Vector3 lastSpeed;
    
    void LateUpdate() {
        if (stuck) {
            if(stuckLastFrame) {
                transform.rotation = lastRotation;
                transform.position = lastPosition + lastSpeed * Time.deltaTime;

                stuckLastFrame = false;
            }

        } else if(!inHand) {
            // make rotation equal velocity
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
            lastRotation = transform.rotation;
            lastPosition = transform.position;
            lastSpeed = GetComponent<Rigidbody>().velocity;
        }

    }

    private void OnCollisionEnter(Collision collision) {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().mass = 0;
        stuck = true;
        hit = collision.collider.transform;

        GameObject scaleUnMesserUpper = new GameObject();

        scaleUnMesserUpper.transform.parent = hit;
        scaleUnMesserUpper.transform.localScale = new Vector3(1f / hit.localScale.x, 1f / hit.localScale.y, 1f / hit.localScale.z);
        scaleUnMesserUpper.transform.rotation = new Quaternion();

        transform.parent = scaleUnMesserUpper.transform;

    }
}
