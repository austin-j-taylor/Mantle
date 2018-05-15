using UnityEngine;

/*
 * Controls the main, third-person camera and toggling between first and third person.
 */

public class CameraController : MonoBehaviour {

    private PlayerMovementController player;
    private Camera thirdPersonCamera;
    private Camera firstPersonCamera;
    private FPVCameraLock cameraLock;
    private Transform cameraTarget;
    private Vector3 positionOffset;
    private float rotationAngle;
    private float squareRootDOD;
    private bool firstPerson;

    private float moveSmoothing = .05f;
    private int directionOffsetDistance = 10;
    private int degreesPerSecond = 120;

    public float Angle {
        get {
            return rotationAngle;
        }
        set {
            rotationAngle = value;
        }
    }
    public bool FirstPerson {
        get {
            return firstPerson;
        }
    }


    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
        thirdPersonCamera = GetComponent<Camera>();
        firstPersonCamera = player.GetComponentInChildren<Camera>();
        cameraLock = player.GetComponentInChildren<FPVCameraLock>();
        positionOffset = new Vector3(0, 13, -22);
        squareRootDOD = Mathf.Sqrt(directionOffsetDistance);
        cameraTarget = transform.parent;
        rotationAngle = 0;
        firstPerson = false;
        thirdPersonCamera.enabled = true;
        firstPersonCamera.enabled = false;
        cameraLock.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update() {
        // Toggling between first and third person
        if (Input.GetKeyDown(KeyCode.G)) {
            firstPerson = !firstPerson;
            if (firstPerson) {
                // changing to first person view. Update player rotation to match camera angle. Make player head invisible.
                thirdPersonCamera.enabled = false;
                firstPersonCamera.enabled = true;
                cameraLock.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                player.transform.localEulerAngles = new Vector3(0, 30, 0);
                cameraLock.Direction = new Vector2(rotationAngle, 0);
            } else {
                // changing to third person view. Update camera angle to reflect current vieiwing angle. Make player head visible.
                thirdPersonCamera.enabled = true;
                firstPersonCamera.enabled = false;
                cameraLock.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                rotationAngle = player.transform.localEulerAngles.y;
                RotateCameraToAngle(rotationAngle); // skips Lerping
            }
        }

        if (!firstPerson) {
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
