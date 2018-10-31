using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaghettiEnemy : EnemyBaseClass {

	[SerializeField] Transform childOne;
	Vector3 startPosOne;
	[SerializeField] Transform childTwo;
	Vector3 startPosTwo;
	LineRenderer spaghetti;

	bool childOneHit = false;
	bool childTwoHit = false;

	float timeSinceReset = 0f;

	protected override void Awake(){
		base.Awake();
		transform.position = Vector3.zero;
		spaghetti = GetComponent<LineRenderer>();
		allCols = GetComponentsInChildren<Collider2D>();
		SetPosition();
		SetLineRenderer();
	}

	public override void Reset(){
		base.Reset();
		transform.position = Vector3.zero;
		childOneHit = false;
		childTwoHit = false;
		spaghetti.enabled = true;
		SetPosition();
		SetLineRenderer();
	}

	void SetLineRenderer(){
		for(int i = 0; i < spaghetti.positionCount; i++){
			Vector3 pos = Vector3.Lerp(childOne.position, childTwo.position, (float)i / (float)(spaghetti.positionCount - 1));
			if(i != 0 && i != spaghetti.positionCount -1){
				float sinePosHolder = pos.x / (childTwo.position.x - childOne.position.x);
				pos.y += Mathf.Sin(sinePosHolder * 10f + Time.timeSinceLevelLoad * 10f);
			}
			spaghetti.SetPosition(i, pos);
		}
	}

	void SetPosition(){
		float randY = Random.Range(EnemyManager.Instance.minY, EnemyManager.Instance.maxY);
		childOne.localPosition = new Vector3(EnemyManager.Instance.minX, randY, 0);
		childTwo.localPosition = new Vector3(EnemyManager.Instance.maxX, -randY, 0);
		
		var dir = -childOne.position;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		childOne.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // this works because I've flipped it's x scale, I believe
		childTwo.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		startPosOne = childOne.localPosition;
		startPosTwo = childTwo.localPosition;
		Debug.Log(startPosOne);
		timeSinceReset = 0f;
	}

	protected override void Move(){
		//Note that using movement speed is weird, but I have the variable already
		//It means, though, that higher "speed" makes it move slower, should maybe change
		float howClose = Mathf.Clamp01(timeSinceReset / movementSpeed); 
		Debug.Log(howClose);
		timeSinceReset += Time.deltaTime;
		childOne.localPosition = Vector3.Lerp(startPosOne, Vector3.zero, howClose);
		childTwo.localPosition = Vector3.Lerp(startPosTwo, Vector3.zero, howClose);
		SetLineRenderer();
	}

	//Called from child's OnTriggerEnter2D
	public void ChildHit(Transform childTrans){
		if(childTrans == childOne){
			childOneHit = true;
			if(childTwoHit){
				HitEnemy ();
				PlayLazor ();
				if (hitPoints <= 0) {
					DestroyEnemy (true);
				}
			}
		} else if(childTrans == childTwo){
			childTwoHit = true;
			if(childOneHit){
				HitEnemy ();
				PlayLazor ();
				if (hitPoints <= 0) {
					DestroyEnemy (true);
				}
			}
		}
	}

	public override void DestroyEnemy(bool playerKilled){
		for (int i = 0; i < allCols.Length; i++) {
			allCols[i].enabled = false;
		}
		for(int i = 0; i < allSprites.Count; i++){
			allSprites[i].enabled = false;
		}
		spaghetti.enabled = false;

		if(playerKilled){
			GameStateManager.instance.ChangeScore();
			GameObject starPointOne = (GameObject)Instantiate(starPrefab, childOne.position, Quaternion.identity);
			GameObject starPointTwo = (GameObject)Instantiate(starPrefab, childTwo.position, Quaternion.identity);
		}

		float animTime = 0f;
		if(deathAnimator != null){
			deathAnimator.transform.parent = null;
			deathAnimator.Play(deathAnimation.name);
			animTime = deathAnimation.length;

		} else{
			Debug.LogWarning("No death animator found!");
		}
		StartCoroutine(WaitForDeathAnimationToEnd(animTime));
	}

	//Called from child's OnTriggerEnter2D
	public void ChildUnhit(Transform childTrans){
		if(childTrans == childOne){
			childOneHit = false;
		} else if(childTrans == childTwo){
			childTwoHit = false;
		}
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Kitty"){
			DestroyEnemy(false);
		}
	}
}
