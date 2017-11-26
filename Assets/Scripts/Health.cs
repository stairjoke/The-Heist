using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {

    public bool regenerateWhileFightingWithShield = true;
    public bool regenerateWhileFightingNoShield = false;
    public float autoRegenerateDelayInSeconds = 0.5f;
    public Transform destroyEffect;
    private float healthStatus = 1f;

    public float getHealth(){
        return healthStatus;
    }
    private IEnumerator selfDestructAtEndOfFrame(){
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
    public float changeHealth(float difference, bool autoDestroy = true){
        healthStatus += difference;
        healthStatus = Mathf.Clamp(shieldStatus, 0f, 1f);
        if(autoDestroy && destroyEffect != null){
            Instantiate(destroyEffect, this.transform.position, this.transform.rotation);
            StartCoroutine(selfDestructAtEndOfFrame());
        }
        return getHealth();
    }


    public bool hasShield = false;
    public Transform shieldTransform;
    private float shieldStatus = 1f;
    private Transform shieldInstance;

    public float getShieldStatus(){
        return (hasShield) ? shieldStatus : 0f;
    }
    private void disableShield(){
        hasShield = false;
        shieldStatus = 1f;
        if(shieldInstance != null){
            Destroy(shieldInstance.gameObject);
            shieldInstance = null;
        }
    }
    private void enableShield(){
        hasShield = true;
        shieldStatus = 1f;
        if(shieldInstance == null){
            shieldInstance = Instantiate(shieldTransform, this.transform.position, this.transform.rotation, this.transform);
        }
    }
    public float changeShieldStatus(bool b){
        if(b){
            enableShield();
        }else{
            disableShield();
        }
        return getShieldStatus();
    }
    public float changeShieldStatus(float difference){
        shieldStatus += difference;
        shieldStatus = Mathf.Clamp(shieldStatus, 0f, 1f);
        if (shieldStatus.Equals(0f)){
            disableShield();
        }
        return getShieldStatus();
    }
}
