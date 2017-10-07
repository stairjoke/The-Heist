using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Foundation : MonoBehaviour {

	public Texture2D mouseCursor;
	public Canvas menu;
	public Transform startButton;

	void Awake(){
		Time.timeScale = 0;
	}

	public void userClickedStart(){
		menu.enabled = false;
		Time.timeScale = 1;
		Cursor.SetCursor (mouseCursor, new Vector2 (7, 7), CursorMode.Auto);
	}

	public void restart(){
		SceneManager.LoadScene ("Scene", LoadSceneMode.Single);
	}

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetKey ("Esc")) {
			Debug.Log ("received esc key");
		}*/
	}

	/*
	public delegate void CallBack();
	public Vector3 interpolateOverTime(Vector3 vectorStart, Vector3 vectorEnd, Vector3 vectorTarget,  callback, float timeInSeconds){
		float timeStart = Time.time;
		float timeLeft = timeInSeconds;
		float lerpFactor = 0f;
		do{ //at least once
			//Time at beginning - current time = negative number plus time given => counts down from allotted time to negative infinity
			timeLeft = timeStart - Time.time + timeInSeconds;
			lerpFactor = timeLeft / timeInSeconds;
			vectorTarget = Vector3.Lerp(vectorStart, vectorEnd, lerpFactor);
		}while(timeLeft > 0f);

		return vectorTarget;
	}
	*/
}
