using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargStationaryController : Targ {

    new void Start() {
        base.Start();

        health.MaxHealth = 10;
    }
}
