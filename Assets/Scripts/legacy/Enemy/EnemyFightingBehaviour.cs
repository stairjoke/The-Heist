using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFightingBehaviour : MonoBehaviour {

	public Transform myShield;
	public Transform myAmmo;
	public Transform shotSpawn;
	public AudioClip shootingNoise;
	public AudioClip death;
	public Transform deathparticleBang;

	private float health = 100;
	public float healthReductionInPercentPerSecond = 40;
	private Transform healthCanvas;
	public Transform healthPanel;

	private float shieldHealth = 100;
	public float shieldReductionInPercentPerSecond = 35;
	private Transform shieldHealthPanel;

	public int shieldActiveFrames = 7;
	public float lookForPlayerIntervalInSeconds = 1;
	public string targetTag = "Player";

	private int shieldDelayFrames;
	private float lastLinecast = 0;
	private Transform player;

	// Use this for initialization
	void Start () {
		myShield.GetComponent<MeshRenderer> ().enabled = false; //shield is now invisible
		player = GameObject.FindGameObjectWithTag (targetTag).transform; //get player

		//draw my health on mom's canvas
		healthCanvas = transform.GetComponentInParent<EnemyManager> ().getCanvas();
		healthPanel = Instantiate (healthPanel, healthCanvas);
	}
	
	// Update is called once per frame
	void Update () {
		float currentTimeInSeconds = Time.time;

		//the shield is visible for shieldActiveFrames, this part counts down the frames left since last activation and hides it when it's done
		shieldDelayFrames--;
		if (shieldDelayFrames < 1 || shieldHealth < 1) {
			myShield.GetComponent<MeshRenderer> ().enabled = false;
		}

		//if last time player was seen by enemy is more than lookForPlayerIntervalInSeconds seconds ago, look for the player
		if (lastLinecast + lookForPlayerIntervalInSeconds < currentTimeInSeconds) {
			tryToShootAtPlayer ();
			lastLinecast = Time.time; //reset timer
		}

		healthPanelPosition ();
	}

	void tryToShootAtPlayer(){
		if(Vector3.Distance(transform.position, player.position) < 10){ //If Player is in sight

			var shot = Instantiate (myAmmo, shotSpawn.position, shotSpawn.rotation); //shoot at player (shot spawn rotates towards player every frame)
			shot.parent = this.transform; //parent this shot so it can be managed

			AudioSource.PlayClipAtPoint (shootingNoise, new Vector3(transform.position.x, Camera.main.transform.position.y - 10, transform.position.z));

		}
	}

	public void hitByPlayerLaser(Transform sender){
		if (sender.transform.tag.Contains ("Player")) {
			//If emeny is hit by the player

			if(shieldHealth > 0){ //count down shield health
				shieldHealth -= shieldReductionInPercentPerSecond * Time.deltaTime; //shield depleted X% per second
				myShield.GetComponent<MeshRenderer> ().enabled = true;
				shieldDelayFrames = shieldActiveFrames;
				//needs sound

			}else{ //no shield
				health -= healthReductionInPercentPerSecond * Time.deltaTime; //reduce enemy health X% per second

//needs damage animation

			}

			healthPanel.GetChild (0).GetComponent<RectTransform> ().localScale = new Vector3((shieldHealth / 100), 1, 1);
			healthPanel.GetChild (1).GetComponent<RectTransform> ().localScale = new Vector3((health / 100), 1, 1);
			if (health < 1) { //if I'm supposed to be dead
				var bang = Instantiate(deathparticleBang, transform.position, transform.rotation);
				AudioSource.PlayClipAtPoint (death, new Vector3 (transform.position.x, Camera.main.transform.position.y - 5, transform.position.z));
				transform.GetComponentInParent<EnemyManager> ().emenyKilled (bang); //tell my manager I'm going to kill myself and hand over the particle system for cleanup
				Destroy (healthPanel.gameObject);
				Destroy (transform.gameObject); //kill myself
			}
		}
	}

	private void healthPanelPosition(){
		healthPanel.position = Camera.main.WorldToScreenPoint (new Vector3(transform.position.x, transform.position.y + 0.544f, transform.position.z)); //0.544 = center of enemy body
	}
}
