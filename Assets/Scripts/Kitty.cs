using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitty : MonoBehaviour {

	public static Transform trans;
	[SerializeField] float coolDownTime = 1f;
	float coolDownTimer;

	bool hit = false;
	int lives = 9;

	// Use this for initialization
	void Awake () {
		trans = transform;
	}

	void Update(){
		if(hit){
			coolDownTimer += Time.deltaTime;
			if(coolDownTimer > coolDownTime){
				hit = false;
				coolDownTimer = 0;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(hit){
			return;
		}
		hit = true;
		coolDownTimer = 0;
		lives--;
		if(lives <= 0){
			//Game OVER!
		}
	}
}
