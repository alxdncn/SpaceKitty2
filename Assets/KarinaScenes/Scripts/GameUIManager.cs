using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

	public Text scoreText;
	public Text timeText;
	ScoreScript scoreScript;


	// Use this for initialization
	void Start () {
		GameObject scoreManager = GameObject.Find("ScoreManager");
		scoreScript = scoreManager.GetComponent<ScoreScript>();	
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "SCORE: " + scoreScript.score;
		timeText.text = string.Format("{0}:{1:00}", (int)scoreScript.time / 60, (int)scoreScript.time % 60);
	}
}
