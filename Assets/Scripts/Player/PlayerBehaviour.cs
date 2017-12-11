using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public float moveSpeed = 1;
    private float backupInitialDragValue;
    private Rigidbody me;
    private UserInputAbstraction abstractInputs;

    void Start(){
        backupInitialDragValue = GetComponent<Rigidbody>().drag;
        me = GetComponent<Rigidbody>();
        abstractInputs = new UserInputAbstraction();
    }
	
	// Update is called once per frame
	void Update () {
        shoot(); //First shoot
        move(); //Then Move
        setDragDefault(); //Then reset drag, incase it was changed by environment
        /*
         * Drag reset must be done last, in order for external drag sources to have an impact on move()
        */
	}

    private void setDragDefault(){
        me.drag = backupInitialDragValue;
    }

    public void receiveForce(Vector3 force3D, float dragDifference = 0f){
        /*
         * Receives external forces and drag
         * Example: Player runs into force field
         * Example: Player runs into mud
        */
        force3D.y = 0f;
        if (!force3D.magnitude.Equals(0f)){ //Only apply if relevant
            me.AddForce(force3D, ForceMode.Force);
        }
        if (!dragDifference.Equals(0f)){ //Only apply if relevant
            me.drag = backupInitialDragValue + dragDifference;
        }
    }

    private void move(){
        /*
         * add motion to rigidbody using deltaTime and move Speed
         * add motino as force, not at acceleration to avoid infinite speeds
        */
        me.AddForce(abstractInputs.motionVector() * moveSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void shoot(){
        if(UserInputAbstraction.isShooting()){
            //GetComponent -> Weapon -> isFiring=true
        }else{
            //GetComponent -> Weapon -> isFiring=false
        }
    }
}
