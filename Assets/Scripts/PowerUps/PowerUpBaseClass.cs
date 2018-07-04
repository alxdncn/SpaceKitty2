using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PowerUpBaseClass : MonoBehaviour {
	protected Collider2D col;

	[SerializeField] float getPowerUpTime = 2f;
	float getPowerUpTimer = 0f;

	int playerHits = 0;
	[SerializeField] int minPlayerHits = 2;

	Animator hitFeedbackAnimator;

	float introTimer = 0f;
	[SerializeField] AnimationClip introAnimation;
	float introTime = 0;

	float xSin;
	float ySin;

	protected virtual void Awake(){
		Debug.Log("In awake");
		col = GetComponent<Collider2D>();
		col.enabled = false;
		hitFeedbackAnimator = GetComponentInChildren<Animator>();
		Animator anim = GetComponent<Animator>();
		if(anim != null && introAnimation != null){
			anim.Play(introAnimation.name);
			introTime = introAnimation.length;
		}
		Spawn();
	}

	void Spawn(){
		float ySize = Camera.main.orthographicSize;
		float xSize = ySize * Camera.main.aspect;
		float topBuffer = ySize / 2;
		float bottomBuffer = 4;
		float sideBuffer = 4;

		xSize -= sideBuffer;

		float xPos = Random.Range(-xSize, xSize);
		float yPos = Random.Range(-ySize + bottomBuffer, ySize - topBuffer);

		transform.position = new Vector3(xPos, yPos, 0);

		Debug.Log(xSize);
	}

	void StartObjectAfterAnimation(){
		introTimer += Time.deltaTime;
		if(introTimer > introTime){
			col.enabled = true;
		}
	}

	protected virtual void Move(){
	}

	protected void PlayerGetsPowerUp(){
		PowerUpHandler.Instance.GotPowerup (this);
		DestroyPowerUp ();
	}

	protected virtual void DestroyPowerUp(){
		col.enabled = false;

		//DO ANIMATIONS

		PowerUpHandler.Instance.SetInactivePUObject (this, gameObject);
	}

	protected virtual void Update(){
		StartObjectAfterAnimation();
		if(playerHits >= minPlayerHits){
			getPowerUpTimer += Time.deltaTime;
			if(getPowerUpTimer > getPowerUpTime){
				getPowerUpTimer = 0f;
				playerHits = 0;
				PlayerGetsPowerUp();
			}
		}
		Move();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "PixelColliders") {
			playerHits++;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "PixelColliders") {
			playerHits--;
			if(playerHits < minPlayerHits){
				getPowerUpTimer = 0;
			}
		}
	}

}
