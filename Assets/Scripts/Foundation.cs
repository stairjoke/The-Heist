using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Foundation : MonoBehaviour {

	public Canvas menu;
	public Transform startButton;

	void Awake(){
		Time.timeScale = 0;
	}

	public void userClickedStart(){
		menu.enabled = false;
		Time.timeScale = 1;
	}

	public void restart(){
		SceneManager.LoadScene ("Scene", LoadSceneMode.Single);
	}

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}
}
