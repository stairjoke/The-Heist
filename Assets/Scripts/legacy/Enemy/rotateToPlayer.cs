using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateToPlayer : MonoBehaviour {

	public string playerTag = "Player";

	private Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag (playerTag).transform; //get player
	}
	
	// Update is called once per frame
	void Update () {
		//takes player position X,0,Z and object postition X,0,Z
		//calculates vector from object to player
		//rotates object to match calculated angle
		transform.rotation = Quaternion.LookRotation (new Vector3(player.position.x, 0, player.position.z) - new Vector3(transform.position.x, 0, transform.position.z), Vector3.up); 
	}
}
