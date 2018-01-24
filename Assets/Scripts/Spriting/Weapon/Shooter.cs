using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public Projectile projectile;

    private float velocity = 15;

    private Vector3 launchPosition; // position on the weapon at which the shot appears

    // constants for physics calculations
    private float velocity2;
    private float velocity4;
    
    void Start() {
        launchPosition = new Vector3(0f, 0f, .5f);
        velocity2 = velocity * velocity;
        velocity4 = velocity2 * velocity2;
    }
    
    /*
     * Fires this Shooter's Projectile from launchPosition such that it will pass through targetPoint
     * 
     */
    public void Fire(Vector3 targetPoint, bool lowShot) {

        // shotSpawn is angled such that it faces targetPoint, not where it actually shoots from. This is good enough aesthetically.
        Vector3 shotSpawn = transform.position + Quaternion.LookRotation(targetPoint - transform.position) * launchPosition;
        Vector3 vectorFromShotToTarget = targetPoint - shotSpawn;

        Vector3 horizontalComponent = new Vector3(vectorFromShotToTarget.x, 0f, vectorFromShotToTarget.z);
        
        float height = targetPoint.y - shotSpawn.y;
        float distance = horizontalComponent.magnitude;
        Vector3 horizontalComponentDirection = horizontalComponent / distance;

        // Calculus
        float gravity = -Physics.gravity.y; // needs to be positive
        float underRoot = velocity4 - gravity * (gravity * distance * distance + 2 * velocity2 * height);
        underRoot = underRoot < 0 ? 0 : underRoot;

        float radiansUp = Mathf.Atan(
                (velocity2 + (lowShot ? -1 : 1) * Mathf.Sqrt(underRoot))
              / (gravity * distance));
        
        Vector3 trueVelocityDirection = new Vector3(horizontalComponentDirection.x * Mathf.Cos(radiansUp), Mathf.Sin(radiansUp), horizontalComponentDirection.z * Mathf.Cos(radiansUp));
        
        Projectile shot = Instantiate(projectile, shotSpawn, Quaternion.LookRotation(trueVelocityDirection));
        
        shot.GetComponent<Rigidbody>().velocity = trueVelocityDirection * velocity;

    }
}
