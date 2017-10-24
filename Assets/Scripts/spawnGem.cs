using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGem : MonoBehaviour {

	public Transform gemPrefab;
	public float spawnDelay = 15f;
	public int maxGems = 10;

	private int currentNoOfGems = 0;

	IEnumerator makeGem(){
		yield return new WaitForSeconds (Random.value * 1);
		var newGem = Instantiate (gemPrefab, transform);
		newGem.GetComponent<scurryAround> ().setManager (transform.parent.gameObject.transform);
		newGem.GetComponent<scurryAround> ().setMySpawnPoint(this);
	}

	// Update is called once per frame
	void Update () {
		if (currentNoOfGems < maxGems) {
			StartCoroutine (makeGem ());
			currentNoOfGems++;
		}
	}

	public void gemCollected() {
		currentNoOfGems--;
	}
}
