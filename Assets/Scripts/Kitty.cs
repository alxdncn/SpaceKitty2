using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoodleStudio95;

public class Kitty : MonoBehaviour {

	public static Kitty instance;
	public Transform trans;
	[SerializeField] float coolDownTime = 1f;
	float coolDownTimer;

	bool hit = false;
	[HideInInspector] public bool canBeHitByPlayer = true;
	public int lives = 9;

	SpriteRenderer[] allSprites;

	[SerializeField] protected float cooldownFadeTime = 0.2f;
	float cooldownFadeTimer;
	bool faded = false;

	[SerializeField] [Range(0,1)] float cooldownAlpha = 0.5f;

	public delegate void KittyHit();
	public KittyHit kittyHit;

	//for audio
	AudioSource mySource;
	public AudioClip[] hurtMeows;
	public AudioClip[] finalMeow;

	//for anim
	DoodleAnimator animator;

	public DoodleAnimationFile hurtAnim;
	public DoodleAnimationFile dieAnim;
	public DoodleAnimationFile baseAnim;

	Animator LivesTextAnim;
	Animator LivesTVAnim;

	// Use this for initialization
	void Awake () {
		instance = this;
		trans = transform;
		LivesTextAnim = GameObject.Find ("Lives").GetComponent<Animator> ();
		LivesTVAnim = GameObject.Find ("LivesTV").GetComponent<Animator> ();
		animator = GetComponentInChildren<DoodleAnimator>();
		allSprites = GetComponentsInChildren<SpriteRenderer>();
		mySource = GetComponent<AudioSource> ();
		lives = DataBetweenScenes.kittyLives;
	}

	void OnEnable(){
		GameStateManager.instance.beatGame += SetStoredLives;
	}

	void OnDisable(){
		GameStateManager.instance.beatGame -= SetStoredLives;
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

	public void GetLife(){
		lives++;
		if(kittyHit != null){
			kittyHit();
		}
	}

	void OnDestroy(){
		DataBetweenScenes.kittyLives = lives;
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(hit){
			return;
		}
<<<<<<< HEAD
		// if(col.gameObject.tag == "PixelColliders")
		// 	return;
		if(GameStateManager.instance.currentState == GameStateManager.State.Running || 
		GameStateManager.instance.currentState == GameStateManager.State.TimesUp){
			hit = true;
			coolDownTimer = 0;
			lives--;

			if(kittyHit != null){
				PlayHurtMew ();
				kittyHit();
			}
=======
		if(col.gameObject.tag == "PixelColliders" && !canBeHitByPlayer)
			return;
>>>>>>> c8a39f40210d2527d065d44d6cc66dd5a3ca587d

			if(lives <= 0){
				lives = 9;
				PlayDeadMew ();
				GameStateManager.instance.LostGame();
			}
		}
	}

	protected virtual IEnumerator RestartBaseAnim (){
		yield return new WaitForSeconds(1f);
		if(lives > 0 && lives < 9){
			animator.ChangeAnimation (baseAnim);
			animator.Play ();
		
		} 
	}

	void PlayDeadMew(){
		int randMew = Random.Range (0, finalMeow.Length);
		mySource.clip = finalMeow [randMew];
		animator.ChangeAnimation (dieAnim);
		animator.Play ();
		mySource.Play ();
	}

	void PlayHurtMew(){
		int randMew = Random.Range (0, hurtMeows.Length);
		mySource.clip = hurtMeows [randMew];
		LivesTextAnim.SetTrigger ("LostLife");
		LivesTVAnim.SetTrigger ("LostLife");
		animator.ChangeAnimation (hurtAnim);
		animator.Play ();
		StartCoroutine (RestartBaseAnim ());
		mySource.Play ();
	}
}
