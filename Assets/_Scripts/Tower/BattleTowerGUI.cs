using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTowerGUI : MonoBehaviour {

	public IntVariable currentTowerLevel;
	public Text towerPrev;
	public Text towerNext;

	// Use this for initialization
	void Start () {
		towerPrev.text = (currentTowerLevel.value -1).ToString();
		towerNext.text = currentTowerLevel.value.ToString();
	}

}
