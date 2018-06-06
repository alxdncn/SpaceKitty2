using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitty : MonoBehaviour {

	public static Kitty instance;
	public Transform trans;
	[SerializeField] float coolDownTime = 1f;
	float coolDownTimer;

	bool hit = false;
	public int lives = 9;

	SpriteRenderer[] allSprites;

	[SerializeField] protected float cooldownFadeTime = 0.2f;
	float cooldownFadeTimer;
	bool faded = false;

	[SerializeField] [Range(0,1)] float cooldownAlpha = 0.5f;

	public delegate void KittyHit();
	public KittyHit kittyHit;

	// Use this for initialization
	void Awake () {
		instance = this;
		trans = transform;
		allSprites = GetComponentsInChildren<SpriteRenderer>();
	}

	void Update(){
		HandleCooldown();
	}

	void HandleCooldown(){
		if (hit) {
			if (coolDownTimer > coolDownTime) {
				coolDownTimer = 0f;
				hit = false;
				for(int i = 0; i < allSprites.Length; i++){
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
				for(int i = 0; i < allSprites.Length; i++){
					allSprites[i].color = new Color(allSprites[i].color.r, allSprites[i].color.g, allSprites[i].color.b, alpha);
				}
				cooldownFadeTimer = 0;
			}

			cooldownFadeTimer += Time.deltaTime;

			coolDownTimer += Time.deltaTime;
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		Debug.Log(col.gameObject.name);

		if(hit){
			return;
		}
		if(col.gameObject.tag == "PixelColliders")
			return;

		hit = true;
		coolDownTimer = 0;
		// lives--;

		if(kittyHit != null){
			kittyHit();
		}

		if(lives <= 0){
			GameStateManager.instance.EndGame();
			//Game OVER!
		}
	}
}
