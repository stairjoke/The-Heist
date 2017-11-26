using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public Transform projectile;
	// public AudioClip shootingSound;

	[Range(0.1f, 1f)]
	public float shootDelay = 0.1f;
	public AudioClip laserNoise;
	//float lastFired = Mathf.NegativeInfinity; continuous shooting

	private Ray userInputPosition;
	private RaycastHit clickPointHit;

	private Ray playerLaser;
	private RaycastHit playerLaserTarget;
	private LineRenderer laser;

	// Use this for initialization
	void Start () {
		laser = GetComponent<LineRenderer> ();
		laser.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")){

			StopCoroutine("FireLaser"); //failsafe just in case
			StartCoroutine("FireLaser");
		}
	}

	private IEnumerator FireLaser(){
		laser.enabled = true;

		while (Input.GetButton ("Fire1")) {
			AudioSource.PlayClipAtPoint (laserNoise, new Vector3(transform.position.x, Camera.main.transform.position.y -10, transform.position.z));
			userInputPosition = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (userInputPosition, out clickPointHit, 60f, LayerMask.GetMask ("Ground"))) {
				/*
			 * Get direction from player to clickPointHit
			 * Test for obstacles hit
			 * Set point as end of laser-beam
			 * Identify obstacle
			 * Options AB:
			 * A: Is a wall => nothing
			 * B: Is an enemy => harm enemy
			*/

				playerLaser = new Ray (transform.position + new Vector3 (0, 1, 0), clickPointHit.point - transform.position);

				if (Physics.Raycast (playerLaser, out playerLaserTarget, LayerMask.GetMask ("ReceivesPlayerShot"))) {
					//Hit something
					//Only draw the laser, when it hits something, because the assumption is the player is in a closed labyrinth and will always hit something

					//Draw laser
					laser.SetPosition(0, transform.position);
					laser.SetPosition (1, playerLaserTarget.point);

					if (playerLaserTarget.transform.name.Contains ("Enemy")) {
						//Hit an enemy
						playerLaserTarget.transform.SendMessage("hitByPlayerLaser", transform);
					} else if (playerLaserTarget.transform.name.Contains ("Wall")) {
						//Hit a wall
						//Could do stuff like an impact thing on the texture
					} else {
						//Hit something else
					}
				} else {
					
				}
			}
			yield return null;
		}
		laser.enabled = false;
	}
}
