using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controls the first-person camera.
 */

public class FPVCameraLock : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;
    private float sens = 1f;
    private float smooth = 2f;

    GameObject character;

    public Vector2 Direction {
        set {
            mouseLook = value;
        }
    }
    // Use this for initialization
    void Start() {
        character = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update() {
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sens * smooth, sens * smooth));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smooth);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smooth);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}
