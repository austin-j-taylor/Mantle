using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowController : MonoBehaviour {

    private PlayerWeaponController player;
    private SpriteZLevelRendering zRenderer;
    private Transform playerBody;

    // Default layer values
    private const int frontQuiver = 57;
    private const int behindQuiver = 43;
    private const int frontLimb = 70;
    private const int behindLimb = 30;
    private const int frontRiser = 71;
    private const int behindRiser = 29;
    private const int frontCam = 72;
    private const int behindCam = 28;
    private const int frontBowstring = 75;
    private const int behindBowstring = 25;
    private const int frontTopCable = 73;
    private const int behindTopCable = 28;
    private const int frontBottomCable = 74;
    private const int behindBottomCable = 27;

    // spriteChildren indices
    private const int body = 0;
    private const int riser = 3;
    private const int topLimb = 4;
    private const int topCam = 5;
    private const int bottomLimb = 6;
    private const int bottomCam = 7;
    private const int quiver = 8;
    private const int bowstring = 1;
    private const int topCable = 2;
    private const int bottomCable = 3;
    
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeaponController>();
        zRenderer = gameObject.GetComponent<SpriteZLevelRendering>();
        playerBody = zRenderer.spriteChildren[body].transform;
    }
    private void Update() {
        // send to front or back, depending on player's rotation
        float angle = playerBody.localEulerAngles.y;
        if (angle > 270 || angle < 90) {
            zRenderer.relativeSpriteLayers[quiver] = behindQuiver;
            SetBowFront();
        } else {
            zRenderer.relativeSpriteLayers[quiver] = frontQuiver;
            SetBowBehind();
        }
    }

    private void PullArrowFromQuiver() {
        player.PullArrowFromQuiver();
    }

    private void ReplaceArrowIntoQuiver() {
        player.ReplaceArrowIntoQuiver();
    }

    private void SetArrowNockedOnBowstring() {
        player.SetArrowNockedOnBowstring();
    }

    public void SetBowFront() {
        zRenderer.relativeSpriteLayers[riser] = frontRiser;
        zRenderer.relativeSpriteLayers[topLimb] = frontLimb;
        zRenderer.relativeSpriteLayers[topCam] = frontCam;
        zRenderer.relativeSpriteLayers[bottomLimb] = frontLimb;
        zRenderer.relativeSpriteLayers[bottomCam] = frontCam;
        zRenderer.relativeLineRendererLayers[bowstring] = frontBowstring;
        zRenderer.relativeLineRendererLayers[topCable] = frontTopCable;
        zRenderer.relativeLineRendererLayers[bottomCable] = frontBottomCable;
    }

    public void SetBowBehind() {
        zRenderer.relativeSpriteLayers[riser] = behindRiser;
        zRenderer.relativeSpriteLayers[topLimb] = behindLimb;
        zRenderer.relativeSpriteLayers[topCam] = behindCam;
        zRenderer.relativeSpriteLayers[bottomLimb] = behindLimb;
        zRenderer.relativeSpriteLayers[bottomCam] = behindCam;
        zRenderer.relativeLineRendererLayers[bowstring] = behindBowstring;
        zRenderer.relativeLineRendererLayers[topCable] = behindTopCable;
        zRenderer.relativeLineRendererLayers[bottomCable] = behindBottomCable;
    }
}
