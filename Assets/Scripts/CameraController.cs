using UnityEngine;

/*
 * Controls the main, third-person camera.
 */

public class CameraController : MonoBehaviour {

    private PlayerMovementController player;
    private Transform cameraTarget;
    private Vector3 positionOffset;
    private float squareRootDOD;

    private float moveSmoothing = .05f;
    private int directionOffsetDistance = 10;
    private int degreesPerSecond = 120;
    public float rotationAngle;

    public float Angle {
        get {
            return rotationAngle;
        }
        set {
            rotationAngle = value;
        }
    }


    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
        positionOffset = new Vector3(0, 13, -22);
        squareRootDOD = Mathf.Sqrt(directionOffsetDistance);
        cameraTarget = transform.parent;
        rotationAngle = 0;
    }

    void Update() {
        if (!player.FirstPerson) {
            // Set camera rotation
            int rotate = 0;
            if (Input.GetKey(KeyCode.Q)) {
                rotate = -degreesPerSecond;
            } else if (Input.GetKey(KeyCode.E)) {
                rotate = degreesPerSecond;
            }

            rotationAngle = (rotationAngle + rotate * Time.deltaTime) % 360;

            float yLerped = Mathf.LerpAngle(cameraTarget.localEulerAngles.y, rotationAngle, 12 * Time.deltaTime);
            RotateCameraToAngle(yLerped);

        }
        // Set camera position
        Vector3 offset = Input.GetKey(KeyCode.Space) ? FollowMouse() : Vector3.zero;
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, player.transform.position + cameraTarget.localRotation * (offset), 12 * Time.deltaTime);
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
    public void RotateCameraToAngle(float angle) {
        cameraTarget.localEulerAngles = new Vector3(cameraTarget.localEulerAngles.x, angle, cameraTarget.localEulerAngles.z);
    }
}
