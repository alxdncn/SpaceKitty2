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
        transform.position = GetPosition(EnemyManager.Instance.minX, EnemyManager.Instance.maxX, EnemyManager.Instance.minY, EnemyManager.Instance.maxY);
	}

	public virtual void Reset(){
        transform.position = GetPosition(EnemyManager.Instance.minX, EnemyManager.Instance.maxX, EnemyManager.Instance.minY, EnemyManager.Instance.maxY);
		gameObject.SetActive (true);
		for (int i = 0; i < allCols.Length; i++) {
			allCols[i].enabled = true;
		}
		hitPoints = startHitPoints;
		hit = false;
	}
    
    Vector3 GetPosition(float minX, float maxX, float minY, float maxY){
        int side = Random.Range(0,4);

        switch(side){
            case 0: //Left
                return new Vector3(minX, Random.Range(minY, maxY), 0);
            case 1: //Right
                return new Vector3(maxX, Random.Range(minY, maxY), 0);
            case 2: //Bottom
                return new Vector3(Random.Range(minX, maxX), minY, 0);
            case 3: //Top
                return new Vector3(Random.Range(minX, maxX), maxY, 0);
        }
        return Vector3.zero;
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
		GameStateManager.instance.ChangeScore();

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
