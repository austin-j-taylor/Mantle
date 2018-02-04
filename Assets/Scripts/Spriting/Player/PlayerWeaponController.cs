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
    }

    void Update() {

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

        Vector3 launchVelocity = bow.CalculateLaunchVelocity(hit.point, !Input.GetButton("Fire3"));

        if (Input.GetKeyDown(KeyCode.F)) {
            funMode = !funMode;
        }
        if(funMode && Input.GetButton("Fire1")) {
            // fun mode
            bow.Fire(launchVelocity, currentlyHeldProjectile);
        }

        if (bow.Loaded) { // is already loaded
            // if right click, lower bow, keeping arrow nocked
            if (Input.GetButtonDown("Fire2")) {
                bow.Undraw();
            } else

            // if not holding left click, fire
            if (!Input.GetButton("Fire1")) {
                anim.SetTrigger("Fire");
                bow.Fire(launchVelocity, currentlyHeldProjectile);
            }
        } else
            if (bow.IsLoading) { // is currently loading
                                 // if right click, carefully release draw, keeping arrow nocked
            if (Input.GetButtonDown("Fire2")) {
                bow.CeaseLoad();
                bow.Undraw();
            } else
            // if release click, fire before full draw for less launch velocty
            if (!Input.GetButton("Fire1")) {
                bow.CeaseLoad();
                // make me littler launch velocity
                anim.SetTrigger("Fire");
                bow.Fire((bow.DrawingTime / bow.LoadTime) * launchVelocity, currentlyHeldProjectile);
            }

        } else { // not loaded, is not alreading loading
                 // if left click, begin loading bow
            if (Input.GetButtonDown("Fire1")) {

                bow.Load();
            } else
            // if not trying to load same frame, just draw arrow from quiver and nock arrow (or replace arrow into quiver)
            if (Input.GetButtonDown("Fire2")) {
                if (bow.Nocked) {
                    anim.SetTrigger("Unnock");
                    bow.Unnock();
                } else {
                    anim.SetTrigger("Nock");
                    bow.Nock();
                }
            }

        }
    }
    public void PullArrowFromQuiver() {
        currentlyHeldProjectile = bow.SpawnProjectile(quiver.transform.position + new Vector3(.07f, 0, 0), quiver.transform.localRotation * Quaternion.Euler(180, 90, 0));
        currentlyHeldProjectile.transform.SetParent(projectileAnchor, true);
    }
}
