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

    // Rotates the arm to aim with the given angle
    public void AngleBowAim(float bowAngle) {

        bowstringController.AttachStringToHand();

        // some blessed trig
        float handPositionDifference = rightHand.position.x - projectileAnchor.position.x;
        float shoulderAngle = Mathf.Atan2(
                handPositionDifference * Mathf.Tan(bowAngle * Mathf.PI / 180),
                handPositionDifference - rightArm.position.x + projectileAnchor.position.x) * 180 / Mathf.PI;

        Quaternion offsetNowArm = Quaternion.Lerp(offsetLastArm, Quaternion.Euler(0, 0, 1.25f * shoulderAngle), 5 * Time.deltaTime);
        offsetLastArm = offsetNowArm;
        rightArm.localRotation = rightArm.localRotation * offsetNowArm;

        // arm has been rotated, but right hand must be rotated to align with left arm (with angle)
        Quaternion offsetNowHand = Quaternion.Lerp(offsetLastHand, Quaternion.Euler(0, 0, .625f * -shoulderAngle), 5 * Time.deltaTime);
        offsetLastHand = offsetNowHand;
        rightHand.localRotation = rightHand.localRotation * offsetNowHand;
        
        bowstringController.AttachStringToHand();
        float angleToRiserNock = (Mathf.Atan2(
                -nockHand.localPosition.y - nockHand.parent.localPosition.y + nockRiser.localPosition.y,
                nockHand.localPosition.x + nockHand.parent.localPosition.x - nockRiser.localPosition.x)) * 180 / Mathf.PI;
        nockHand.localEulerAngles = new Vector3(0, 0, -(angleToRiserNock));

    }
}
