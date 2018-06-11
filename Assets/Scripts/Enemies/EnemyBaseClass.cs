using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour {

	protected Collider2D[] allCols;

	protected List<SpriteRenderer> allSprites;

	protected float coolDownTimer = 0f;
	[SerializeField] protected float coolDownTime = 0.5f;
	protected bool hit = false;

	[SerializeField] protected float cooldownFadeTime = 0.5f;
	float cooldownFadeTimer;
	bool faded = false;

	[SerializeField] [Range(0,1)] float cooldownAlpha = 0.5f;

	[SerializeField] protected int startHitPoints = 1;
	protected int hitPoints;

	[SerializeField] protected float movementSpeed = 10f;

	[SerializeField] Animator deathAnimator;
	protected string deathAnimationName = "Die";
	Transform deathAnimatorParent;
	Vector3 deathAnimatorStartPosition;

	protected virtual void Awake(){
		hitPoints = startHitPoints;
        transform.position = GetPosition(EnemyManager.Instance.minX, EnemyManager.Instance.maxX, EnemyManager.Instance.minY, EnemyManager.Instance.maxY);
		deathAnimatorParent = deathAnimator.transform.parent;
		deathAnimatorStartPosition = deathAnimator.transform.localPosition;
		SpriteRenderer[] spriteArray = GetComponentsInChildren<SpriteRenderer>();

		SpriteRenderer myRend = GetComponent<SpriteRenderer>();

		allSprites = new List<SpriteRenderer>();
		if(myRend != null){
			allSprites.Add(myRend);
		}

		for(int i = 0; i < spriteArray.Length; i++){
			if(spriteArray[i].gameObject.layer != LayerMask.NameToLayer("DeathAnimations")){
				allSprites.Add(spriteArray[i]);
			}
		}
	}

	public virtual void Reset(){
        transform.position = GetPosition(EnemyManager.Instance.minX, EnemyManager.Instance.maxX, EnemyManager.Instance.minY, EnemyManager.Instance.maxY);
		gameObject.SetActive (true);
		for (int i = 0; i < allCols.Length; i++) {
			allCols[i].enabled = true;
		}
		hitPoints = startHitPoints;
		hit = false;
		deathAnimator.transform.parent = deathAnimatorParent;
		deathAnimator.transform.localPosition = deathAnimatorStartPosition;
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
		if(GameStateManager.instance.currentState != GameStateManager.State.Running)
			return;

		if(!hit){
			Move ();
		}
		HandleCooldown();
	}

	protected void HandleCooldown(){
		if (hit) {
			if (coolDownTimer > coolDownTime) {
				coolDownTimer = 0f;
				hit = false;
				for(int i = 0; i < allSprites.Count; i++){
					allSprites[i].color = new Color(allSprites[i].color.r, allSprites[i].color.g, allSprites[i].color.b, 1f);
				}
				return;
			}

			if(cooldownFadeTimer > cooldownFadeTime){
				faded = !faded;
				float alpha = 0.8f;
				if(faded){
					alpha = cooldownAlpha;
				}
				for(int i = 0; i < allSprites.Count; i++){
					if(allSprites[i].gameObject.activeSelf){
						allSprites[i].color = new Color(allSprites[i].color.r, allSprites[i].color.g, allSprites[i].color.b, alpha);
					}
				}
				cooldownFadeTimer = 0;
			}

			cooldownFadeTimer += Time.deltaTime;

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
		Vector3 dist = new Vector3 (trans.position.x - Kitty.instance.trans.position.x, trans.position.y - Kitty.instance.trans.position.y, 0);

		return dist;
	}

	protected virtual void Move(){
		transform.position += -GetVectorToKitty (transform).normalized * movementSpeed * Time.deltaTime;
		if(!EnemyManager.Instance.GetKnowEnemyTypes().Contains(gameObject.name)){
			for(int i = 0; i < allSprites.Count; i++){
				if(allSprites[i].isVisible){
					ShowTutorialForClass(gameObject.name);
					break;
				}
			}
		}
	}

	protected void ShowTutorialForClass(string enemyName){
		Debug.Log(enemyName);

		EnemyManager.Instance.EnemyFirstSeen(enemyName);
	}

	protected virtual void HitEnemy(){
		hitPoints--;
		hit = true;
		if(hitPoints > 0){
			faded = true;
			cooldownFadeTimer = 0f;
			for(int i = 0; i < allSprites.Count; i++){
				allSprites[i].color = new Color(allSprites[i].color.r, allSprites[i].color.g, allSprites[i].color.b, cooldownAlpha);
			}
		}
	}

	public virtual void DestroyEnemy(){
		for (int i = 0; i < allCols.Length; i++) {
			allCols[i].enabled = false;
		}

		if(deathAnimator != null){
			deathAnimator.transform.parent = null;
			deathAnimator.Play(deathAnimationName);
		} else{
			Debug.LogWarning("No death animator found!");
		}

		EnemyManager.Instance.EnemyIsDestroyed (this);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "PixelColliders" && !hit) {
			HitEnemy ();
			if (hitPoints <= 0) {
				GameStateManager.instance.ChangeScore();
				DestroyEnemy ();
			}
		} else if(col.gameObject.tag == "Kitty"){
			DestroyEnemy();
		}
	}
}
