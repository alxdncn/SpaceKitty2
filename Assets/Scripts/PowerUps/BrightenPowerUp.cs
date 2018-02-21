using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightenPowerUp : PowerUpBaseClass {

	// Use this for initialization
	void Awake () {
		col = GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.B)) {
			PlayerHit ();
		}
	}

	protected override void Move ()
	{
		
	}
}
