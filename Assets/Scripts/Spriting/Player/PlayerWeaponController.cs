using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

    public Bow bow;

    private LayerMask ignorePlayerLayer;
    private float maxRange = 100f;

    void Start() {
        ignorePlayerLayer = ~(1 << LayerMask.NameToLayer("Player"));
    }

    void Update() {

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // goes from CAMERA towards SCREEN which likely WILL INTERSECT WITH THE PHYSICAL REALM
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, maxRange, ignorePlayerLayer)) { // target point selected, should always be true
            // shooter.PredictPath(launchVelocity, hit.point);

            // if using bow
            BowUpdate(hit);

            // if using crossbow:

        }
    }

    private void BowUpdate(RaycastHit hit) {
        if (bow.Loaded) { // is already loaded
            // if right click, lower bow, keeping arrow nocked
            if (Input.GetButtonDown("Fire2")) {
                bow.Undraw();
            } else

            // if not holding left click, fire
            if (!Input.GetButton("Fire1")) {
                Vector3 launchVelocity = bow.CalculateLaunchVelocity(hit.point, !Input.GetButton("Fire3"));
                bow.Fire(launchVelocity);
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
                // TODO make me littler
                Vector3 launchVelocity = (bow.DrawingTime / bow.LoadTime) *  bow.CalculateLaunchVelocity(hit.point, !Input.GetButton("Fire3"));
                bow.Fire(launchVelocity);
            }

        } else { // not loaded, is not alreading loading
                 // if left click, begin loading bow
            if (Input.GetButtonDown("Fire1")) {
                bow.Load();
            } else
            // if not trying to load same frame, just draw arrow from quiver and nock arrow (or replace arrow into quiver)
            if (Input.GetButtonDown("Fire2")) {
                if (bow.Nocked) {
                    bow.Unnock();
                } else {
                    bow.Nock();
                }
            }

        }
    }
}
