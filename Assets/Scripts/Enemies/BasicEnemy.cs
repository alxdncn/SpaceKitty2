using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBaseClass {

    // Use this for initialization
    protected override void Awake () {
		base.Awake ();
		SetColliderToMainObject ();
	}

}
