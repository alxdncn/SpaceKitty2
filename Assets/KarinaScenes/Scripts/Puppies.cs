using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppies : MonoBehaviour {

	public float speed;
	public Vector3 vel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		vel = Random.insideUnitSphere * speed;
		//vel.y = 0.0f;
		transform.Translate (vel * Time.deltaTime);
	}
}
