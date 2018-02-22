using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

    public Bow bow;
    public GameObject quiver;
    public Transform rightArm;
    public Transform rightHand;
    public Transform leftHand;
    public Transform projectileAnchor;

    private Projectile currentlyHeldProjectile;
    public Projectile HeldProjectile {
        get { return currentlyHeldProjectile; }
    }

    private Animator anim;
    private LayerMask ignorePlayerLayer;

    private Quaternion rotationLast;
    private Quaternion offsetLastArm;
    private Quaternion offsetLastHand;

    private const float maxRange = 100f;
    private const float armAngle = 63.483f;

    // for launching one projectile every frame
    private bool funMode = false;

    void Start() {
        ignorePlayerLayer = ~(1 << LayerMask.NameToLayer("Player"));
        anim = GetComponentInChildren<Animator>();
        currentlyHeldProjectile = null;
    }

    void LateUpdate() {

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // goes from CAMERA towards SCREEN which likely WILL INTERSECT WITH THE PHYSICAL REALM
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, maxRange, ignorePlayerLayer)) { // target point selected, should always be true

            // if using bow
            BowUpdate(hit);

            // if using crossbow:

        }
    }

    private void BowUpdate(RaycastHit hit) {
        //Vector3 launchVelocity = bow.CalculateLaunchVelocity(hit.point, (currentlyHeldProjectile == null) ? bow.transform.position : currentlyHeldProjectile.transform.position, true || !Input.GetButton("Fire3"), out angle);

        float launchAngle = bow.CalculateBowAngle(hit.point, projectileAnchor.position, true || !Input.GetButton("Fire3"));

        if (Input.GetKeyDown(KeyCode.F)) {
            funMode = !funMode;
        }
        if (funMode && Input.GetButton("Fire1")) {
            // fun mode
            PullArrowFromQuiver();
            Vector3 launchVelocity = bow.CalculateLaunchVelocity(hit.point, currentlyHeldProjectile.transform.position, true || !Input.GetButton("Fire3"));
            bow.Fire(launchVelocity, currentlyHeldProjectile);
            ReleaseArrowFromString();
        }

        if (bow.Loaded) { // is already loaded
            AngleBowAim(launchAngle);
            // if right click, lower bow, keeping arrow nocked
            if (Input.GetButtonDown("Fire2")) {
                anim.SetBool("IsDrawing", false);
                anim.SetTrigger("Unnock");
                bow.Undraw();
            } else

            // if not holding left click, fire
            if (!Input.GetButton("Fire1")) {
                Vector3 launchVelocity = bow.CalculateLaunchVelocity(hit.point, currentlyHeldProjectile.transform.position, true || !Input.GetButton("Fire3"));
                bow.Fire(launchVelocity, currentlyHeldProjectile);
                ReleaseArrowFromString();
                anim.SetBool("IsDrawing", false);
                anim.SetTrigger("Fire");
            }
        } else
        if (bow.IsLoading) { // is currently loading
            AngleBowAim(launchAngle);
            // if right click, carefully release draw, keeping arrow nocked
            if (Input.GetButtonDown("Fire2")) {
                anim.SetBool("IsDrawing", false);
                anim.SetTrigger("Unnock");
                bow.CeaseLoad();
                bow.Undraw();
            } else
            // if release click, fire before full draw for less launch velocty
            if (!Input.GetButton("Fire1")) {
                bow.CeaseLoad();
                // make me littler launch velocity
                Vector3 launchVelocity = bow.CalculateLaunchVelocity(hit.point, currentlyHeldProjectile.transform.position, true || !Input.GetButton("Fire3"));
                bow.Fire((bow.DrawingTime / bow.LoadTime) * launchVelocity, currentlyHeldProjectile);
                ReleaseArrowFromString();
                anim.SetBool("IsDrawing", false);
                anim.SetTrigger("Fire");
            }
        } else { // not loaded, is not alreading loading
                 // if nocked and left click, begin loading bow
            if (bow.Nocked) {
                if (Input.GetButtonDown("Fire2")) {
                    // replace arrow into quiver
                    anim.SetTrigger("Unnock");
                    SetArrowNotNockedOnBowstring();
                } else
                if (Input.GetButton("Fire1")) {
                    anim.SetBool("IsDrawing", true);
                    bow.Load();
                }
            } else { // not nocked
                if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1")) {
                    // draw arrow from quiver and nock arrow
                    anim.SetTrigger("Nock");
                }
            }
        }
    }

    public void ReleaseArrowFromString() {
        currentlyHeldProjectile = null;

    }

    public void PullArrowFromQuiver() {
        currentlyHeldProjectile = bow.SpawnProjectile(quiver.transform.position + new Vector3(.07f, 0, 0), quiver.transform.localRotation * Quaternion.Euler(180, 90, 0));
        currentlyHeldProjectile.transform.SetParent(projectileAnchor, true);
    }

    public void ReplaceArrowIntoQuiver() {
        Destroy(currentlyHeldProjectile.gameObject);
    }

    public void SetArrowNockedOnBowstring() {
        bow.Nocked = true;
        currentlyHeldProjectile.transform.SetParent(bow.nockHand);
        currentlyHeldProjectile.SetTailPositionNocked();
    }

    private void SetArrowNotNockedOnBowstring() {
        bow.Nocked = false;
        currentlyHeldProjectile.transform.SetParent(projectileAnchor, true);
        //currentlyHeldProjectile.SetTailPositionNotNocked();
    }
    public float bowAngel;
    public float shoulderAngle;
    public float angleRiser;
    public float angleToBow;
    public float x;
    public float y;
    public float xbow;
    public float ybow;
    //public float xRis;
    //public float yRis;
    //public float xHand;
    //public float yHand;
    public Vector3 riser;
    public Vector3 nockPos;
    public Vector3 riserp;
    public Vector3 handp;
    public Vector3 bowp;
    public Transform handNock;

    private void AngleBowAim(float bowAngle) {
        // some blessed trig
        //float handPositionDifference = rightHand.position.x - projectileAnchor.position.x;
        //float rightShoulderToAnchor = rightArm.position.x - projectileAnchor.position.x;
        //float adjacent = handPositionDifference - rightShoulderToAnchor;
        //float opposite = handPositionDifference * Mathf.Tan(bowAngle * Mathf.PI / 180);
        //float shoulderAngle = Mathf.Atan2(opposite, adjacent) * 180 / Mathf.PI;

        //float armAngle = 1.36f * shoulderAngle;
        //float handAngle = .68f * -shoulderAngle;

        //Quaternion offsetNowArm = Quaternion.Lerp(offsetLastArm, Quaternion.Euler(0, 0, armAngle), 1);//5 * Time.deltaTime);
        //offsetLastArm = offsetNowArm;
        //rightArm.localRotation = rightArm.localRotation * offsetNowArm;

        //// arm has been rotated, but right hand must be rotated to align with left arm (with angle)
        //Quaternion offsetNowHand = Quaternion.Lerp(offsetLastHand, Quaternion.Euler(0, 0, handAngle), 1);//5 * Time.deltaTime);
        //offsetLastHand = offsetNowHand;
        //rightHand.localRotation = rightHand.localRotation * offsetNowHand;

        //// rotate arrow to align with bow
        ////y = bow.nockRiser.position.y - bow.nockHand.position.y;
        ////x = bow.nockRiser.position.x - bow.nockHand.position.x;
        ////ybow = bow.transform.position.y - bow.nockHand.position.y;
        ////xbow = bow.transform.position.x - bow.nockHand.position.x;
        ////xRis = bow.transform.InverseTransformPoint(bow.nockRiser.position).z;
        ////yRis = bow.transform.InverseTransformPoint(bow.nockRiser.position).y;
        ////xHand = bow.transform.InverseTransformPoint(bow.nockHand.position).z;
        ////yHand = bow.transform.InverseTransformPoint(bow.nockHand.position).y;
        //x = bow.nockHand.localPosition.x + bow.nockHand.parent.localPosition.x + bow.nockRiser.localPosition.x;
        //y = bow.nockHand.localPosition.y + bow.nockHand.parent.localPosition.y + bow.nockRiser.localPosition.y;
        nockPos = handNock.localPosition;
        Debug.Log(handNock.localPosition);
        //handp = bow.nockHand.parent.localPosition;
        //riserp = bow.nockRiser.localPosition;

        //////float y = yRis - yHand;
        //////float x = xRis - xHand;
        ////riser = bow.transform.InverseTransformPoint(bow.nockRiser.position);
        ////hand = bow.transform.InverseTransformPoint(bow.nockHand.position);
        ////riserp = bow.nockRiser.position;
        ////handp = bow.nockHand.position;
        ////bowp = bow.transform.position;

        //float angleToRiserNock = (Mathf.Atan2(y, x)) * 180 / Mathf.PI;
        ////angleToBow = Mathf.Atan2(ybow, xbow) * 180 / Mathf.PI;
        ////bow.nockHand.localEulerAngles = new Vector3(0, 0, -(angleToRiserNock - angleToBow));
        //bow.nockHand.localEulerAngles = new Vector3(0, 0, -(angleToRiserNock - angleToBow));

        //bowAngel = bowAngle;
        //this.shoulderAngle = shoulderAngle;
        //angleRiser = angleToRiserNock;
    }

}
