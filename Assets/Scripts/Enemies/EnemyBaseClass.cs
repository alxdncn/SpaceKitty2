using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour {

	protected Collider2D[] allCols;

	protected float coolDownTimer = 0f;
	[SerializeField] protected float coolDownTime = 0.5f;
	protected bool hit = false;

	[SerializeField] protected int startHitPoints = 1;
	protected int hitPoints;

	[SerializeField] protected float movementSpeed = 10f;

	protected virtual void Awake(){
		hitPoints = startHitPoints;
	}

	public virtual void Reset(){
		gameObject.SetActive (true);
		for (int i = 0; i < allCols.Length; i++) {
			allCols[i].enabled = true;
		}
		hitPoints = startHitPoints;
		hit = false;
	}

	protected virtual void Update(){
		Move ();
		if (hit) {
			if (coolDownTimer > coolDownTime) {
				coolDownTimer = 0f;
				hit = false;
			}

			coolDownTimer += Time.deltaTime;
		}
	}

	protected void SetColliderToMainObject(){
		Collider2D mainObjCollider = GetComponent<Collider2D> ();

		allCols = new Collider2D[1];

		if (mainObjCollider != null) {
			allCols [0] = mainObjCollider;
		} else {
			Debug.LogWarning ("No collider on object!");
		}
	}

	protected Vector3 GetVectorToKitty(Transform trans){
		Vector3 dist = new Vector3 (trans.position.x - Kitty.trans.position.x, trans.position.y - Kitty.trans.position.y, 0);

		return dist;
	}

	protected virtual void Move(){
		transform.position += -GetVectorToKitty (transform).normalized * movementSpeed * Time.deltaTime;
	}

	protected virtual void HitEnemy(){
		hitPoints--;
		hit = true;
	}

	public virtual void DestroyEnemy(){
		for (int i = 0; i < allCols.Length; i++) {
			allCols[i].enabled = false;
		}

		//DO ANIMATION

		EnemyManager.Instance.EnemyIsDestroyed (this);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "PixelColliders" && !hit) {
			HitEnemy ();
			if (hitPoints <= 0) {
				DestroyEnemy ();
			}
		}
	}
}
