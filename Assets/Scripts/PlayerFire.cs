﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour {

    public GameObject projectile;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void Launch(Vector3 force)
    {
        //GameObject proj = GameObject.Instantiate(projectile, transform.position, Quaternion.identity);

        GameObject proj = GameManager.Instance.InactivePellets.Dequeue();
        proj.transform.position = transform.position;
        proj.GetComponent<ProjectileInfo>().ToggleActive();
        proj.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
