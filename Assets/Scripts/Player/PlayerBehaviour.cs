using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public float moveSpeed = 1;
    private float backupInitialDragValue;
    private Rigidbody me;

    void Start(){
        backupInitialDragValue = GetComponent<Rigidbody>().drag;
        me = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        shoot();
        setDragDefault();
        move();
	}

    private void setDragDefault(){
        me.drag = backupInitialDragValue;
    }

    public void receiveForce(Vector3 force3D, float dragDifference = 0f){
        force3D.y = 0f;
        if (!force3D.magnitude.Equals(0f)){
            me.AddForce(force3D, ForceMode.Force);
        }
        if (!dragDifference.Equals(0f)){
            me.drag = backupInitialDragValue + dragDifference;
        }
    }

    private void move(){
        me.AddForce(UserInputAbstraction.motionVector() * moveSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void shoot(){
        if(UserInputAbstraction.isShooting()){
            //GetComponent -> Weapon -> isFiring=true
        }else{
            //GetComponent -> Weapon -> isFiring=false
        }
    }
}
