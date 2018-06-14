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

	[SerializeField] Text levelText;
	Text tutorialText;
	Image tutorialImage;

	bool tutorialActive = false;

	[SerializeField] float tutorialDisplayTime;
	float tutorialDisplayTimer;

	void Awake(){
		Instance = this;
		tutorialText = tutorialObject.GetComponentInChildren<Text>();
		tutorialImage = tutorialObject.transform.GetChild(1).GetComponent<Image>();
		tutorialObject.SetActive(false);
		levelText.text = "Level " + DataBetweenScenes.level;
	}
	// Use this for initialization
	void Start () {
		OnKittyHit();
		GameStateManager.instance.Pause();
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

	public void DoneShowingLevel(){
		GameStateManager.instance.RunGame();
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
		float xSize = tutorialImage.rectTransform.sizeDelta.y * newSprite.textureRect.width/newSprite.textureRect.height;
		tutorialImage.rectTransform.sizeDelta = new Vector2(xSize, tutorialImage.rectTransform.sizeDelta.y);
		tutorialDisplayTimer = tutorialDisplayTime;
		tutorialActive = true;
		GameStateManager.instance.Pause();
	}
}
