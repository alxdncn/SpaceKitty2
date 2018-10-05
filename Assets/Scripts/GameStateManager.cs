using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

	public static GameStateManager instance = null;

	public enum State{
		Running,
		Paused,
		TimesUp,
		Ended
	}

	public State currentState = State.Running;

	public int Score { get; private set; }
	float time;
	[SerializeField] float roundTime = 60f;

	int level = 0;

	[SerializeField] float goToNextLevelTimer = 2f;
	[SerializeField] float gameOverTimer = 6f;
	float restartTime;
	float restartTimer = 0f;

	public float RoundProgress {get; private set;}

	public delegate void ScoreChanged();
	public event ScoreChanged onScoreChanged;

	public delegate void GameEnded();
	public event GameEnded lostGame;

	public delegate void BeatGame();
	public event BeatGame beatGame;

	[SerializeField] AnimationCurve gameProgressionCurve;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		time = 0;
		restartTimer = 0f;
		level = SceneManager.GetActiveScene().buildIndex;
		Score = DataBetweenScenes.totalScore;

		Cursor.visible = false;

		RunGame();
	}

	void Start(){
		if(onScoreChanged != null){
			onScoreChanged();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			LostGame();
			restartTimer = 0;
			SceneManager.LoadScene(level);
		}

		// Debug.Log(currentState + " " + RoundProgress);
		if(currentState == State.Running){
			time += UnityEngine.Time.deltaTime;
			RoundProgress = gameProgressionCurve.Evaluate(time/roundTime);
			if(RoundProgress >= 1f){
				currentState = State.TimesUp;
			}
		} 
		if(currentState == State.Ended){
			restartTimer += Time.deltaTime;

			if(restartTimer >= restartTime){
				restartTimer = 0;
				SceneManager.LoadScene(level);
			}
		}

		if(Input.GetKeyDown(KeyCode.T)){
			if(currentState == State.Paused){
				RunGame();
			} else {
				Pause();
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}

	public void RunGame(){
		currentState = State.Running;
		UnityEngine.Time.timeScale = 1f;
	}

	public void Pause(){
		currentState = State.Paused;
		// UnityEngine.Time.timeScale = 0f;
	}

	public void LostGame(){
		currentState = State.Ended;
		DataBetweenScenes.totalScore = 0;
		level = 0;
		DataBetweenScenes.level = 1;
		if(DataBetweenScenes.allKnownEnemies != null){
			DataBetweenScenes.allKnownEnemies.Clear();
		}
		DataBetweenScenes.kittyLives = 9;
		restartTime = gameOverTimer;
		if(lostGame != null){
			lostGame();
		}
	}

	public void WonGame(){
		Debug.Log("Beat Game, Level " + level.ToString());
		level = Mathf.Clamp(level + 1, 0, SceneManager.sceneCountInBuildSettings - 1);		
		Debug.Log("Going to Level " + level.ToString());
		DataBetweenScenes.totalScore = Score;
		DataBetweenScenes.level++;
		currentState = State.Ended;
		restartTime = goToNextLevelTimer;
		if(beatGame != null){
			beatGame();
		}
	}

	public void ChangeScore(bool reset = false){
		if(reset){
			Score = 0;
		} else{
			Score++;
		}
		if(onScoreChanged != null){
			onScoreChanged();
		}
	}
}
