using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

	public Text scoreText;
	public Text timeText;

	// Use this for initialization
	void Start () {
	}

	void OnEnable(){
		GameStateManager.instance.onScoreChanged += OnScoreChanged;
	}

	void OnDisable(){
		GameStateManager.instance.onScoreChanged -= OnScoreChanged;
	}
	
	// Update is called once per frame
	void Update () {
		
		timeText.text = string.Format("{0}:{1:00}", (int)GameStateManager.instance.Time / 60, (int)GameStateManager.instance.Time % 60);
	}

	void OnScoreChanged(){
		scoreText.text = "SCORE: " + GameStateManager.instance.Score;
	}
}
