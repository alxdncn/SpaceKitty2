using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

	float playPoint = 3f;
	public AudioClip bark;
	public AudioSource mySource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		playPoint -= 0.1f;
		if (playPoint <= 0) {
			mySource.PlayOneShot (bark);
			playPoint = Random.Range (3f, 10f);
		}
	}

}
