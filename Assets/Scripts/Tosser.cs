﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tosser : MonoBehaviour {

    public GameObject projectile;
    private GameObject newProjectile;

    public float timeTilThrow;
    public Transform hand;
    public float offset;
    public float timeTilPickup; //the time it takes the animation to pick up the projectile
    public GameObject target; //the target of the toss
    public float theta; //angle of toss

    public float forceMult;

    private float throwTime;

    public List<GameObject> allProjectiles;

    // Use this for initialization
    void Start () {
        throwTime = timeTilThrow;
        allProjectiles = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateProjectile()
    {
        newProjectile = Instantiate(projectile);
        allProjectiles.Add(newProjectile);
        StartCoroutine("holdProjectile");
    }

    void TossProjectile(Vector3 force)
    {
        
    }

    private IEnumerator holdProjectile()
    {
        if (newProjectile != null)
        {
            timeTilThrow = throwTime;

            newProjectile.transform.position = transform.position + transform.forward;
            newProjectile.GetComponent<ProjectileInfo>().origin = gameObject;

            while (timeTilThrow > 0)
            {
                if (timeTilThrow <= timeTilPickup)
                    newProjectile.transform.position = hand.position + -hand.up * offset;
                timeTilThrow -= Time.deltaTime;
                yield return null;
            }


            Vector3 force = FindForce();
            newProjectile.GetComponent<RockFly>().Launch(force);
        }
    }

    private Vector3 FindForce()
    {
        float degToRad = Mathf.PI / 180.0f;
        Vector3 direction = target.transform.position - transform.position;

        float heightOffset = direction.y;

        float heightForceMult = Mathf.Pow(1.05f, heightOffset);

        direction.y = 0.0f;

        float magnitude = Mathf.Sqrt(direction.magnitude * Mathf.Abs(Physics.gravity.y) / Mathf.Sin(2 * theta * degToRad));

        direction.Normalize();

        Vector3 rotAxis = Vector3.Cross(direction, Vector3.up);
        Quaternion rot = Quaternion.AngleAxis(theta, rotAxis);

        direction = rot * direction;
        Vector3 finalDir = direction * magnitude * heightForceMult;

        return finalDir;
    }

    //for use when enemy dies. Will destroy all projectiles it threw
    private void DestroyAllProjectiles() { 
        foreach(GameObject proj in allProjectiles)
        {
            proj.GetComponent<ProjectileBreak>().StartBreak();
        }
    }
}


