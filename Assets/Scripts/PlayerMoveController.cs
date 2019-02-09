﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour {

    public float moveSpeed = 10.0f;
    public float jumpForce = 25.0f;
    public float baseGravMult = 0.25f;
	public float upGravMult = 0.3f;
	public float downGravMult = 1.0f;
	private float currentGravMult = 1.0f;
	public float turnSpeed = 25.0f;
    public CharacterController controller;
	public Animator anim;
	public Transform waist;
	public Transform pivot;
	public GameObject model;
	public bool grounded;
    
	private Vector3 moveDirection;
	private float terminalVelocity = -25.0f;

    private float hurtTimer;


	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        checkKnockbackTimer();
		ApplyGravity ();
    	Move();
	}

    void Move()
	{
		CheckGround ();

		float prevY = moveDirection.y;
        if (MovementManager.Instance.canMove)
        {
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            if (GameManager.Instance.inputModeKeyboard)
                moveDirection.Normalize();
            moveDirection *= moveSpeed;
        }
		moveDirection.y = prevY;
		//the way you're looking before canMove is factored in
		Vector3 lookDirection = moveDirection;

		//jumps
		if (grounded) {
			moveDirection.y = 0f;
			if (Input.GetButtonDown ("Jump")) {
				moveDirection.y = jumpForce;
			}
		}

		//this code handles interpolating towards desired forward. Only cosmetic for now. xzMove is moveDirection except y is set to 0
		Vector3 xzMove = new Vector3 (moveDirection.x, 0f, moveDirection.z);
		float step = turnSpeed * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);

		//moves player in direction based on camera orientation
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
		{
			transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
			Quaternion newRot = Quaternion.LookRotation(new Vector3(lookDirection.x, 0f, lookDirection.z));
			model.transform.rotation = Quaternion.Slerp (model.transform.rotation, newRot, step);
		}

		//put animation variable connections here
		anim.SetFloat("Speed", (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z)));
		anim.SetFloat ("YSpeed", controller.velocity.y);
    }

	void CheckGround(){
		int layerMask = 1 << 8;
		if (Physics.Raycast (transform.position, Vector3.down, (controller.height / 2f) + 0.25f, layerMask, QueryTriggerInteraction.Collide))
			grounded = true;
		else
			grounded = false;
	}

	void ApplyGravity(){
		if (moveDirection.y > 0)
			currentGravMult = baseGravMult * upGravMult;
		else if (moveDirection.y < 0)
			currentGravMult = baseGravMult * downGravMult;
		else
			currentGravMult = baseGravMult;
		
		moveDirection.y += (Physics.gravity.y * currentGravMult);

		if (moveDirection.y <= terminalVelocity){
			moveDirection.y = terminalVelocity;
		}
	}

    //initiates knocking the player backwards towards a specific direction
    void PlayerKnockback(object[] array)
    {
        hurtTimer = (float)array[1];
        Vector3 direction = (Vector3)array[0];
        Vector3 directionXZ = new Vector3(direction.x, 0.0f, direction.z);
        transform.forward = -directionXZ;
        moveDirection = direction;
        MovementManager.Instance.canMove = false;
    }

    void checkKnockbackTimer()
    {
        if(hurtTimer > 0)
            hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0)
            MovementManager.Instance.canMove = true;
    }

    //will stop the player in their tracks. Combine this with MovementManager.Instance.canMove = false to fully stop player
    void PlayerStopMovement()
    {
        moveDirection.x = 0;
        moveDirection.z = 0;
    }

}
