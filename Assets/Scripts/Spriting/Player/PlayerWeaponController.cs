using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

    public Shooter shooter;

    public LayerMask ignorePlayerLayer;
    private float maxRange = 100f;

    void Start() {
        ignorePlayerLayer = ~(1 << LayerMask.NameToLayer("Player"));
    }

    void Update() {

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // straightRayToTarget goes from CAMERA towards SCREEN which likely WILL INTERSECT WITH THE PHYSICAL REALM

        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, maxRange, ignorePlayerLayer)) { // target point selected

            //shooter.PredictPath(launchVelocity, hit.point);

            if (Input.GetButtonDown("Fire1")) {

                Vector3 launchVelocity = shooter.CalculateLaunchVelocity(hit.point, !Input.GetButton("Fire3"));
                shooter.Fire(launchVelocity);
            }
        }
    }
}
