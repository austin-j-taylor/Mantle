using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private PlayerMovementController player;
    private Vector3 currentOffset;
    private Vector3 rotationOffset;
    private GameObject targetTransform;
    private Vector3 positionOffset;
    private float squareRootDOD;

    private float turnSmoothing = 10f;
    private float moveSmoothing = .05f;
    private int directionOffsetDistance = 10;
    private int degreesRotation = 120;


    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
        positionOffset = new Vector3(0, 13, -22);
        squareRootDOD = Mathf.Sqrt(directionOffsetDistance);
        targetTransform = new GameObject();
        currentOffset = new Vector3();
    }

    private void LateUpdate() {
        Vector3 offset = Input.GetKey(KeyCode.Space) ? FollowMouse() : Vector3.zero;

        currentOffset = Vector3.Lerp(currentOffset, player.transform.rotation * offset, moveSmoothing);
        //transform.LookAt(player.transform.position + currentOffset);
    }

    void Update() {
        Vector3 positionBeforeRotation = targetTransform.transform.position;

        int rotate = 0;
        //if (Input.GetKey(KeyCode.Q)) {
        //    rotate = -degreesRotation;
        //} else if (Input.GetKey(KeyCode.E)) {
        //    rotate = degreesRotation;
        //}

        targetTransform.transform.RotateAround(PlayerMovementController.GetPlayerPosition(), Vector3.up, rotate * Time.deltaTime);
        Vector3 deltaRotation = targetTransform.transform.position - positionBeforeRotation;
        rotationOffset = rotationOffset + deltaRotation;


        /* currentOffset is applied to position instead of rotation in FixedUpdate until Unity resolves its Japes */

        Vector3 offset = Input.GetKey(KeyCode.Space) ? FollowMouse() : Vector3.zero;
        currentOffset = Vector3.Lerp(currentOffset, player.transform.rotation * offset, moveSmoothing);
        
        /**/

        targetTransform.transform.position = PlayerMovementController.GetPlayerPosition() + positionOffset + rotationOffset + currentOffset;// + offset; // uncomment for fun times
        transform.position = Vector3.Lerp(transform.position, targetTransform.transform.position, turnSmoothing * Time.deltaTime);

        

    }

    Vector3 FollowMovement() {
        Direction facingDirection = player.GetMovingDirection();
        Vector3 directionOffset;

        switch (facingDirection) {
            case Direction.East:
                directionOffset = new Vector3(directionOffsetDistance, 0, 0);
                break;
            case Direction.Northeast:
                directionOffset = new Vector3(squareRootDOD, 0, squareRootDOD);
                break;
            case Direction.North:
                directionOffset = new Vector3(0, 0, directionOffsetDistance);
                break;
            case Direction.Northwest:
                directionOffset = new Vector3(-squareRootDOD, 0, squareRootDOD);
                break;
            case Direction.West:
                directionOffset = new Vector3(-directionOffsetDistance, 0, 0);
                break;
            case Direction.Southwest:
                directionOffset = new Vector3(-squareRootDOD, 0, -squareRootDOD);
                break;
            case Direction.South:
                directionOffset = new Vector3(0, 0, -directionOffsetDistance);
                break;
            case Direction.Southeast:
                directionOffset = new Vector3(squareRootDOD, 0, -squareRootDOD);
                break;
            default:
                directionOffset = new Vector3(0, 0, 0);
                break;
        }

        return directionOffset;
    }

    Vector3 FollowMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.x -= Screen.width / 2;
        mousePosition.y -= Screen.height / 2;

        Vector3 normalizedMousePosition = new Vector3(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        Vector3 directionOffset = directionOffsetDistance * new Vector3(normalizedMousePosition.x, 0, normalizedMousePosition.y);

        return directionOffset;

    }
}
