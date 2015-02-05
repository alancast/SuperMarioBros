﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraMGR : MonoBehaviour {

	public static CameraMGR instance;
	public Text 	livesText;
	public static int 		lives = 4;
	public Text		scoreText;
	public static int 		score;
	public Text 	coinageText;
	public static int 		coinage;
	public Text 	timeText;
	public int 		timeInt;
	public Text 	BeastModeText;
		
	void Awake(){
		instance = this;
	}
	
	void Start(){
		//lives = 4;
		GameObject livesGO = GameObject.Find("lives"); 
		if (!livesGO) return;
		livesText = livesGO.GetComponent<Text>();
		livesText.text = lives.ToString();
		GameObject scoreGO = GameObject.Find("score"); 
		if (!scoreGO) return;
		scoreText = scoreGO.GetComponent<Text>();
		scoreText.text = score.ToString();
		GameObject coinageGO = GameObject.Find("coinage"); 
		if (!coinageGO) return;
		coinageText = coinageGO.GetComponent<Text>();
		coinageText.text = coinage.ToString();
		timeInt = 300;
		GameObject timeGO = GameObject.Find("time"); 
		if (!timeGO) return;
		timeText = timeGO.GetComponent<Text>();
		timeText.text = timeInt.ToString();
		GameObject BeastModeGO = GameObject.Find("BeastMode"); 
		if (!BeastModeGO) return;
		BeastModeText = BeastModeGO.GetComponent<Text>();
		if (PE_Controller.BeastMode){
			BeastModeText.text = "Beast Mode Enabled";
		}
		else{
			BeastModeText.text = "Beast Mode Off";
		}
	}
	
	void Update(){
		timeInt = 300 - (int)Time.timeSinceLevelLoad;
		timeText.text = timeInt.ToString();
		if (timeInt <= 0){
			lives -= 1;
			livesText.text = lives.ToString();
			if (lives <= 0){
				lives = 4;
				score = 0;
				coinage = 0;
				Application.LoadLevel("_Scene_End_Game");
			}
			Application.LoadLevel("_Scene_Alex_7");
		}
	}

}
