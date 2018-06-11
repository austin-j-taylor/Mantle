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

    private Animator animator;
    private Rigidbody rb;
    private CameraController cameraController;
    private Direction movingDirection = Direction.None;
    private static GameObject thePlayer;

    private float speed = 3f;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        cameraController = Camera.main.GetComponent<CameraController>();
        thePlayer = gameObject;
    }

    void FixedUpdate() {
        float horiz = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horiz, 0f, verti);
        movement = movement.normalized * speed * Time.deltaTime;

        if (cameraController.FirstPerson) {
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
