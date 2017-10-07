using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[Range(50.0f, 500.0f)]
	public float bulletSpeed = 170f;

	[Range(2f, 100f)]
	public float lifespan = 15;

	private float timeOfCreation;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().AddRelativeForce(new Vector3(0,0, bulletSpeed*10), ForceMode.Force);
		timeOfCreation = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeOfCreation + lifespan <= Time.time) {
			Destroy (transform.gameObject);
		}
	}
}
