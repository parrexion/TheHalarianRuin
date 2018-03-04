using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for displaying the information of the selected module.
/// </summary>
public class SelectedModuleUI : MonoBehaviour {

	public Transform[] statsTextList;

	//Selected item
	public ItemEntryReference selectedModule;
	private Module currentModule;
	public Text itemName;
	public Image itemIcon;
	private Text[] names;
	private Text[] values;


	void Start () {

		//Set up the texts showing the stats
		Text[] t;
		names = new Text[statsTextList.Length];
		values = new Text[statsTextList.Length];
		for (int i = 0; i < statsTextList.Length; i++) {
			t = statsTextList[i].GetComponentsInChildren<Text>();
			names[i] = t[0];
			values[i] = t[1];
			values[i].text = i.ToString();
		}

	}

	void OnEnable() {
		selectedModule.reference = null;
	}

	void Update () {
		//Update values
		UpdateValues();
	}


	/// <summary>
	/// Updates the information text of the currently selected module.
	/// </summary>
	void UpdateValues(){

		if (selectedModule.reference != null) {
			currentModule = (Module)selectedModule.reference;
			itemName.text = currentModule.entryName;
			itemIcon.sprite = currentModule.icon;
			itemIcon.enabled = true;
			values[0].text = currentModule.values.moduleType.ToString();
			values[1].text = currentModule.values.damage.ToString();
			values[2].text = currentModule.values.maxCharges.ToString();
			values[3].text = (currentModule.values.cooldown != -1) ? currentModule.values.cooldown.ToString() + " s" : "-";
		}
		else {
			itemName.text = "";
			itemIcon.enabled = false;
			values[0].text = "";
			values[1].text = "";
			values[2].text = "";
			values[3].text = "";
		}
	}
}
