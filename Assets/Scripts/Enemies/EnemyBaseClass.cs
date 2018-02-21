using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour {

	protected Collider2D[] allCols;

	public virtual void Reset(){
		gameObject.SetActive (true);
		for (int i = 0; i < allCols.Length; i++) {
			allCols[i].enabled = true;
		}
	}
		
	public virtual void DestroyEnemy(){
		for (int i = 0; i < allCols.Length; i++) {
			allCols[i].enabled = false;
		}

		//DO ANIMATION

		EnemyManager.Instance.EnemyIsDestroyed (this);
	}

	protected virtual void Move(){

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "PixelColliders") {
			DestroyEnemy ();
		}
	}
}
