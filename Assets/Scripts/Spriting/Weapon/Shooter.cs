using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public Projectile projectile;

    private float velocity = 15;
    private Vector3 shotSpawn;
    private Vector3 launchPosition; // position on the weapon at which the shot appears
    private LineRenderer predictedPath;

    // constants for physics calculations
    private float velocity2;
    private float velocity4;

    void Start() {
        predictedPath = GetComponent<LineRenderer>();
        launchPosition = new Vector3(0f, 0f, .5f);
        velocity2 = velocity * velocity;
        velocity4 = velocity2 * velocity2;
    }

    /*
     * Fires this Shooter's Projectile from launchPosition such that it will pass through targetPoint
     * 
     */
    public Vector3 CalculateLaunchVelocity(Vector3 targetPoint, bool lowShot) {

        // shotSpawn is angled such that it faces targetPoint, not where it actually shoots from. This is good enough aesthetically.
        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
        shotSpawn = transform.position + transform.rotation * launchPosition;
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

        //Projectile shot = Instantiate(projectile, shotSpawn, Quaternion.LookRotation(trueVelocityDirection));

        Vector3 trueVelocity = trueVelocityDirection * velocity;

        //shot.GetComponent<Rigidbody>().velocity = trueVelocity;

        return trueVelocity;

    }

    public void Fire(Vector3 launchVelocity) {

        Projectile shot = Instantiate(projectile, shotSpawn, Quaternion.LookRotation(launchVelocity));

        shot.GetComponent<Rigidbody>().velocity = launchVelocity;
    }

    /*
     * Visually predicts the path the projectile would take if it launched with the current velocity
     */
    //public void PredictPath(Vector3 targetPos) {
    //    int firstIndex = 0;
    //    int lastIndex = 1;

    //    predictedPath.enabled = true;

    //    predictedPath.SetPosition(firstIndex, shotSpawn);



    //    predictedPath.SetPosition(lastIndex, targetPos);

    //}
    int segmentCount = 40;
    float segmentScale = .5f;

    public void PredictPath(Vector3 launchVelocity, Vector3 targetPos) {

        predictedPath.enabled = true;

        Vector3[] segments = new Vector3[segmentCount];
        segments[0] = shotSpawn;
        Vector3 segVelocity = launchVelocity;

        for (int i = 1; i < segmentCount; i++) {
            float segTime = segmentScale / segVelocity.magnitude;

            segVelocity = segVelocity + Physics.gravity * segTime;

            if (false) {

            } else {
                segments[i] = segments[i - 1] + segVelocity * segTime;
            }
        }

        //predictedPath.numPositions(segmentCount);
        predictedPath.SetPositions(segments);
        //for(int i = 0; i < segmentCount; i++) {
        //    predictedPath.SetPosition(i, segments[i]);
        //}

    }
}
