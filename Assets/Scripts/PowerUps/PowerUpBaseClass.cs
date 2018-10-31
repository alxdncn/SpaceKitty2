using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoodleStudio95;

[RequireComponent(typeof(Collider2D))]
public abstract class PowerUpBaseClass : MonoBehaviour {
	protected Collider2D col;

	[SerializeField] float getPowerUpTime = 2f;
	float getPowerUpTimer = 0f;

	int playerHits = 0;
	[SerializeField] int minPlayerHits = 2;

	Animator hitFeedbackAnimator;

	float introTimer = 0f;
	float introTime = 0;
	bool playingIntroAnim = true;

	float xSin;
	float ySin;

	SpriteRenderer rend;
	DoodleAnimator anim;
	
	[SerializeField] DoodleAnimationFile baseAnim;
	[SerializeField] DoodleAnimationFile activatingAnim;

	bool activating = false;

	protected virtual void Awake(){
		Debug.Log("In awake");
		col = GetComponent<Collider2D>();
		col.enabled = false;
		hitFeedbackAnimator = GetComponentInChildren<Animator>();
		anim = GetComponentInChildren<DoodleAnimator>();
		rend = GetComponentInChildren<SpriteRenderer>();

		if(anim != null){
			introTime =  anim.CurrentAnimationLengthInSeconds;
		} else{
			introTime = 0f;
		}
		Spawn();
	}

	public void Reset(){
		col.enabled = false;
		anim = GetComponentInChildren<DoodleAnimator>();
		if(anim != null){
			introTime =  anim.CurrentAnimationLengthInSeconds;
		} else{
			introTime = 0f;
		}
		playingIntroAnim = true;
		activating = false;
		Spawn();
	}

	void Spawn(){
		Vector3 randPos = Vector3.zero;
		do{
			float ySize = Camera.main.orthographicSize;
			float xSize = ySize * Camera.main.aspect;
			float topBuffer = ySize / 2;
			float bottomBuffer = 4;
			float sideBuffer = 4;

			xSize -= sideBuffer;

			randPos.x = Random.Range(-xSize, xSize);
			randPos.y = Random.Range(-ySize + bottomBuffer, ySize - topBuffer);
		} while(randPos.magnitude < 3.0f);

		transform.position = randPos;

	}

	void StartObjectAfterAnimation(){
		Debug.Log("In start animation");
		introTimer += Time.deltaTime;
		if(introTimer > introTime){
			col.enabled = true;
			playingIntroAnim = false;
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
		if(playingIntroAnim)
			StartObjectAfterAnimation();
		else
			Move();

		if(playerHits >= minPlayerHits){
			if(!activating){
				Debug.Log("Activating");
				activating = true;

				// anim.ChangeAnimation(activatingAnim);
			}
			getPowerUpTimer += Time.deltaTime;
			rend.color = Color.Lerp(Color.white, Color.red, Mathf.Clamp01(getPowerUpTimer/getPowerUpTime));
			if(getPowerUpTimer > getPowerUpTime){
				getPowerUpTimer = 0f;
				playerHits = 0;
				Debug.Log("HEYO WE GOT DAT POWERUP");
				PlayerGetsPowerUp();
			}
		} 
		else if(activating){
			activating = false;
			Debug.Log("Deactivating");
			rend.color = Color.white;

			// anim.ChangeAnimation(baseAnim);
		}
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
				Debug.Log("Reset powerup timer");
				getPowerUpTimer = 0;
			}
		}
	}

}
