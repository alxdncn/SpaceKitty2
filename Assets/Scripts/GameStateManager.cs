using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

	public static GameStateManager instance = null;

	public enum State{
		Running,
		Paused,
		Ended
	}

	public State currentState = State.Running;

	public int Score { get; private set; }
	float time;
	[SerializeField] float roundTime = 60f;

	int level = 0;

	[SerializeField] float restartTime = 4;
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
		Score = DataBetweenScenes.totalScore;

		RunGame();
	}

	void Start(){
		if(onScoreChanged != null){
			onScoreChanged();
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log(currentState + " " + RoundProgress);
		if(currentState == State.Running){
			time += UnityEngine.Time.deltaTime;
			RoundProgress = gameProgressionCurve.Evaluate(time/roundTime);
			if(RoundProgress >= 1f){
				WonGame();
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
			Pause();
		}
	}

	public void RunGame(){
		currentState = State.Running;
		UnityEngine.Time.timeScale = 1f;
	}

	public void Pause(){
		Debug.Log("Pausing Game");
		currentState = State.Paused;
		// UnityEngine.Time.timeScale = 0f;
	}

	public void LostGame(){
		Debug.Log("Lost Game");
		currentState = State.Ended;
		DataBetweenScenes.totalScore = 0;
		level = 0;
		if(lostGame != null){
			lostGame();
		}
	}

	void WonGame(){
		Debug.Log("Beat Game, Level " + level.ToString());
		level += 1 % SceneManager.sceneCountInBuildSettings;
		Debug.Log("Going to Level " + level.ToString());
		DataBetweenScenes.totalScore = Score;
		currentState = State.Ended;
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
