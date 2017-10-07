using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enableButtonWhenReady : MonoBehaviour {

	public Transform loadingText;
	public Transform button;

	void OnSceneLoaded(){
		loadingText.GetComponent<Image> ().enabled = false;
		button.GetComponent<Image> ().enabled = true;
		button.GetComponent<Button> ().interactable = true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
