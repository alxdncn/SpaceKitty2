using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

	public static GameStateManager instance = null;

	enum State{
		Running,
		Paused,
		Ended
	}

	State currentState = State.Running;

	public int Score { get; private set; }
	float time;
	[SerializeField] float roundTime = 60f;

	public float RoundProgress {get; private set;}

	public delegate void ScoreChanged();
	public event ScoreChanged onScoreChanged;

	public delegate void GameEnded();
	public event GameEnded gameEnded;

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
		RunGame();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(currentState + " " + RoundProgress);

		if(currentState == State.Running){
			time += UnityEngine.Time.deltaTime;
			RoundProgress = gameProgressionCurve.Evaluate(time/roundTime);
			if(RoundProgress >= 1f && beatGame != null){
				currentState = State.Ended;
				beatGame();
			}
		} 
		if(currentState == State.Ended){
			if(Input.GetKeyDown(KeyCode.N)){
				SceneManager.LoadScene(1);
			}
			if(Input.GetKeyDown(KeyCode.Space)){
				SceneManager.LoadScene(0);
			}
		}
	}

	public void RunGame(){
		currentState = State.Running;
		UnityEngine.Time.timeScale = 1f;
	}

	public void Pause(){
		Debug.Log("Pausing Game");
		currentState = State.Paused;
		UnityEngine.Time.timeScale = 0f;
	}

	public void EndGame(){
		Debug.Log("Ending Game");
		currentState = State.Ended;
		UnityEngine.Time.timeScale = 0f;
		if(gameEnded != null){
			gameEnded();
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
