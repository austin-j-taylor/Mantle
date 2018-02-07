using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

    public Bow bow;
    public GameObject quiver;
    public Transform projectileAnchor;

    private Projectile currentlyHeldProjectile;

    private Animator anim;
    private LayerMask ignorePlayerLayer;
    private float maxRange = 100f;
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

        Vector3 launchVelocity = bow.CalculateLaunchVelocity(hit.point, (currentlyHeldProjectile == null) ? bow.transform.position : currentlyHeldProjectile.transform.position, !Input.GetButton("Fire3"));

        if (Input.GetKeyDown(KeyCode.F)) {
            funMode = !funMode;
        }
        if (funMode && Input.GetButton("Fire1")) {
            // fun mode
            PullArrowFromQuiver();
            bow.Fire(launchVelocity, currentlyHeldProjectile);
            currentlyHeldProjectile = null;
        }

        if (bow.Loaded) { // is already loaded
            // if right click, lower bow, keeping arrow nocked
            if (Input.GetButtonDown("Fire2")) {
                anim.SetBool("IsDrawing", false);
                anim.SetTrigger("Unnock");
                bow.Undraw();
            } else

            // if not holding left click, fire
            if (!Input.GetButton("Fire1")) {
                anim.SetBool("IsDrawing", false);
                anim.SetTrigger("Fire");
                bow.Fire(launchVelocity, currentlyHeldProjectile);
                currentlyHeldProjectile = null;
            }
        } else
            if (bow.IsLoading) { // is currently loading
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
                anim.SetBool("IsDrawing", false);
                anim.SetTrigger("Fire");
                bow.Fire((bow.DrawingTime / bow.LoadTime) * launchVelocity, currentlyHeldProjectile);
                currentlyHeldProjectile = null;
            }

        } else { // not loaded, is not alreading loading
                 // if nocked and left click, begin loading bow
            if (bow.Nocked) {
                if (Input.GetButtonDown("Fire2")) {
                    // replace arrow into quiver
                    anim.SetTrigger("Unnock");
                    SetArrowNotNockedOnBowstring();
                } else
                if (Input.GetButtonDown("Fire1")) {
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

    public void PullArrowFromQuiver() {
        currentlyHeldProjectile = bow.SpawnProjectile(quiver.transform.position + new Vector3(.07f, 0, 0), quiver.transform.localRotation * Quaternion.Euler(180, 90, 0));
        currentlyHeldProjectile.transform.SetParent(projectileAnchor, true);
    }

    public void ReplaceArrowIntoQuiver() {
        Destroy(currentlyHeldProjectile.gameObject);
    }

    public void SetArrowNockedOnBowstring() {
        bow.Nocked = true;
        currentlyHeldProjectile.transform.SetParent(bow.nockPosition);
        currentlyHeldProjectile.SetTailPositionNocked();
    }

    private void SetArrowNotNockedOnBowstring() {
        bow.Nocked = false;
        currentlyHeldProjectile.transform.SetParent(projectileAnchor, true);
        //currentlyHeldProjectile.SetTailPositionNotNocked();
    }
}
