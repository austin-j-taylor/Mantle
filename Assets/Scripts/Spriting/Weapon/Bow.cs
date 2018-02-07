using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Shooter {

    private float drawingTime;

    private void Start() {
        shooterAnimator = GetComponent<Animator>();
        velocity = 20;
        loadTime = 1f;
        launchPosition = new Vector3(0f, 0f, .5f);
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

}
