using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservePlayer : MonoBehaviour {

	public Transform player;

	// Use this for initialization
	void Start () {
		if (player == null) {
			Debug.Log ("Camera can't find player");
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (player.position.x, transform.position.y, player.position.z);
	}
}
