using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Shooter {

    private BowstringController bowstringController;

    private float drawingTime;

    public Transform rightArm;
    public Transform rightHand;
    public Transform nockHand;
    public Transform nockRiser;
    public Transform projectileAnchor;

    private Quaternion rotationLast;
    private Quaternion offsetLastArm;
    private Quaternion offsetLastHand;

    private void Start() {
        bowstringController = GetComponentInChildren<BowstringController>();
        shooterAnimator = GetComponent<Animator>();
        velocity = 20;
        loadTime = 1f;
        SetVelocityShortcuts();
    }

    protected override IEnumerator Loading() {
        IsLoading = true;
        shooterAnimator.SetBool("IsLoading", true);

        drawingTime = Time.time;
        yield return new WaitForSeconds(loadTime);

        shooterAnimator.SetBool("IsLoading", false);
        shooterAnimator.SetBool("Loaded", true);
        IsLoading = false;
        Loaded = true;
    }

    public float DrawingTime {
        get { return Time.time - drawingTime; }
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
    //public Vector3 riser;
    //public Vector3 riserp;
    //public Vector3 handp;
    //public Vector3 bowp;

    public void AngleBowAim(float bowAngle) {

        bowstringController.AttachStringToHand();

        // some blessed trig
        float handPositionDifference = rightHand.position.x - projectileAnchor.position.x;
        float rightShoulderToAnchor = rightArm.position.x - projectileAnchor.position.x;
        float adjacent = handPositionDifference - rightShoulderToAnchor;
        float opposite = handPositionDifference * Mathf.Tan(bowAngle * Mathf.PI / 180);
        float shoulderAngle = Mathf.Atan2(opposite, adjacent) * 180 / Mathf.PI;

        float armAngle = 1.36f * shoulderAngle;
        float handAngle = .68f * -shoulderAngle;

        Quaternion offsetNowArm = Quaternion.Lerp(offsetLastArm, Quaternion.Euler(0, 0, armAngle), 1);//5 * Time.deltaTime);
        offsetLastArm = offsetNowArm;
        rightArm.localRotation = rightArm.localRotation * offsetNowArm;

        // arm has been rotated, but right hand must be rotated to align with left arm (with angle)
        Quaternion offsetNowHand = Quaternion.Lerp(offsetLastHand, Quaternion.Euler(0, 0, handAngle), 1);//5 * Time.deltaTime);
        offsetLastHand = offsetNowHand;
        rightHand.localRotation = rightHand.localRotation * offsetNowHand;

        // rotate arrow to align with bow
        //y = bow.nockRiser.position.y - bow.nockHand.position.y;
        //x = bow.nockRiser.position.x - bow.nockHand.position.x;
        //ybow = bow.transform.position.y - bow.nockHand.position.y;
        //xbow = bow.transform.position.x - bow.nockHand.position.x;
        //xRis = bow.transform.InverseTransformPoint(bow.nockRiser.position).z;
        //yRis = bow.transform.InverseTransformPoint(bow.nockRiser.position).y;
        //xHand = bow.transform.InverseTransformPoint(bow.nockHand.position).z;
        //yHand = bow.transform.InverseTransformPoint(bow.nockHand.position).y;
        x = nockHand.localPosition.x + nockHand.parent.localPosition.x + nockRiser.localPosition.x;
        y = nockHand.localPosition.y + nockHand.parent.localPosition.y + nockRiser.localPosition.y;
        //handp = nockHand.parent.localPosition;
        //riserp = nockRiser.localPosition;

        ////float y = yRis - yHand;
        ////float x = xRis - xHand;
        //riser = bow.transform.InverseTransformPoint(bow.nockRiser.position);
        //hand = bow.transform.InverseTransformPoint(bow.nockHand.position);
        //riserp = bow.nockRiser.position;
        //handp = bow.nockHand.position;
        //bowp = bow.transform.position;

        float angleToRiserNock = (Mathf.Atan2(y, x)) * 180 / Mathf.PI;
        //angleToBow = Mathf.Atan2(ybow, xbow) * 180 / Mathf.PI;
        //bow.nockHand.localEulerAngles = new Vector3(0, 0, -(angleToRiserNock - angleToBow));
        nockHand.localEulerAngles = new Vector3(0, 0, -(angleToRiserNock - angleToBow));

        bowAngel = bowAngle;
        this.shoulderAngle = shoulderAngle;
        angleRiser = angleToRiserNock;

    }
}
