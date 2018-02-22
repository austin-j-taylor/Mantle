using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Allows child sprites to be dynamically layered at runtime depending on their relative position to the player and the camera's rotation.
 * So, the sprites visually act like 3D objects.
 * 
 */
 
public class SpriteZLevelRendering : MonoBehaviour {

    //[HideInInspector]
    public int[] relativeSpriteLayers;
    public int[] relativeLineRendererLayers;

    private const int IsometricRangePerZUnit = 10;
    private const int LayerDifferenceConstant = 100;
    
    public SpriteRenderer[] spriteChildren;
    public LineRenderer[] lineRendererChildren;

    private Transform movingObject;

    private void Awake() {
        movingObject = transform;

        spriteChildren = GetComponentsInChildren<SpriteRenderer>();
        lineRendererChildren = GetComponentsInChildren<LineRenderer>();

        relativeSpriteLayers = new int[spriteChildren.Length];
        for (int i = 0; i < spriteChildren.Length; i++) {
            relativeSpriteLayers[i] = spriteChildren[i].sortingOrder;
        }

        relativeLineRendererLayers = new int[lineRendererChildren.Length];
        for (int i = 0; i < lineRendererChildren.Length; i++) {
            relativeLineRendererLayers[i] = lineRendererChildren[i].sortingOrder;
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
            int sortingOrder = (int)(layeringValue * IsometricRangePerZUnit) * LayerDifferenceConstant + relativeSpriteLayers[i];
            spriteChildren[i].sortingOrder = sortingOrder;
        }

        for (int i = 0; i < lineRendererChildren.Length; i++) {
            int sortingOrder = (int)(layeringValue * IsometricRangePerZUnit) * LayerDifferenceConstant + relativeLineRendererLayers[i];
            lineRendererChildren[i].sortingOrder = sortingOrder;
        }
    }

    //public void UpdateSpriteChildren() {
    //    spriteChildren = GetComponentsInChildren<SpriteRenderer>();

    //    relativeSpriteLayers = new int[spriteChildren.Length];
    //    for (int i = 0; i < spriteChildren.Length; i++) {
    //        relativeSpriteLayers[i] = spriteChildren[i].sortingOrder;
    //    }

    //}

}
