using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapBodies : MonoBehaviour {

	//This script should be on all snake middles

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		//replace this check with whatever duncan is doing
		if (other.gameObject.tag == "PixelColliders") {
			RemoveSnakeBodies ();
		}
	}

	//run this when the middle of snake body hits light
	void RemoveSnakeBodies(){
		SnakeTest.middles.Remove (gameObject);
	}
}
