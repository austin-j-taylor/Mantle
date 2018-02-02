using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public Projectile projectile;

    protected float velocity = 15;
    protected float loadTime = 1f;
    protected Vector3 launchPosition; // position on the weapon at which the shot appears

    private LineRenderer predictedPath;
    private Vector3 shotSpawn;
    private Coroutine loadingRoutine;
    public bool nocked = false;
    public bool isLoading = false;
    public bool loaded = false;

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
        launchPosition = new Vector3(0f, 0f, .5f);
        SetVelocityShortcuts();
    }

    protected void SetVelocityShortcuts() {
        velocity2 = velocity * velocity;
        velocity4 = velocity2 * velocity2;
    }

    /*
     * Fires this Shooter's Projectile from launchPosition such that it will pass through targetPoint
     * 
     */
    public Vector3 CalculateLaunchVelocity(Vector3 targetPoint, bool lowShot) {
        // shotSpawn is angled such that it faces targetPoint, not where it actually shoots from. This is good enough aesthetically. for now.
        //transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
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
        
        Vector3 trueVelocity = trueVelocityDirection * velocity;
        
        //transform.rotation = Quaternion.LookRotation(trueVelocity);

        return trueVelocity;

    }

    public void Fire(Vector3 launchVelocity) {
        nocked = false;
        loaded = false;

        Projectile shot = Instantiate(projectile, shotSpawn, Quaternion.LookRotation(launchVelocity));

        shot.GetComponent<Rigidbody>().velocity = launchVelocity;
    }

    public void Load() {
        loadingRoutine = StartCoroutine(Loading());
    }

    protected virtual IEnumerator Loading() {
        if (!nocked) {
            Nock();
        }
        isLoading = true;

        yield return new WaitForSeconds(loadTime);

        isLoading = false;
        loaded = true;
    }

    public virtual void CeaseLoad() {
        isLoading = false;
        StopCoroutine(loadingRoutine);
    }

    public void Nock() {
        nocked = true;
    }

    public void Unnock() {
        nocked = false;
    }

    public void Undraw() {
        loaded = false;
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

    public void PredictPath(Vector3 launchVelocity, Vector3 targetPos) {

        int segmentCount = 40;
        float segmentScale = .5f;

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
