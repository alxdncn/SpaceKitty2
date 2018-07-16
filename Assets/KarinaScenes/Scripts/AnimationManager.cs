using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoodleStudio95;


[RequireComponent(typeof(DoodleAnimator))]


public class AnimationManager : MonoBehaviour {

	public StateIds currentState;

	[SerializeField]
	public enum StateIds{
		Basic,
		Intersitial,
		KittyDeath,
		PlayerDeath,
	}

	DoodleAnimator animator;

	public DoodleAnimationFile basicAnim;
	public DoodleAnimationFile interAnim;
	public DoodleAnimationFile kittyDeathAnim;
	public DoodleAnimationFile playerDeathAnim;

	void Awake(){
		animator = gameObject.GetComponent<DoodleAnimator>();
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	void SwitchStates(){
		switch(currentState){
		case StateIds.Basic:
				PlayAnim (basicAnim);
				break;
			case StateIds.Intersitial:
				PlayAnim (interAnim);
				break;
			case StateIds.KittyDeath:
				PlayAnim (kittyDeathAnim);
				break;
			case StateIds.PlayerDeath:
				PlayAnim (playerDeathAnim);
				break;
		}
	}

	//hierarchy of animations
	//some animations can interupt and others cannot

	//Play whatever animation

	void PlayAnim(DoodleAnimationFile toPlay){
		animator.ChangeAnimation (toPlay);
		animator.Play ();
	}


}
