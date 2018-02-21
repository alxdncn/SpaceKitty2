using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBaseClass : MonoBehaviour {

	protected Collider2D col;

	public virtual void Reset(){
		gameObject.SetActive (true);
		gameObject.transform.position = new Vector3 (Random.Range (-5f, 5f), Random.Range (-5f, 5f), 0);
	}
		
	public virtual void DestroyEnemy(){
		col.enabled = false;

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
