using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Direction movingDirection = Direction.None;
    private Camera rotationController;
    private Rigidbody rb;

    private static GameObject thePlayer;

    private void Start() {
        rotationController = Camera.main;
        thePlayer = gameObject;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        float horiz = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");
        
        Vector3 movement = new Vector3(horiz, 0f, verti);

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
        
        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + transform.TransformDirection(movement));

        transform.rotation = Quaternion.Euler(new Vector3(0f, rotationController.transform.rotation.eulerAngles.y, 0f));
    }

    public Direction GetMovingDirection() {
        return movingDirection;
    }

    public static Vector3 GetPlayerPosition() {
        if(thePlayer != null)
            return thePlayer.transform.position;
        
        return Vector3.zero;
    }
}
