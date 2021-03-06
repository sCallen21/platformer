﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public string attackName;
    public float damage;
    public float time;
    public float timeWhenHurts;         // the time in the animation when the hurtbox should be turned on
    public float timeWhenDoesntHurt;    // the time in the animation when the hurtbox should be turned off
    public Animator anim;
    public float knockbackMult = 1.0f;  // the amount of knockback the attack causes

    private bool attacking;
    private bool canHurt;
    private float timer;
    private AnimatorStateInfo animInfo;
    private AnimatorClipInfo[] animClip;

    EnemyInfo enemyInfo;

    public bool getAttacking() { return attacking; }
    public bool getCanHurt() { return canHurt; }
    public float getTimer() { return timer; }

    // Use this for initialization
    void Start () {
        attacking = false;
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        /* OLD CODE, USES A COROUTINE NOW
        if (attacking)
        {
            animInfo = anim.GetCurrentAnimatorStateInfo(0);
            animClip = anim.GetCurrentAnimatorClipInfo(0);

            timer = (animClip[0].clip.length / animInfo.speed) * animInfo.normalizedTime;

            if (animClip[0].clip.name == attackName)
                time = animClip[0].clip.length / animInfo.speed;

            if (timer >= timeWhenHurts / animInfo.speed && timer <= timeWhenDoesntHurt / animInfo.speed && animClip[0].clip.name == attackName)
            {
                canHurt = true;
            } else
            {
                canHurt = false;
            }

            if (timer >= time)
            {
                attacking = false;
            }
        }
        */
	}

    public void StartAttack()
    {
        attacking = true;
        timer = 0;
        canHurt = false;

        StartCoroutine("attack");
    }

    private IEnumerator attack()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        animClip = anim.GetCurrentAnimatorClipInfo(0);

        if (animClip[0].clip.name == attackName)
            time = animClip[0].clip.length / animInfo.speed;

        while (timer < time)
        {
            //timer = (animClip[0].clip.length / animInfo.speed) * animInfo.normalizedTime;
            timer += Time.deltaTime;

            if (timer >= timeWhenHurts / animInfo.speed && timer <= timeWhenDoesntHurt / animInfo.speed && animClip[0].clip.name == attackName)
            {
                canHurt = true;
            }
            else
            {
                canHurt = false;
            }

            yield return null;
        }
        attacking = false;
    }
}
