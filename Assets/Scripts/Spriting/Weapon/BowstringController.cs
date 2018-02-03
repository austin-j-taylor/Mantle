using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowstringController : MonoBehaviour {

    public Transform nock;
    public Transform topCam;
    public Transform bottomCam;
    public Transform topCableAnchor;
    public Transform bottomCableAnchor;
    public Transform topStringAnchor;
    public Transform bottomStringAnchor;
    public Transform topPivot;
    public Transform bottomPivot;

    private LineRenderer bowstring;
    private LineRenderer topCable;
    private LineRenderer bottomCable;
    
    void Start () {
    
        bowstring = GetComponent<LineRenderer>();
        topCable = topPivot.GetComponent<LineRenderer>();
        bottomCable = bottomPivot.GetComponent<LineRenderer>();

        topCable.SetPosition(0, Vector3.zero);
        bottomCable.SetPosition(0, Vector3.zero);

        topPivot.position = topCam.position;
        bottomPivot.position = bottomCam.position;
        
        nock.position = (topStringAnchor.position + bottomStringAnchor.position) / 2;

    }
	
	void Update () {
        // update positions
        bowstring.SetPosition(0, transform.InverseTransformPoint(topStringAnchor.position));
        bowstring.SetPosition(2, transform.InverseTransformPoint(bottomStringAnchor.position));
        bowstring.SetPosition(1, transform.InverseTransformPoint(nock.position));

        topCable.SetPosition(1, topPivot.InverseTransformPoint(bottomCableAnchor.position));
        bottomCable.SetPosition(1, bottomPivot.InverseTransformPoint(topCableAnchor.position));

        // cam rotation magic
        float drawDistance = (nock.position.x - 1.05f) * 270;

        topCam.rotation = Quaternion.Euler(0, 0, -drawDistance);
        bottomCam.rotation = Quaternion.Euler(0, 0, drawDistance);


    }
}
