﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour {


	[SerializeField] private float distanceToMove;
	[SerializeField] private float moveSpeed;
	private bool moveToPoint = false;
	private Vector3 endPosition;

	// Use this for initialization
	void Start () {
		endPosition = transform.position;

	}

	void FixedUpdate () {
		if (moveToPoint)
		{
			transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);
		}


	}
	
	// Update is called once per frame
	void Update () {
			if (Input.GetKeyDown(KeyCode.A)) //Left
			{
				endPosition = new Vector3(endPosition.x - distanceToMove, endPosition.y, endPosition.z);
				transform.rotation = Quaternion.Euler (0, 0, 0);
				Vector3 aDir = new Vector3 (0, 0, 0);
				MoveSumSnake (aDir);
				SnakeTest.lastpos = endPosition;
				moveToPoint = true;
			}
			if (Input.GetKeyDown(KeyCode.D)) //Right
			{
				endPosition = new Vector3(endPosition.x + distanceToMove, endPosition.y, endPosition.z);
				transform.rotation = Quaternion.Euler (0, 0, -180);
				Vector3 aDir = new Vector3 (0, 0, -180);
				MoveSumSnake (aDir);
				SnakeTest.lastpos = endPosition;
				moveToPoint = true;
			}
			if (Input.GetKeyDown(KeyCode.W)) //Up
			{
				endPosition = new Vector3(endPosition.x, endPosition.y + distanceToMove, endPosition.z);
				transform.rotation = Quaternion.Euler (0, 0, -90);
				Vector3 aDir = new Vector3 (0, 0, -90);
				MoveSumSnake (aDir);
				SnakeTest.lastpos = endPosition;

				moveToPoint = true;
			}
			if (Input.GetKeyDown (KeyCode.S)) { //Down
				endPosition = new Vector3 (endPosition.x, endPosition.y - distanceToMove, endPosition.z);
				transform.rotation = Quaternion.Euler (0, 0, -260);
				Vector3 aDir = new Vector3 (0, 0, -260);
				MoveSumSnake (aDir);
				SnakeTest.lastpos = endPosition;
				moveToPoint = true;
			}

	}

	void MoveSumSnake(Vector3 dir){
		SnakeTest.myButt.transform.position = SnakeTest.middles [SnakeTest.middles.Count - 1].transform.position;
		Vector3 newDir = SnakeTest.middles [SnakeTest.middles.Count - 1].transform.rotation.eulerAngles;

		SnakeTest.myButt.transform.rotation = Quaternion.Euler (newDir.x, newDir.y, newDir.z);
		for (int i = SnakeTest.middles.Count; i >= 0; i--) {
			if (i == 0) {
				SnakeTest.middles [i].transform.position = SnakeTest.lastpos;
				SnakeTest.middles [i].transform.rotation = Quaternion.Euler(dir.x, dir.y, dir.z);
			} else if(i == SnakeTest.middles.Count){
				CreateSumSnake(SnakeTest.middles [i-1].transform.position);
			}else {
				SnakeTest.middles [i].transform.position = SnakeTest.middles [i-1].transform.position;
				newDir = SnakeTest.middles [i - 1].transform.rotation.eulerAngles;
				SnakeTest.middles [i].transform.rotation = Quaternion.Euler (newDir.x, newDir.y, newDir.z);
			}
		}
	}

	void CreateSumSnake (Vector3 pos){
		if (!SnakeTest.tooBIG) {
			SnakeTest.middles.Add (Instantiate ((Resources.Load ("SnakeMiddle")) as GameObject, new Vector3 (pos.x, pos.y, pos.z), Quaternion.identity));
			if (SnakeTest.middles.Count >= 6) {
				SnakeTest.tooBIG = true;
			}
		}
	}
}
