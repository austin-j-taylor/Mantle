using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public Projectile projectile;
    public Transform nockHand;
    public Transform nockRiser;

    protected Animator shooterAnimator;

    protected float velocity = 15;
    protected float loadTime = 1f;
    protected bool nocked = false;
    protected bool isLoading = false;
    protected bool loaded = false;

    private LineRenderer predictedPath;
    private Coroutine loadingRoutine;

    // constants for physics calculations
    private float velocity2;
    private float velocity4;

    public bool Nocked {
        get { return nocked; }
        set { nocked = value; }
    }
    public bool IsLoading {
        get { return isLoading; }
        set { isLoading = value; }
    }
    public bool Loaded {
        get { return loaded; }
        set { loaded = value; }
    }
    public float LoadTime {
        get { return loadTime; }
    }

    void Start() {
        predictedPath = GetComponent<LineRenderer>();
        shooterAnimator = GetComponent<Animator>();
        SetVelocityShortcuts();
    }

    protected void SetVelocityShortcuts() {
        velocity2 = velocity * velocity;
        velocity4 = velocity2 * velocity2;
    }

    /*
     * Calculates the launch velocity of this shooter's projectile such that the projectile passes from spawnPoint to targetPoint.
     * lowShot determines if the projectile takes the low path or the high path, since there'll always be 2 from the quadratic equation.
     */
    public Vector3 CalculateLaunchVelocity(Vector3 targetPoint, Vector3 spawnPoint, bool lowShot) {
        // shotSpawn is angled such that it faces targetPoint, not where it actually shoots from. This is good enough aesthetically. for now.
        //transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
        //shotSpawn = transform.position + transform.rotation * launchPosition;
        Vector3 shotSpawn = spawnPoint;
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

        Vector3 trueVelocity = trueVelocityDirection * velocity;
        
        //RotateToLaunch(angle);

        return trueVelocity;

    }
    
    public float CalculateBowAngle(Vector3 targetPoint, Vector3 anchorPoint, bool lowShot) {
        Vector3 shotSpawn = anchorPoint;
        Vector3 vectorFromShotToTarget = targetPoint - shotSpawn;
        Vector3 horizontalComponent = new Vector3(vectorFromShotToTarget.x, 0f, vectorFromShotToTarget.z);

        float height = targetPoint.y - shotSpawn.y;
        float distance = horizontalComponent.magnitude;

        // Calculus
        float gravity = -Physics.gravity.y; // needs to be positive
        float underRoot = velocity4 - gravity * (gravity * distance * distance + 2 * velocity2 * height);
        underRoot = underRoot < 0 ? 0 : underRoot;

        float radiansUp = Mathf.Atan(
                (velocity2 + (lowShot ? -1 : 1) * Mathf.Sqrt(underRoot))
              / (gravity * distance));

        return radiansUp * 180 / Mathf.PI;
    }
    
    public Projectile SpawnProjectile(Vector3 spawnPosition, Quaternion spawnRotation) {
        Projectile newProjectile = Instantiate(projectile, spawnPosition, spawnRotation);
        newProjectile.GetComponent<Collider>().enabled = false;
        newProjectile.GetComponent<Rigidbody>().isKinematic = true;
        newProjectile.InHand = true;
        return newProjectile;
    }

    public void Fire(Vector3 launchVelocity, Projectile shot) {

        shot.transform.SetParent(null, true);
        shot.GetComponent<Collider>().enabled = true;
        shot.GetComponent<Rigidbody>().isKinematic = false;

        shooterAnimator.SetTrigger("Fire");
        shooterAnimator.SetBool("Loaded", false);
        nocked = false;
        loaded = false;
        shot.InHand = false;
        
        shot.GetComponent<Rigidbody>().velocity = launchVelocity;
    }

    public void Load() {
        shooterAnimator.ResetTrigger("Fire");
        loadingRoutine = StartCoroutine(Loading());
    }

    protected virtual IEnumerator Loading() {
        isLoading = true;
        shooterAnimator.SetBool("IsLoading", true);

        yield return new WaitForSeconds(loadTime);

        shooterAnimator.SetBool("IsLoading", false);
        shooterAnimator.SetBool("Loaded", true);
        isLoading = false;
        loaded = true;
    }

    public virtual void CeaseLoad() {
        isLoading = false;
        shooterAnimator.SetBool("IsLoading", false);
        StopCoroutine(loadingRoutine);
    }

    public void Undraw() {
        loaded = false;
        shooterAnimator.SetBool("Loaded", false);
        shooterAnimator.SetBool("IsLoading", false);
        shooterAnimator.SetTrigger("Undraw");
    }

    public void RotateToLaunch(float angle) {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-angle, 90, 0), 10 * Time.deltaTime);
    }

    /*
     * Visually predicts the path the projectile would take if it launched with the current velocity
     */
    //public void PredictPath(Vector3 launchVelocity, Vector3 targetPos) {

    //    int segmentCount = 40;
    //    float segmentScale = .5f;

    //    predictedPath.enabled = true;

    //    Vector3[] segments = new Vector3[segmentCount];
    //    segments[0] = shotSpawn;
    //    Vector3 segVelocity = launchVelocity;

    //    for (int i = 1; i < segmentCount; i++) {
    //        float segTime = segmentScale / segVelocity.magnitude;

    //        segVelocity = segVelocity + Physics.gravity * segTime;

    //        if (false) {

    //        } else {
    //            segments[i] = segments[i - 1] + segVelocity * segTime;
    //        }
    //    }

    //    //predictedPath.numPositions(segmentCount);
    //    predictedPath.SetPositions(segments);
    //    //for(int i = 0; i < segmentCount; i++) {
    //    //    predictedPath.SetPosition(i, segments[i]);
    //    //}

    //}
}
