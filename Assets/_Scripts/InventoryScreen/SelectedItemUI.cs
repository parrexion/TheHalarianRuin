using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for displaying the information of the selected item.
/// </summary>
public class SelectedItemUI : MonoBehaviour {

	public Transform[] statsTextList;
	public Transform[] modifierTextList;

	//Player stats
	public IntVariable playerHealth;
	public IntVariable playerAttack;
	public IntVariable playerDefense;
	public IntVariable playerSAttack;
	public IntVariable playerSDefense;

	private Text[] names;
	private Text[] values;

	//Selected item
	public ItemEntryReference selectedItem;
	private ItemEquip currentItem;
	public Text itemName;
	private Text[] changes;
	public Image itemIcon;
	private Text[] modifiers;
	private int effectSize;


	void Start () {
		//Set up the texts showing the stats
		Text[] t;
		names = new Text[statsTextList.Length];
		values = new Text[statsTextList.Length];
		changes = new Text[statsTextList.Length];
		for (int i = 0; i < statsTextList.Length; i++) {
			t = statsTextList[i].GetComponentsInChildren<Text>();
			names[i] = t[0];
			values[i] = t[1];
			changes[i] = t[2];
//			names[i].text = "name";
			values[i].text = i.ToString();
			changes[i].text = "";
		}

		modifiers = new Text[modifierTextList.Length];
		for (int i = 0; i < modifierTextList.Length; i++) {
			modifiers[i] = modifierTextList[i].GetComponent<Text>();
			modifiers[i].text = "";
		}
	}

	void OnEnable() {
		selectedItem.reference = null;
	}

	void Update () {
		//Update values
		UpdateValues();
	}

	/// <summary>
	/// Updates the information text of the currently selected item.
	/// </summary>
	void UpdateValues(){
		values[0].text = playerHealth.value.ToString();
		values[1].text = playerAttack.value.ToString();
		values[2].text = playerDefense.value.ToString();
		values[3].text = playerSAttack.value.ToString();
		values[4].text = playerSDefense.value.ToString();

		if (selectedItem.reference != null) {
			currentItem = (ItemEquip)selectedItem.reference;
			itemName.text = currentItem.entryName;
			itemIcon.sprite = currentItem.icon;
			itemIcon.color = currentItem.tintColor;
			itemIcon.enabled = true;
			
			changes[0].text = (currentItem.healthModifier != 0) ? "+" + currentItem.healthModifier.ToString() : "";
			changes[1].text = (currentItem.attackModifier != 0) ? "+" + currentItem.attackModifier.ToString() : "";
			changes[2].text = (currentItem.defenseModifier != 0) ? "+" + currentItem.defenseModifier.ToString() : "";
			changes[3].text = (currentItem.sAttackModifier != 0) ? "+" + currentItem.sAttackModifier.ToString() : "";
			changes[4].text = (currentItem.sDefenseModifier != 0) ? "+" + currentItem.sDefenseModifier.ToString() : "";

			effectSize = Mathf.Min(3,currentItem.percentModifiers.Count);
			for (int i = 0; i < 3; i++) {
				modifiers[i].text = (i < effectSize) ? currentItem.percentModifiers[i].GetEffectString() : "";
			}
		}
		else {
			itemName.text = "";
			changes[0].text = "";
			changes[1].text = "";
			changes[2].text = "";
			changes[3].text = "";
			changes[4].text = "";
			itemIcon.enabled = false;
			modifiers[0].text = "";
			modifiers[1].text = "";
			modifiers[2].text = "";
		}
	}
}
