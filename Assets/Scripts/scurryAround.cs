using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scurryAround : MonoBehaviour {

	private Transform manager;
	private spawnGem mySpawnPoint;
	private NavMeshAgent agent;
	private float routeLastChanged = Mathf.NegativeInfinity;

	public void setManager(Transform _manager){
		manager = _manager;
	}

	public void setMySpawnPoint(spawnGem sP) {
		mySpawnPoint = sP;
	}

	// Use this for initialization
	void Start () {
		agent = transform.GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (routeLastChanged + 20f < Time.time && Random.value < 0.8f) {
			routeLastChanged = Time.time;
//!!! HARD CODED GAME SIZE!
			agent.SetDestination (new Vector3((Random.value -0.5f) * 40f, 1f, (Random.value -0.5f) * 40f));
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.name.Contains ("Player")) {
			mySpawnPoint.gemCollected();
			manager.GetComponent<score> ().playerHitMe (gameObject);
		}
	}
}
