using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveShieldToPlayer : MonoBehaviour {

	public float delayInSeconds = 10f;

	private float lastRecharge = Mathf.NegativeInfinity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(lastRecharge + delayInSeconds < Time.time){
			//it's ok to recharge now
		}else{
			//nope, you gotta wait
		}
	}

	void OnCollisionEnter(Collision you){
		if(you.gameObject.name.Contains("Player")){ //I only talk to the player!
			you.gameObject.GetComponent<PlayerHealthManager>().setPlayerShield(); //recharge by default amount set by player
		}
	}
}
