using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowController : MonoBehaviour {

    private PlayerWeaponController player;
    private SpriteZLevelRendering zRenderer;
    private Transform playerBody;
    private SpriteRenderer projectileSpriteHolder1;
    private SpriteRenderer projectileSpriteHolder2;

    private bool playerIsHoldingArrow;

    // Default layer values
    private const int frontQuiver = 57;
    private const int behindQuiver = 43;
    private const int frontLimb = 70;
    private const int behindLimb = 30;
    private const int frontRiser = 71;
    private const int behindRiser = 29;
    private const int frontProjectile1 = 80;
    private const int behindProjectile1 = 20;
    private const int frontProjectile2 = 81;
    private const int behindProjectile2 = 19;
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
    private const int projectile1 = 2;
    private const int projectile2 = 3;
    private const int riser = 5;
    private const int topLimb = 6;
    private const int topCam = 7;
    private const int bottomLimb = 8;
    private const int bottomCam = 9;
    private const int quiver = 10;
    private const int bowstring = 1;
    private const int topCable = 2;
    private const int bottomCable = 3;
    
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeaponController>();
        zRenderer = gameObject.GetComponent<SpriteZLevelRendering>();

        projectileSpriteHolder1 = zRenderer.spriteChildren[projectile1];
        projectileSpriteHolder2 = zRenderer.spriteChildren[projectile2];

        playerBody = zRenderer.spriteChildren[body].transform;
        playerIsHoldingArrow = false;
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

        // if the player switches to holding an arrow, notify the sprite renderer
        if(playerIsHoldingArrow && player.HeldProjectile == null) {
            playerIsHoldingArrow = false;
            ReplaceProjectileWithDummy();
        } else if(!playerIsHoldingArrow && player.HeldProjectile != null) {
            playerIsHoldingArrow = true;
            ReplaceDummyWithProjectile();
        }
    }

    private void PullArrowFromQuiver() {
        player.PullArrowFromQuiver();
        // replace dummy sprite with arrow sprite
        ReplaceDummyWithProjectile();
    }

    private void ReplaceArrowIntoQuiver() {
        player.ReplaceArrowIntoQuiver();
        // replace arrow sprite with dummy sprite
        ReplaceProjectileWithDummy();
    }

    private void SetArrowNockedOnBowstring() {
        player.SetArrowNockedOnBowstring();
    }

    // Replaces the dummy sprite in player's left hand with the player's actual projectile
    private void ReplaceDummyWithProjectile() {
        zRenderer.spriteChildren[projectile1] = player.HeldProjectile.GetComponentsInChildren<SpriteRenderer>()[0];
        zRenderer.spriteChildren[projectile2] = player.HeldProjectile.GetComponentsInChildren<SpriteRenderer>()[1];
    }

    // Vice-versa
    private void ReplaceProjectileWithDummy() {
        zRenderer.spriteChildren[projectile1] = projectileSpriteHolder1;
        zRenderer.spriteChildren[projectile2] = projectileSpriteHolder2;
    }

    public void SetBowFront() {
        zRenderer.relativeSpriteLayers[riser] = frontRiser;
        zRenderer.relativeSpriteLayers[projectile1] = frontProjectile1;
        zRenderer.relativeSpriteLayers[projectile2] = frontProjectile2;
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
        zRenderer.relativeSpriteLayers[projectile1] = behindProjectile1;
        zRenderer.relativeSpriteLayers[projectile2] = behindProjectile2;
        zRenderer.relativeSpriteLayers[topLimb] = behindLimb;
        zRenderer.relativeSpriteLayers[topCam] = behindCam;
        zRenderer.relativeSpriteLayers[bottomLimb] = behindLimb;
        zRenderer.relativeSpriteLayers[bottomCam] = behindCam;
        zRenderer.relativeLineRendererLayers[bowstring] = behindBowstring;
        zRenderer.relativeLineRendererLayers[topCable] = behindTopCable;
        zRenderer.relativeLineRendererLayers[bottomCable] = behindBottomCable;
    }
}
