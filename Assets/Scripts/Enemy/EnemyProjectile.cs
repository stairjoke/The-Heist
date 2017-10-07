using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

	public string playerTag = "Player";
	public float bulletSpeed = 100;

	private Transform player;
	private Vector3 aim;

	// Use this for initialization
	void Start () {
		aim = new Vector3(0,0,bulletSpeed);
		player = GameObject.FindGameObjectWithTag (playerTag).transform;
		Vector3.RotateTowards (aim, player.position, 3f, 3f);
		GetComponent<Rigidbody> ().AddRelativeForce(aim, ForceMode.Force);
	}

	void OnCollisionEnter(Collision other){
		Destroy (gameObject);
	}
}
