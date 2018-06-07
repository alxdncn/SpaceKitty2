using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

	static GameUIManager instance;
	public static GameUIManager Instance{
		get{
			if (instance == null) {
				instance = FindObjectOfType<GameUIManager>();
			}
			return instance;
		}
		private set { instance = value; }
	}

	[SerializeField] Text scoreText;
	[SerializeField] Text timeText;

	[SerializeField] GameObject tutorialObject;
	[SerializeField] Text tutorialText;
	[SerializeField] Image tutorialImage;

	bool tutorialActive = false;

	[SerializeField] float tutorialDisplayTime;
	float tutorialDisplayTimer;

	void Awake(){
		Instance = this;
		tutorialObject.SetActive(false);
	}
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

	void Update(){
		if(tutorialActive){
			if(tutorialDisplayTimer <= 0){
				tutorialActive = false;
				tutorialObject.SetActive(false);
				GameStateManager.instance.RunGame();
			}

			tutorialDisplayTimer -= Time.deltaTime;
		}
	}
	
	void OnKittyHit(){
		timeText.text = "LIVES: " + Kitty.instance.lives.ToString();
	}

	void OnScoreChanged(){
		scoreText.text = "SCORE: " + GameStateManager.instance.Score;
	}

	public void SetTutorialText(string newText, Sprite newSprite){
		tutorialObject.SetActive(true);
		tutorialText.text = newText;
		tutorialImage.sprite = newSprite;
		tutorialDisplayTimer = tutorialDisplayTime;
		tutorialActive = true;
		GameStateManager.instance.Pause();
	}
}
