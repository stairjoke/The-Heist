using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	[Range(1, 100)]
	public int maxNoOfEnemiesInGame = 10;
	public Transform player;
	public Transform enemyPrefab;
	public Transform canvas;
	public float spawnDelay = 1f;

	private float lastSpawn = 0;
	private int currentNoOfEnemiesInGame = 0;

	// Use this for initialization
	void Start () {
		makeEnemy ();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentNoOfEnemiesInGame < maxNoOfEnemiesInGame && lastSpawn + spawnDelay + (Random.value * 5) < Time.time){
			makeEnemy ();
			lastSpawn = Time.time;
		}
	}

	public Transform getCanvas(){
		return canvas;
	}

	IEnumerator removeObjectInSeconds(Transform obj, float s){
		yield return new WaitForSeconds (s);
		Destroy (obj.gameObject);
	}

	public void emenyKilled(Transform particleSystemForCleanup){
		StartCoroutine (removeObjectInSeconds(particleSystemForCleanup, 1f));
		currentNoOfEnemiesInGame--;
	}

	public void makeEnemy(){
		/*
		//Find the closest Spawn point to the player

		Transform tMin = null; // Pretty much nothing
		float mDist = Mathf.Infinity; // Most certainly far away...
		float cDist = Mathf.Infinity; //Far away

		foreach(Transform t in transform){ //Iterate through all child objects
			if(t.name.Contains("spawn")){
				cDist = Vector3.Distance(player.position, t.position);
				if(cDist < mDist){
					mDist = cDist;
					tMin = t;
				}
			}
		}
		*/
		//Spawn Enemies

		Transform tRand;
		foreach (Transform t in transform) {
			if (t.name.Contains("spawn") && Random.value > 0.5f) {
				var newEnemy = Instantiate (enemyPrefab, t.position, t.rotation);
				newEnemy.transform.parent = this.transform;
				currentNoOfEnemiesInGame++;
				break;
			}
		}

		/*
		while (currentNoOfEnemiesInGame < maxNoOfEnemiesInGame) {
			var newEnemy = Instantiate (enemyPrefab, tMin.position, tMin.rotation);
			newEnemy.transform.parent = this.transform;
			currentNoOfEnemiesInGame++;
		}
		*/
	}
}
