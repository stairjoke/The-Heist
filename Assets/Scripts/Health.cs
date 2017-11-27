using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {

    /*
     * Class keeps track of game objects health and shield-health
     * This class does not manage collisions etc. GameObjects
     * have to communicate health and shield changes
    */

    public bool regenerateWhileFightingWithShield = true;
    public bool regenerateWhileFightingNoShield = false;
    public float autoRegenerateDelayInSeconds = 0.5f;
    public Transform destroyEffect; //optional
    private float healthStatus = 1f; //ranges from 0...1 = percentage

    //Health portion of class
    public float getHealth(){
        return healthStatus;
    }
    private IEnumerator selfDestructAtEndOfFrame(){
        /*
         * Delayes objects destruction to allow frame to calculate collisions etc. corretly
        */
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
    public float changeHealth(float difference, bool autoDestroy = true){
        /*
         * Public method to be called when GameObject's health is impacted.
         * Calculates new health and schedules object to be destroyed
         * returns health
        */
        healthStatus += difference;
        healthStatus = Mathf.Clamp(shieldStatus, 0f, 1f);
        if(autoDestroy && destroyEffect != null){
            Instantiate(destroyEffect, this.transform.position, this.transform.rotation);
            StartCoroutine(selfDestructAtEndOfFrame());
        }
        return getHealth();
    }

    //Shield portion of class
    public bool hasShield = false;
    public Transform shieldTransform; //optional
    private float shieldStatus = 1f;
    private Transform shieldInstance;

    public float getShieldStatus(){
        return (hasShield) ? shieldStatus : 0f;
    }
    private void disableShield(){
        hasShield = false;
        if(shieldInstance != null){
            Destroy(shieldInstance.gameObject);
            shieldInstance = null;
        }
    }
    private void enableShield(){
        hasShield = true;
        shieldStatus = 1f; //make sure shield is charged
        if(shieldInstance == null){
            shieldInstance = Instantiate(shieldTransform, this.transform.position, this.transform.rotation, this.transform);
        }
    }
    public float changeShieldStatus(bool b){
        /*
         * Public method to be called to enable or disable shield
         * returns shield-status
        */
        if(b){
            enableShield();
        }else{
            disableShield();
        }
        return getShieldStatus();
    }
    public float changeShieldStatus(float difference){
        /*
         * Public method to be called when GameObject's shield-health is impacted.
         * Calculates new shield-health and disables shield if shield-health too low
         * returns shield-status
        */
        shieldStatus += difference;
        shieldStatus = Mathf.Clamp(shieldStatus, 0f, 1f);
        if (shieldStatus.Equals(0f)){
            disableShield();
        }
        return getShieldStatus();
    }
}
