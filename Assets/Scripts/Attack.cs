﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public Animator anim;
	public GameObject currentWep;

	private bool twoHand;

	private enum AttackState { 
		none = 0,
		lightOneHand = 1,
		thrustTwoHand = 2
	};

	private AttackState state = AttackState.none;
	private float attackTimer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (state == AttackState.none) {
			if (Input.GetAxis ("Attack") > 0) {
				state = AttackState.thrustTwoHand;
				PlayerManager.Instance.attacking = true;
				attackTimer = currentWep.GetComponent<WeaponInfo> ().swingTime;
				currentWep.SendMessage ("SetTrailTimer");
			}
		} else {
			attackTimer -= Time.deltaTime;
			if(attackTimer <= 0){
				attackTimer = 0;
				state = AttackState.none;
				PlayerManager.Instance.attacking = false;
			}
		}

		twoHand = currentWep.GetComponent<WeaponInfo> ().twoHand;

		int stateNum = (int)state;

		anim.SetInteger ("AttackState", stateNum);
		anim.SetBool ("TwoHand", twoHand);
	}
}