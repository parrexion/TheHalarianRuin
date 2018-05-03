using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DrawGameOverScreen : MonoBehaviour {

	public Text timeText;
	public AreaIntVariable currentArea;
	public UnityEvent buttonClickEvent;
	public UnityEvent changeMapEvent;

	public StringVariable wonBattleState;
	public FloatVariable battleTime;

	// Use this for initialization
	void Start () {
		if (wonBattleState.value != "lose") {
			gameObject.SetActive(false);
			return;
		}

		Debug.Log("Oh noes!");
		SetValues();
	}


	private void SetValues(){

		timeText.text = "You lasted for\n"+ battleTime.value.ToString("F2") + "s";
	}

	public void RetryBattle(){
		buttonClickEvent.Invoke();
		currentArea.value = (int)Constants.SCENE_INDEXES.BATTLE;
		changeMapEvent.Invoke();
	}

	public void ReturnToMainScreen(){
		buttonClickEvent.Invoke();
		currentArea.value = (int)Constants.SCENE_INDEXES.MAINMENU;
		changeMapEvent.Invoke();
	}

}
