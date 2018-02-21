using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTest : MonoBehaviour {

	public Transform myHead;
	public static Transform myButt;

	public static bool tooBIG = false;

	public static Vector3 lastpos;
	public static List<GameObject> middles = new List<GameObject>();

	Transform Light;
	float distance;



	// Use this for initialization
	void Start () {
	//	Light = GameObject.FindGameObjectWithTag ("PixelCollider");
		middles.Add (Instantiate ((Resources.Load ("SnakeMiddle")) as GameObject, new Vector3 (1.8f, 1.2f, 0f), Quaternion.identity));
		lastpos = new Vector3 (myHead.transform.position.x, myHead.transform.position.y, myHead.transform.position.z);
		myButt = GameObject.Find ("SnakeButt").GetComponent<Transform>();
	}
		

	// Update is called once per frame
	void Update () {
		//CloseToLight ();
	}

	void OnTriggerEnter2D(Collider2D other){
		//replace this check with whatever Duncan is doing
		if(other.gameObject.tag == "PixelColliders"){
			if (middles.Count == 0) {
				RemoveWienie ();
			}
		}
	}

	void CloseToLight(){
		distance = Vector3.Distance (gameObject.transform.position, Light.position);
		Debug.Log (distance);
	}

	//run if ALL snake middles are gone and if the head or body is hit by light
	void RemoveWienie(){
		Destroy (gameObject);
	}

}
