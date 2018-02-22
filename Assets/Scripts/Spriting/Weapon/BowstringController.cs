using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowstringController : MonoBehaviour {

    public Transform nockPosition;
    public Transform nock;
    public Transform topLimb;
    public Transform bottomLimb;
    public Transform topCam;
    public Transform bottomCam;
    public Transform topCableAnchor;
    public Transform bottomCableAnchor;
    public Transform topStringAnchor;
    public Transform bottomStringAnchor;
    public Transform topPivot;
    public Transform bottomPivot;

    public Transform leftHandProjectileAnchor;

    public Bow bow;

    private LineRenderer bowstring;
    private LineRenderer topCable;
    private LineRenderer bottomCable;

    //private const int largeUpperAngle = 349;
    //private const int largeLowerAngle = 113;
    private const int outerUpperAngle = 348;
    private const int outerLowerAngle = 112;
    private const int innerUpperAngle = 270;
    private const int innerLowerAngle = 165;

    void Start() {
        bowstring = GetComponent<LineRenderer>();
        topCable = topPivot.GetComponent<LineRenderer>();
        bottomCable = bottomPivot.GetComponent<LineRenderer>();

        topCable.SetPosition(0, Vector3.zero);
        bottomCable.SetPosition(0, Vector3.zero);

        topPivot.position = topCam.position;
        bottomPivot.position = bottomCam.position;

        nockPosition.position = (topStringAnchor.position + bottomStringAnchor.position) / 2;

    }

    void Update() {


        // cam rotation magic
        float drawDistance = nock.localPosition.x;
        topCam.localRotation = Quaternion.Euler(0, 0, -drawDistance * 450);
        bottomCam.localRotation = Quaternion.Euler(0, 0, drawDistance * 450);

        topLimb.localRotation = Quaternion.Euler(0, 0, -drawDistance * 15);
        bottomLimb.localRotation = Quaternion.Euler(0, 0, drawDistance * 15);

        Vector3 camOuterLargeRadius = new Vector3(.05f, 0, 0);
        Vector3 camOuterSmallRadius = new Vector3(.025f, 0, 0);

        Vector3 camInnerLargeRadius = new Vector3(.027f, 0, 0);
        Vector3 camInnerSmallRadius = new Vector3(.0125f, 0, 0);

        // setting string angular positions

        //float stringAngularRotation = -drawDistance * 570;
        float stringAngularRotation = -drawDistance * 570;

        stringAngularRotation %= 360;
        if (stringAngularRotation < 0) {
            stringAngularRotation += 360;
        }

        if (stringAngularRotation > outerLowerAngle && stringAngularRotation < outerUpperAngle) {
            topStringAnchor.localPosition = Quaternion.AngleAxis(stringAngularRotation, transform.forward) * camOuterLargeRadius;
            bottomStringAnchor.localPosition = Quaternion.AngleAxis(-stringAngularRotation, transform.forward) * camOuterLargeRadius;
        } else {
            topStringAnchor.localPosition = Quaternion.AngleAxis(stringAngularRotation, transform.forward) * camOuterSmallRadius + new Vector3(.0375f, -.0375f, 0);
            bottomStringAnchor.localPosition = Quaternion.AngleAxis(-stringAngularRotation, transform.forward) * camOuterSmallRadius + new Vector3(.0375f, .0375f, 0);
        }

        //float cableAngularRotation = -drawDistance * 500 + 180;
        float cableAngularRotation = -drawDistance * 500 + 180;

        cableAngularRotation %= 360;
        if (cableAngularRotation < 0) {
            cableAngularRotation += 360;
        }

        if (!(cableAngularRotation > innerLowerAngle && cableAngularRotation < innerUpperAngle)) {
            topCableAnchor.localPosition = Quaternion.AngleAxis(cableAngularRotation, transform.forward) * camInnerLargeRadius;
            bottomCableAnchor.localPosition = Quaternion.AngleAxis(-cableAngularRotation, transform.forward) * camInnerLargeRadius;
        } else {
            topCableAnchor.localPosition = Quaternion.AngleAxis(cableAngularRotation, transform.forward) * camInnerSmallRadius + new Vector3(-.01875f, .01875f, 0);
            bottomCableAnchor.localPosition = Quaternion.AngleAxis(-cableAngularRotation, transform.forward) * camInnerSmallRadius + new Vector3(-.01875f, -.01875f, 0);
        }


    }

    // if bow is being drawn, make the nock's position equal to the player's projectile anchor.
    // May be called from multiple times per frame because Unity's animation system is  hard to work around.
    public void AttachStringToHand() {
        nock.position = leftHandProjectileAnchor.position;
    }

    private void LateUpdate() {
        if (bow.IsLoading || bow.Loaded)
            AttachStringToHand();

        // update line positions
        bowstring.SetPosition(0, transform.InverseTransformPoint(topStringAnchor.position));
        bowstring.SetPosition(2, transform.InverseTransformPoint(bottomStringAnchor.position));
        bowstring.SetPosition(1, transform.InverseTransformPoint(nock.position));

        topCable.SetPosition(1, topPivot.InverseTransformPoint(bottomCableAnchor.position));
        bottomCable.SetPosition(1, bottomPivot.InverseTransformPoint(topCableAnchor.position));
    }
}
