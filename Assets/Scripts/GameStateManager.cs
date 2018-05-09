using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

	public static GameStateManager instance = null;

	enum State{
		Running,
		Paused,
		Ended
	}

	State currentState = State.Paused;

	public int Score { get; private set; }
	public float Time { get; private set; }
	public float DeltaTime { get; private set; }

	public delegate void ScoreChanged();
	public event ScoreChanged onScoreChanged;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		Time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentState == State.Running){
			Time += UnityEngine.Time.deltaTime;
			DeltaTime = UnityEngine.Time.deltaTime;
		} else{
			DeltaTime = 0;
		}
	}

	public void RunGame(){
		currentState = State.Running;
	}

	public void Pause(){
		currentState = State.Paused;
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
