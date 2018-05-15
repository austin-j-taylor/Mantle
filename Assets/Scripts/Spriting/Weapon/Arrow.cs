using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile {
    
	void Start () {
        base.Start();
        //nockedPositionOffset = new Vector3(-.442f, 0, 0);
        nockedPositionOffset = new Vector3(-.677f, 0, 0);
        damage = 5;
    }
	
}
