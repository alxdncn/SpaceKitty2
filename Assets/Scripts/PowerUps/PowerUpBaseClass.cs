using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PowerUpBaseClass : MonoBehaviour {
	protected Collider2D col;

	protected virtual void Move(){

	}

	protected void PlayerHit(){
		PowerUpHandler.Instance.GotPowerup (this);
		DestroyPowerUp ();
	}

	protected virtual void DestroyPowerUp(){
		col.enabled = false;

		//DO ANIMATIONS

		PowerUpHandler.Instance.SetInactivePUObject (this, gameObject);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "PixelColliders") {
			PlayerHit ();
		}
	}

}
