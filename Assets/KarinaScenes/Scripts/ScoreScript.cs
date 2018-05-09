using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour {


	public static ScoreScript instance = null;
	public int score = 0;
	public float time = 0f;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}
		
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
	}
}
