using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour {

    public GameObject leftArm;
    public GameObject rightArm;

    private SpriteZLevelRendering zRenderer;

    // Default layer values
    private const int frontRight = 31;
    private const int frontLeft = 30;
    private const int behindRight = 11;
    private const int behindLeft = 10;

    private const int left = 1;
    private const int right = 2;

    void Start() {
        zRenderer = gameObject.GetComponent<SpriteZLevelRendering>();
    }

    public void WalkingUpFromLeftOn() {
        SetLeftHandBehind();
        SetRightHandFront();
    }

    public void WalkingUpFromLeftOff() {
        SetLeftHandFront();
        SetRightHandBehind();
    }

    public void WalkingUpFromRightOn() {
        SetLeftHandFront();
        SetRightHandBehind();
    }

    public void WalkingUpFromRightOff() {
        SetLeftHandBehind();
        SetRightHandFront();
    }

    private void SetLeftHandBehind() {
        zRenderer.relativeLayers[left] = behindLeft;
    }

    private void SetRightHandBehind() {
        zRenderer.relativeLayers[right] = behindRight;
    }

    private void SetLeftHandFront() {
        zRenderer.relativeLayers[left] = frontLeft;
    }

    private void SetRightHandFront() {
        zRenderer.relativeLayers[right] = frontRight;
    }

}
