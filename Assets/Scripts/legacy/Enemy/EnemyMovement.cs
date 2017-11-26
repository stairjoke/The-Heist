using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	public string targetTag = "Player";
	public float rerouteInterval = 3f;
	public float rotationSpeed = 1;
	public float closerStoppingDistance = 3f;

	private Transform enemyTarget;
	private float reroutingTimer = 0;
	private NavMeshAgent agent;


	// Use this for initialization
	void Start () {
		enemyTarget = GameObject.FindGameObjectWithTag (targetTag).transform;
		if (enemyTarget == null) {
			Application.Quit (); //Cannot operate without a target!
		}

		agent = this.GetComponent <NavMeshAgent>();
		if (agent.stoppingDistance == closerStoppingDistance) {
			closerStoppingDistance = agent.stoppingDistance / 2;
			Debug.LogError ("Agent stopping distance must be higher value than closer stopping distance.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (reroutingTimer <= Time.time) { //only reroute in time intervalls of X seconds

			//Only use navMesh while Player is out of reach
			if(Vector3.Distance(enemyTarget.position, transform.position) > agent.stoppingDistance){ //No >= because it stops too abruptly
				agent.enabled = true;
				agent.SetDestination (enemyTarget.position); //reroute
			}else{
				//Player is close

				//if can se no obstacle disable navigation
				if(!Physics.Linecast(transform.position, enemyTarget.transform.position, LayerMask.GetMask("StaticObjects"))){ //Player can be shot
					//Disable Navmesh to allow for manual transformation
					agent.enabled = false;

				}else if(agent.stoppingDistance != closerStoppingDistance){ //can see obstable and stopping distance is still large
					//move closer
					agent.enabled = true;
					agent.stoppingDistance = closerStoppingDistance;

				}else if(agent.stoppingDistance == closerStoppingDistance){ //obstable despite moving in closer
					agent.enabled = true;
					//if still unable to reach, move somewhere else
					agent.SetDestination(new Vector3(
						Random.value * (closerStoppingDistance+1) + enemyTarget.transform.position.x,
						enemyTarget.transform.position.y,
						Random.value * (closerStoppingDistance+1) + enemyTarget.transform.position.z
					));
				}
			}
			reroutingTimer = Time.time + rerouteInterval; //reset delay

			if(false == agent.enabled){
				//Enemy is not navigating

				//Keep facing the placer
				Vector3 direction = (enemyTarget.position - transform.position).normalized;
				Quaternion lookRotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
				transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			}
		}

	}
}
