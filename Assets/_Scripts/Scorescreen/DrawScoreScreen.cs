﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DrawScoreScreen : MonoBehaviour {

	[Header("Libraries")]
	public ScrObjLibraryVariable battleLibrary;
	public StringVariable battleUuid;
	public StringVariable dialogueUuid;

	[Header("Player values")]
	public IntVariable currentArea;
	public IntVariable playerArea;
	public FloatVariable playerPosX;
	public FloatVariable playerPosY;
	public BoolVariable paused;
	public IntVariable totalExp;
	public IntVariable totalMoney;

	[Header("Score values")]
	public StringVariable wonBattleState;
	public BoolVariable playerInvincible;
	public FloatVariable battleTime;
	public IntVariable playerMaxHealth;
	public FloatVariable playerAndroidDamage;
	public FloatVariable playerSoldierDamage;
	public IntVariable enemiesFought;
	public IntVariable expGained;
	public IntVariable moneyGained;

	[Header("Texts")]
	public Text escapedText;
	public Text timeText;
	public Text healthText;
	public Text noEnemiesText;
	public Text expText;
	public Text moneyText;

	[Header("Other")]
	public GameObject levelupTutorial;

	[Header("Events")]
	public UnityEvent buttonClickEvent;
	public UnityEvent changeMapEvent;
	public UnityEvent playMusicEvent;
	public UnityEvent playerStatsChangedEvent;


	// Use this for initialization
	void Awake () {
		if (wonBattleState.value == "lose") {
			gameObject.SetActive(false);
			return;
		}

		if (wonBattleState.value == "escape") {
			expGained.value = 0;
			moneyGained.value = 0;
		}

		ExpLevel startLevel = new ExpLevel(totalExp.value);
		totalExp.value += expGained.value;
		ExpLevel endLevel = new ExpLevel(totalExp.value);
		if (startLevel.level != endLevel.level) {
			playerStatsChangedEvent.Invoke();
			if (endLevel.level == 2)
				levelupTutorial.SetActive(true);
		}

		totalMoney.value += moneyGained.value;
		SetTexts();
		playMusicEvent.Invoke();
	}

	/// <summary>
	/// Sets the values for all the texts in the score screen.
	/// </summary>
	private void SetTexts(){

		if (wonBattleState.value == "win") {
			escapedText.text = "";
		}

		timeText.text = "Time:    "+ battleTime.value.ToString("F2") + "s";
		if (playerMaxHealth.value == 0 || playerInvincible.value) {
			healthText.text = "";
		}
		else {
			float currentHealth = playerMaxHealth.value - (playerAndroidDamage.value + playerSoldierDamage.value);
			healthText.text = "Health left:    "+((currentHealth)/(playerMaxHealth.value) * 100).ToString("F2") + "%";
		}
		if (wonBattleState.value == "win") {
			noEnemiesText.text = "Enemies defeated:   " + enemiesFought.value;
			//Add what type of enemies was defeated
		}
		else {
			noEnemiesText.text = "Enemies faced:   "+enemiesFought.value;
		}

		expText.text = "Experience gained:    "+ expGained.value;
		moneyText.text = "Money gained:    "+ moneyGained.value;
		//Add loot
	}


	public void LeaveScoreScreen(){
		buttonClickEvent.Invoke();
		battleLibrary.GenerateDictionary();
		// Debug.Log("UUID is: " + battleUuid.value);
		BattleEntry be = (BattleEntry)battleLibrary.GetEntry(battleUuid.value);
		switch (be.nextLocation)
		{
			case BattleEntry.NextLocation.OVERWORLD:
				paused.value = false;
				if (be.changePosition) {
					if (be.playerArea != Constants.OverworldArea.DEFAULT)
						playerArea.value = (int)be.playerArea;
					playerPosX.value = be.playerPosition.x;
					playerPosY.value = be.playerPosition.y;
				}
				currentArea.value = playerArea.value;
				changeMapEvent.Invoke();
				break;
			case BattleEntry.NextLocation.DIALOGUE:
				currentArea.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
				dialogueUuid.value = be.nextDialogue.uuid;
				changeMapEvent.Invoke();
				break;
			case BattleEntry.NextLocation.BATTLE:
				currentArea.value = (int)Constants.SCENE_INDEXES.BATTLE;
				battleUuid.value = be.nextBattle.uuid;
				changeMapEvent.Invoke();
				break;
		}

	}

}
