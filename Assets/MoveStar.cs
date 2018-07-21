using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStar : MonoBehaviour {

	GameObject ScoreBoard;

	Animator ScoreTextAnimator;
	Animator ScoreAnimator;

	public float duration;
	public float rotationsPerMin;

	// Use this for initialization
	void Start () {
		ScoreTextAnimator = GameObject.Find ("Score").GetComponent<Animator>();
		ScoreAnimator =  GameObject.Find ("ScoreTV").GetComponent<Animator> ();
		ScoreBoard = GameObject.Find ("ScoreSpot");
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(0f,0f,6.0f*rotationsPerMin*Time.deltaTime);
		transform.position = Vector3.Slerp(transform.position, ScoreBoard.transform.position, Time.time / duration);
		if (Vector3.Distance(transform.position, ScoreBoard.transform.position) <3f) {
			ScoreAnimator.SetTrigger ("Scored");
			ScoreTextAnimator.SetTrigger ("Scored");
			Destroy (gameObject);
		}
	}
}
