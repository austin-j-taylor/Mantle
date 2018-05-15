using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controls the first-person camera.
 */

public class FPVCameraLock : MonoBehaviour {

    private Transform player;
    private Vector2 mouseLook;
    private Vector2 smoothV;
    private float sens = 1f;
    private float smooth = 2f;
    
    public Vector2 Direction {
        set {
            mouseLook = value;
        }
    }
    // Use this for initialization
    void Start() {
        player = transform.parent;
    }

    // Update is called once per frame
    void Update() {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseInput = Vector2.Scale(mouseInput, new Vector2(sens * smooth, sens * smooth));
        smoothV.x = Mathf.Lerp(smoothV.x, mouseInput.x, 1f / smooth);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseInput.y, 1f / smooth);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        player.localRotation = Quaternion.AngleAxis(mouseLook.x, player.up);
    }
}
