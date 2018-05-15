using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controlls player movement and certain first-person camera control.
 * 
 */

/* Walking Directions 
 *        N
 *        3       
 *      4   2 
 * W  5   0   1  E
 *      6   8    
 *        7
 *        S
 */
public enum Direction : short {
    None,
    West,
    Northwest,
    North,
    Northeast,
    East,
    Southeast,
    South,
    Southwest
}

public class PlayerMovementController : MonoBehaviour {

    public Animator animator;
    private float speed = 3f;
    private bool firstPV = false;

    private Direction movingDirection = Direction.None;
    private Camera thirdPersonCamera;
    private Camera firstPersonCamera;
    private CameraController cameraController;
    private FPVCameraLock cameraLock;
    private Rigidbody rb;

    private static GameObject thePlayer;

    public bool FirstPerson {
        get {
            return firstPV;
        }
    }

    private void Start() {
        //Cursor.lockState = CursorLockMode.Locked;
        thirdPersonCamera = Camera.main;
        firstPersonCamera = GetComponentInChildren<Camera>();
        cameraLock = GetComponentInChildren<FPVCameraLock>();
        cameraController = thirdPersonCamera.GetComponent<CameraController>();
        thePlayer = gameObject;
        rb = GetComponent<Rigidbody>();

        thirdPersonCamera.enabled = true;
        firstPersonCamera.enabled = false;
        cameraLock.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            firstPV = !firstPV;
            if (firstPV) {
                // changing to first person view. Update player rotation to match camera angle. Make player head invisible.
                thirdPersonCamera.enabled = false;
                firstPersonCamera.enabled = true;
                cameraLock.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                transform.localEulerAngles = new Vector3(0, 30, 0);
                cameraLock.Direction = new Vector2(cameraController.Angle, 0);
            } else {
                // changing to third person view. Update camera angle to reflect current vieiwing angle. Make player head visible.
                thirdPersonCamera.enabled = true;
                firstPersonCamera.enabled = false;
                cameraLock.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                cameraController.Angle = transform.localEulerAngles.y;
                cameraController.RotateCameraToAngle(cameraController.Angle); // skips Lerping
            }
        }
    }

    void FixedUpdate() {
        float horiz = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horiz, 0f, verti);
        movement = movement.normalized * speed * Time.deltaTime;

        if (firstPV) {
            //transform.Translate(2 * horiz * Time.deltaTime, 0, 2 * verti * Time.deltaTime);

            rb.MovePosition(transform.position + transform.TransformDirection(movement));

        } else {
            // makes movement relative to camera
            movement = Quaternion.Euler(0, cameraController.Angle, 0) * movement;

            if (horiz != 0 || verti != 0) {
                animator.SetBool("IsJustWalking", true);

                if (horiz == 0) {
                    if (verti > 0) {
                        animator.SetInteger("WalkingDirection", 2);
                        movingDirection = Direction.North;
                    } else {
                        animator.SetInteger("WalkingDirection", 4);
                        movingDirection = Direction.South;
                    }
                } else if (horiz > 0) {
                    animator.SetInteger("WalkingDirection", 1);
                    animator.SetInteger("LastFacingHorizontalDirection", 1);
                    if (verti > 0) {
                        movingDirection = Direction.Northeast;
                    } else if (verti < 0) {
                        movingDirection = Direction.Southeast;
                    } else {
                        movingDirection = Direction.East;
                    }
                } else {
                    animator.SetInteger("WalkingDirection", 3);
                    animator.SetInteger("LastFacingHorizontalDirection", 3);
                    if (verti > 0) {
                        movingDirection = Direction.Northwest;
                    } else if (verti < 0) {
                        movingDirection = Direction.Southwest;
                    } else {
                        movingDirection = Direction.West;
                    }
                }

            } else {
                animator.SetInteger("WalkingDirection", 0);
                animator.SetBool("IsJustWalking", false);
                movingDirection = Direction.None;
            }

            Vector3 eulers = transform.eulerAngles;
            if(horiz != 0 || verti != 0) {
                Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
                newRotation.x = 0;
                newRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
            }
            transform.Translate(movement, Space.World);
        }
    }

    public Direction GetMovingDirection() {
        return movingDirection;
    }

    public static Vector3 GetPlayerPosition() {
        if (thePlayer != null)
            return thePlayer.transform.position;

        return Vector3.zero;
    }
}
