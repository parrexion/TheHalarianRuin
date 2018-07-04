using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for displaying the information of the selected item.
/// </summary>
public class SelectedShopItemUI : MonoBehaviour {

	public Transform[] statsTextList;
	public Transform[] modifierTextList;

	[Header("Player equipped")]
	public InvListVariable equippedItems;

	[Header("Selected item")]
	public ItemEntryReference selectedItem;
	public Text itemName;
	public Image itemIcon;
	
	public Sprite itemGoodIcon;
	public Sprite itemBadIcon;

	private Image[] differenceIcon;
	private Text[] changes;
	private Text[] modifiers;

	private ItemEquip _currentItem;
	private ItemEquip _equippedItem;
	private int _effectSize;


	void Start () {
		//Set up the texts showing the stats
		Text[] t;
		changes = new Text[statsTextList.Length];
		differenceIcon = new Image[statsTextList.Length];
		for (int i = 0; i < statsTextList.Length; i++) {
			t = statsTextList[i].GetComponentsInChildren<Text>();
			changes[i] = t[1];
			changes[i].text = "";
			differenceIcon[i] = statsTextList[i].GetComponentInChildren<Image>();
			differenceIcon[i].enabled = false;
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

		if (selectedItem.reference != null) {
			_currentItem = (ItemEquip)selectedItem.reference;
			itemName.text = _currentItem.entryName;
			itemIcon.sprite = _currentItem.icon;
			itemIcon.color = _currentItem.tintColor;
			itemIcon.enabled = true;
			
			changes[0].text = (_currentItem.healthModifier != 0) ? "+" + _currentItem.healthModifier.ToString() : "  -";
			changes[1].text = (_currentItem.attackModifier != 0) ? "+" + _currentItem.attackModifier.ToString() : "  -";
			changes[2].text = (_currentItem.defenseModifier != 0) ? "+" + _currentItem.defenseModifier.ToString() : "  -";
			changes[3].text = (_currentItem.sAttackModifier != 0) ? "+" + _currentItem.sAttackModifier.ToString() : "  -";
			changes[4].text = (_currentItem.sDefenseModifier != 0) ? "+" + _currentItem.sDefenseModifier.ToString() : "  -";
			
			_equippedItem = (ItemEquip)equippedItems.GetItemByType(_currentItem.equipType);
			int value = (_equippedItem) ? _equippedItem.healthModifier : 0;
			differenceIcon[0].sprite = (_currentItem.healthModifier - value > 0) ? itemGoodIcon :itemBadIcon;
			differenceIcon[0].enabled = (_currentItem.healthModifier - value != 0);
			value = (_equippedItem) ? _equippedItem.attackModifier : 0;
			differenceIcon[1].sprite = (_currentItem.attackModifier - value > 0) ? itemGoodIcon : itemBadIcon;
			differenceIcon[1].enabled = (_currentItem.attackModifier - value != 0);
			value = (_equippedItem) ? _equippedItem.defenseModifier : 0;
			differenceIcon[2].sprite = (_currentItem.defenseModifier - value > 0) ? itemGoodIcon : itemBadIcon;
			differenceIcon[2].enabled = (_currentItem.defenseModifier - value != 0);
			value = (_equippedItem) ? _equippedItem.sAttackModifier : 0;
			differenceIcon[3].sprite = (_currentItem.sAttackModifier - value > 0) ? itemGoodIcon : itemBadIcon;
			differenceIcon[3].enabled = (_currentItem.sAttackModifier - value != 0);
			value = (_equippedItem) ? _equippedItem.sDefenseModifier : 0;
			differenceIcon[4].sprite = (_currentItem.sDefenseModifier - value > 0) ? itemGoodIcon : itemBadIcon;
			differenceIcon[4].enabled = (_currentItem.sDefenseModifier - value != 0);

			_effectSize = Mathf.Min(2,_currentItem.percentModifiers.Count);
			for (int i = 0; i < 2; i++) {
				modifiers[i].text = (i < _effectSize) ? _currentItem.percentModifiers[i].GetEffectString() : "";
			}
		}
		else {
			itemName.text = "";
			itemIcon.enabled = false;
			for (int i = 0; i < statsTextList.Length; i++) {
				changes[i].text = "";
				differenceIcon[i].enabled = false;
			}
			for (int i = 0; i < modifierTextList.Length; i++) {
				modifiers[i].text = "";
			}
		}
	}
}
