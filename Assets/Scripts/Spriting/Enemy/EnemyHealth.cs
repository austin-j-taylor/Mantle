using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    private double health;
    private double maxHealth;

    public double Health {
        get {
            return health;
        }
        set {
            health = value;
            if (health < 0)
                health = 0;
        }
    }
    public double MaxHealth {
        get {
            return maxHealth;
        }
        set {
            maxHealth = value;
            if (maxHealth < 0)
                maxHealth = 0;
        }
    }
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
