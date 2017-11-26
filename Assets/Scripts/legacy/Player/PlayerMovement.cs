using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[Range(1f, 100f)]
	public float moveSpeed = 5f;

	private Vector3 walkingDirection;
	private Rigidbody myself;
	private float sprinting = 0;

	// Use this for initialization
	void Start () {
		myself = GetComponent<Rigidbody> ();
		walkingDirection = new Vector3 (0f,0f,0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Sprint")) {
			sprinting = 1.8f;
		} else {
			sprinting = 0;
		}
		if(Input.GetButton("Vertical") || Input.GetButton("Horizontal")){
			//Get Inputs
			walkingDirection.x = Input.GetAxisRaw("Horizontal") * moveSpeed * (1 + sprinting);
			walkingDirection.y = 0;
			walkingDirection.z = Input.GetAxisRaw("Vertical") * moveSpeed * (1 + sprinting);

			myself.AddForce (walkingDirection, ForceMode.Force);
		}
	}
}
