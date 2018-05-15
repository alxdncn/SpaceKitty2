using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : EnemyBaseClass {

	[SerializeField] private float distanceToMove;
	private Vector3 endPosition;

	int dirNum = 0;
	int moveCount = 0;

	[Tooltip("Should be one of the following values: 4, 6, 8, 10, 12, 14")]
	[SerializeField] [Range(4, 14)] int directionRandomRange = 10;
	[SerializeField] int maxMoves = 6;
	[SerializeField] int minMoves = 2;

	Transform myHead;
	Transform myButt;

	List<GameObject> middles = new List<GameObject>();
	List<GameObject> inactiveMiddles = new List<GameObject>();

	int middleCount = 6;

	[SerializeField] float moveTime = 0.3f;
	float moveTimer = 0f;

//	[SerializeField] new float coolDownTime;

	enum Directions {
		Up, 
		Down,
		Left,
		Right
	}

	Directions direction;

	protected override void Awake(){
		base.Awake ();
		startHitPoints = middleCount;
		hitPoints = startHitPoints;

		myButt = transform.Find ("SnakeButt");
		myHead = transform.Find ("SnakeHead");
		allCols = GetComponentsInChildren<BoxCollider2D> ();

		endPosition = transform.position;

		for (int i = 0; i < middleCount; i++) {
			CreateSnakeMiddle (endPosition, true);
		}

	}

	public override void Reset(){
		base.Reset ();

		endPosition = transform.position;

		int inactiveMiddleCount = inactiveMiddles.Count;

		for (int i = 0; i < inactiveMiddleCount; i++) {
			CreateSnakeMiddle (endPosition, false);
		}

	}

	protected override void Update(){
		if (moveTimer > moveTime) {
			Move ();
			moveTimer = 0;
		}
			
		moveTimer += Time.deltaTime;

		if (hit) {
			if (coolDownTimer > coolDownTime) {
				coolDownTimer = 0f;
				hit = false;
			}

			coolDownTimer += Time.deltaTime;
		}
	}

	void MoveSumSnake(Vector3 dir){
		Vector3 newDir;
		if (middles.Count > 0) {
			myButt.transform.position = middles [middles.Count - 1].transform.position;
			newDir = middles [middles.Count - 1].transform.eulerAngles;
		} else {
			myButt.transform.position = endPosition;
			newDir = myHead.eulerAngles;
		}

		myButt.transform.rotation = Quaternion.Euler (newDir.x, newDir.y, newDir.z);
		for (int i = middles.Count - 1; i >= 0; i--) {
			if (i == 0) {
				middles [i].transform.position = endPosition;
				middles [i].transform.rotation = Quaternion.Euler(dir.x, dir.y, dir.z);
			} else {
				middles [i].transform.position = middles [i-1].transform.position;
				newDir = middles [i - 1].transform.rotation.eulerAngles;
				middles [i].transform.rotation = Quaternion.Euler (newDir.x, newDir.y, newDir.z);
			}
		}
	}

	protected override void Move ()
	{
		if (direction == Directions.Left) //Left
		{
			TakeStep(0, new Vector2(-distanceToMove, 0));
		}
		else if (direction == Directions.Right) //Right
		{
			TakeStep(-180, new Vector2(distanceToMove, 0));
		}
		else if (direction == Directions.Up) //Up
		{
			TakeStep(-90, new Vector2(0, distanceToMove));
		}
		else if (direction == Directions.Down) { //Down
			TakeStep(-270, new Vector2(0, -distanceToMove));
		}
		if (moveCount <= 0) {
			SetMove ();
		}
	}

	void TakeStep(float zAngle, Vector2 posChange){
		Vector3 aDir = new Vector3 (0, 0, zAngle);
		MoveSumSnake (aDir);
		endPosition = new Vector3 (endPosition.x + posChange.x, endPosition.y + posChange.y, endPosition.z);
		myHead.position = endPosition;
		myHead.rotation = Quaternion.Euler (0, 0, zAngle);
		moveCount--;
	}

	void SetMove(){
		moveCount = Random.Range (minMoves, maxMoves);
		dirNum = Random.Range (0, directionRandomRange);

		bool aboveKitty = true;
		bool rightOfKitty = true;

		Vector2 distFromKitty = GetVectorToKitty (myHead);

		if(distFromKitty.y < 0){
			aboveKitty = false;
		}
		if (distFromKitty.x < 0){
			rightOfKitty = false;
		}

		if (aboveKitty) {
			if (rightOfKitty) {
				if (dirNum < Mathf.RoundToInt((directionRandomRange - 2) / 2)) {
					direction = Directions.Down;
				} else if (dirNum < directionRandomRange - 2) {
					direction = Directions.Left;
				} else if (dirNum == directionRandomRange - 2) {
					direction = Directions.Up;
				} else if (dirNum == directionRandomRange - 1) {
					direction = Directions.Right;
				}
			} else {
				if (dirNum < Mathf.RoundToInt((directionRandomRange - 2) / 2)) {
					direction = Directions.Down;
				} else if (dirNum < directionRandomRange - 2) {
					direction = Directions.Right;
				} else if (dirNum == directionRandomRange - 2) {
					direction = Directions.Up;
				} else if (dirNum == directionRandomRange - 1) {
					direction = Directions.Left;
				}
			}
		} else {
			if (rightOfKitty) {
				if (dirNum < Mathf.RoundToInt((directionRandomRange - 2) / 2)) {
					direction = Directions.Up;
				} else if (dirNum < directionRandomRange - 2) {
					direction = Directions.Left;
				} else if (dirNum == directionRandomRange - 2) {
					direction = Directions.Down;
				} else if (dirNum == directionRandomRange - 1) {
					direction = Directions.Right;
				}
			} else {
				if (dirNum < Mathf.RoundToInt((directionRandomRange - 2) / 2)) {
					direction = Directions.Up;
				} else if (dirNum < directionRandomRange - 2) {
					direction = Directions.Right;
				} else if (dirNum == directionRandomRange - 2) {
					direction = Directions.Down;
				} else if (dirNum == directionRandomRange - 1) {
					direction = Directions.Left;
				}
			}
		}

	}

	//add snek middle as long as the snek is not TOO BIG
	void CreateSnakeMiddle (Vector3 pos, bool init){
		if (init) {
			GameObject newMiddle = Instantiate ((Resources.Load ("SnakeMiddle")) as GameObject);
			newMiddle.transform.parent = this.transform;
			newMiddle.transform.localPosition = new Vector3 (0f, 0f, 0f);

			middles.Add (newMiddle);
		} else {
			GameObject newMiddle = inactiveMiddles [0];
			inactiveMiddles.RemoveAt (0);
			middles.Add (newMiddle);
			newMiddle.SetActive (true);
		}
	}

	protected override void HitEnemy ()
	{
		base.HitEnemy ();
		GameObject killMiddle = middles [middles.Count - 1];
		inactiveMiddles.Add (killMiddle);
		middles.Remove (killMiddle);
		killMiddle.SetActive (false);
	}
}
