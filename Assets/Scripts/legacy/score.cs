using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour {

	public Transform scoreBoardText;

	private int scoreCount = 0;
	private string givenText = "";

	// Use this for initialization
	void Start () {
		givenText = scoreBoardText.GetComponent<Text> ().text;
		scoreBoardText.GetComponent<Text> ().text = givenText + "0";
	}
	
	public void playerHitMe(GameObject gem){
		scoreBoardText.GetComponent<Text> ().text = givenText + (++scoreCount).ToString();
		Destroy (gem.gameObject);
	}

	public int getScore(){
		return scoreCount;
	}
}
