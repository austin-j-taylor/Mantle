using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Allows child sprites to be dynamically layered at runtime depending on their relative position to the player and the camera's rotation.
 * So, the sprites visually act like 3D objects.
 * 
 */
 
public class SpriteZLevelRendering : MonoBehaviour {

    [HideInInspector]
    public int[] relativeLayers;

    private const int IsometricRangePerZUnit = 10;
    private const int LayerDifferenceConstant = 100;
    
    private SpriteRenderer[] spriteChildren;
    private Transform movingObject;

    private void Start() {
        movingObject = transform.parent;

        spriteChildren = GetComponentsInChildren<SpriteRenderer>();

        relativeLayers = new int[spriteChildren.Length];
        for (int i = 0; i < spriteChildren.Length; i++) {
            relativeLayers[i] = spriteChildren[i].sortingOrder;
        }
    }
    void Update () {
        Vector3 playerPosition = PlayerMovementController.GetPlayerPosition();
        float cameraY = Camera.main.transform.rotation.eulerAngles.y;

        Ray playerAxis = new Ray(playerPosition, Quaternion.AngleAxis(cameraY, Vector3.up) * Vector3.right);
        Vector3 crossProduct = Vector3.Cross(playerAxis.direction, movingObject.position - playerAxis.origin);

        float distanceToPlayerAxis = crossProduct.magnitude * Mathf.Sign(crossProduct.y); 
        float layeringValue = distanceToPlayerAxis;

        for (int i = 0; i < spriteChildren.Length; i++) {
            int sortingOrder = (int)(layeringValue * IsometricRangePerZUnit) * LayerDifferenceConstant + relativeLayers[i];
            spriteChildren[i].sortingOrder = sortingOrder;
        }
    }
}
