using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthPuppies : MonoBehaviour {

	public static List<GameObject> puppies = new List<GameObject> ();
	public List<Vector3> puppyPos = new List<Vector3> ();
	//static GameObject aPup;

	public int pupCount; 

	// Use this for initialization
	void Start () {
		//aPup = Resources.Load ("Puppy") as GameObject;
		GiveBirth ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GiveBirth(){
		for (int i = 0; i < pupCount; i++) {
			puppyPos [i] = new Vector3 ((gameObject.transform.position.x+Random.Range (-1f, 1f)),(gameObject.transform.position.y+Random.Range (-1f, 1f)),0);
			puppies.Add (Instantiate ((Resources.Load ("Puppy head")) as GameObject, puppyPos[i], Quaternion.identity));
		}
	
	}

}
