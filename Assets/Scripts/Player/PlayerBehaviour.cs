using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public float moveSpeed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        move();
	}

    private void move(){
        GetComponent<Rigidbody>().AddForce(UserInputAbstraction.userMotionVector() * moveSpeed);
    }
}
