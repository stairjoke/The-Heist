using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservePlayer : MonoBehaviour {

	public Transform player;
    public float touchDelayRampTime = 0.75f;
    private float touchDelayRamp = 0f;
    private Vector3 lerpStart;

	// Use this for initialization
	void Start () {
		if (player == null) {
			Debug.Log ("Camera can't find player");
		}
        lerpStart = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        var desiredPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

        if(Input.touches.Length < 1){ //no interaction
            touchDelayRamp += Time.deltaTime; //start Counting time

            //These two movement combined describe an x^2 relationship:
            //Move Position closer to destination
            transform.position = Vector3.Lerp(lerpStart, desiredPosition, touchDelayRamp / touchDelayRampTime);

            //Move Starting position closer to destination
            lerpStart = Vector3.Lerp(lerpStart, transform.position, touchDelayRamp/touchDelayRampTime);
        }else{
            touchDelayRamp = 0;
            lerpStart = transform.position;
        }
	}
}
