using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaghettiChild : MonoBehaviour {

	SpaghettiEnemy parentEnemyScript;

	int pixelCollidersTouching = 0;
	// Use this for initialization
	void Awake () {
		parentEnemyScript = GetComponentInParent<SpaghettiEnemy>();
	}

	void OnEnable(){
		pixelCollidersTouching = 0;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "PixelColliders"){
			if(pixelCollidersTouching <= 0 && 
			(GameStateManager.instance.currentState == GameStateManager.State.Running || 
			GameStateManager.instance.currentState == GameStateManager.State.TimesUp)){
				parentEnemyScript.ChildHit(transform);
			}
			pixelCollidersTouching++;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "PixelColliders") {
			pixelCollidersTouching--;
			if(pixelCollidersTouching <= 0){
				parentEnemyScript.ChildUnhit(transform);
				pixelCollidersTouching = 0;
			}
		}
	}
}
