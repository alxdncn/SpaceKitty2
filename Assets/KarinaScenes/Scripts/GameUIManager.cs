using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

	public Text scoreText;
	public Text timeText;

	// Use this for initialization
	void Start () {
		OnKittyHit();
	}

	void OnEnable(){
		GameStateManager.instance.onScoreChanged += OnScoreChanged;
		Kitty.instance.kittyHit += OnKittyHit;
	}

	void OnDisable(){
		GameStateManager.instance.onScoreChanged -= OnScoreChanged;
		Kitty.instance.kittyHit -= OnKittyHit;
	}
	
	void OnKittyHit(){
		timeText.text = Kitty.instance.lives.ToString();
	}

	void OnScoreChanged(){
		scoreText.text = "SCORE: " + GameStateManager.instance.Score;
	}
}
