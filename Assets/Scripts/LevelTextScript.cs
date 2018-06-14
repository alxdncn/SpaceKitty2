using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTextScript : MonoBehaviour {

	void EndAnimation(){
		GameUIManager.Instance.DoneShowingLevel();
	}
}
