using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {

	public Transform enemy;
	public int numberOfEnemiesToSpawn = 1;
	[Range(0.5f, 15f)]
	public float respawnDelayInSeconds = 2;
	public string myTargetTag = "Player";
	[Range(10, 45)]
	public int activationDistance = 20;

	private Object[] myEnemy;
	private float lastSpawn;

	// Use this for initialization
	void Start () {
		myEnemy = new Object[numberOfEnemiesToSpawn];
		lastSpawn = Time.time;
	}

	private void makeEnemy(){
		if(Time.time > lastSpawn + respawnDelayInSeconds){
			for(int i = 0; i < numberOfEnemiesToSpawn; i++){
				if(myEnemy[i] == null){
					myEnemy[i] = Instantiate (enemy, transform.position, transform.rotation);
					lastSpawn = Time.time;
					return;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, GameObject.FindWithTag(myTargetTag).transform.position) < activationDistance){
			makeEnemy ();
		}
	}
}
