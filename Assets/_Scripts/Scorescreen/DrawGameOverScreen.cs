using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DrawGameOverScreen : MonoBehaviour {

	public Text timeText;
	public UnityEvent buttonClickEvent;

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

}
