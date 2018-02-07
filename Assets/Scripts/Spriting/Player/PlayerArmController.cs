using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour {

    public GameObject leftArm;
    public GameObject rightArm;

    private PlayerWeaponController player;
    private PlayerBowController bowController;
    private SpriteZLevelRendering zRenderer;

    // Default layer values
    private const int frontRight = 61;
    private const int frontLeft = 60;
    private const int behindRight = 41;
    private const int behindLeft = 40;

    private const int left = 1;
    private const int right = 2;

    void Start() {
        bowController = GetComponent<PlayerBowController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeaponController>();
        zRenderer = GetComponent<SpriteZLevelRendering>();
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
        zRenderer.relativeSpriteLayers[left] = behindLeft;
    }

    private void SetRightHandBehind() {
        //bowController.SetBowBehind();
        zRenderer.relativeSpriteLayers[right] = behindRight;
    }

    private void SetLeftHandFront() {
        zRenderer.relativeSpriteLayers[left] = frontLeft;
    }

    private void SetRightHandFront() {
        //bowController.SetBowFront();
        zRenderer.relativeSpriteLayers[right] = frontRight;
    }

}
