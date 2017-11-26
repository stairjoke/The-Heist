using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour {

	public const float maximumHealthUnits = 100; //Used as scaling factor for graphics
	public const float playerInitialHealth = 100;
	public float regenDelayInSeconds = 3;
	public float regenUnitsPerSecond = 5;
	public const float playerShieldDefaultStrength = 100;
	[Range(1f,99f)]
	public const float defaultHarm = 10f;
	public Transform playerHealthCanvaspanel;
	public Color healthBarFlashColor;
	public Color healthBarLowColor;
	public Transform playerShield;
	public Transform playerShieldHealthCanvaspanel;
	public Color shieldHealthFlashColor;
	public Color shieldhealthLowColor;
	public Canvas finishPanel;
	public Transform finishPaneltextbox;
	public Transform gemsManager;

	private Color healthBarBaseColor;
	private Color shieldHealthBaseColor;
	private bool playerShielded = true;
	private float playerShieldHealth;
	private float myHealth;
	private float lastHit = Mathf.Infinity;

	void Start(){
		//read
		healthBarBaseColor = playerHealthCanvaspanel.GetComponent<Image> ().color;
		shieldHealthBaseColor = playerShieldHealthCanvaspanel.GetComponent<Image> ().color;

		//write
		setPlayerHealth ();
		setPlayerShield ();
	}

	void Update(){
		if ((lastHit + regenDelayInSeconds < Time.time || playerShielded) && myHealth < maximumHealthUnits) { //if player wasn't hit in a while and has damage
			setPlayerHealth(myHealth + regenUnitsPerSecond * Time.deltaTime);
		}
	}

	public float getPlayerShieldHealth(){
		return playerShieldHealth;
	}
	public float setPlayerShield(float newShieldHealth = playerShieldDefaultStrength){
		playerShieldHealth = newShieldHealth;

		if (playerShieldHealth > 0) {
			playerShielded = true;
			playerHealthCanvaspanel.localScale = new Vector3 (playerHealthCanvaspanel.localScale.x, 1f, playerHealthCanvaspanel.localScale.z); //make room for shield strength
			playerShieldHealthCanvaspanel.GetComponent<Image>().enabled = true;
		}else{
			playerShielded = false;
			playerHealthCanvaspanel.localScale = new Vector3 (playerHealthCanvaspanel.localScale.x, 2.47f, playerHealthCanvaspanel.localScale.z); //grow to cover shield strength
			playerShieldHealthCanvaspanel.GetComponent<Image>().enabled = false;
		}

		//Display shield health
		playerShieldHealthCanvaspanel.localScale = new Vector3 (playerShieldHealth / maximumHealthUnits, playerShieldHealthCanvaspanel.localScale.y, playerShieldHealthCanvaspanel.localScale.z);
		return playerShieldHealth;
	}

	public float getPlayerHealth(){
		return myHealth;
	}
	private float setPlayerHealth(float newHealth = playerInitialHealth){
		myHealth = newHealth;
		if (myHealth < playerInitialHealth / 3) {
			playerHealthCanvaspanel.GetComponent<Image> ().color = healthBarLowColor;
		} else {
			playerHealthCanvaspanel.GetComponent<Image> ().color = healthBarBaseColor;
		}
		if (myHealth <= 0) {
			//Player is dead
			playerDied();
		}
		playerHealthCanvaspanel.localScale = new Vector3 (myHealth / maximumHealthUnits, playerHealthCanvaspanel.localScale.y, playerHealthCanvaspanel.localScale.z);
		return myHealth;
	}

	void OnCollisionEnter(Collision other){
		if (other.transform.tag.Contains ("projectile") && other.transform.parent.name.Contains ("Enemy")) { //if collision with projectile from enemy
			playerHarmed (defaultHarm); //take harm
		}
	}

	private void playerHarmed(float damage){
		if(!playerShielded){
			//No Shield
			lastHit = Time.time;
			setPlayerHealth (getPlayerHealth() - damage);
		}else{
			//Yes Shield!
			float remainingDamage = Mathf.Abs(setPlayerShield (playerShieldHealth - damage));
			if (remainingDamage < 0) { //more damage than the shield can take
				setPlayerHealth(myHealth - remainingDamage);
				setPlayerShield (0f);
			}
		}
	}

	public void playerDied(){
		finishPanel.GetComponent<Canvas> ().enabled = true;
		string gems = gemsManager.GetComponent<score> ().getScore ().ToString ();
		finishPaneltextbox.GetComponent<Text>().text = "Gems collected: " + gems;
		Time.timeScale = 0;
	}
}
