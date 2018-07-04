﻿using UnityEngine;

/// <summary>
/// The UI component of the inventory screen containing all the images of the inventory.
/// </summary>
public class InventoryUI : MonoBehaviour {

	public Transform equipItemsParent;
	public Transform bagItemsParent;
	public Transform destroyTransform;

	InventorySlot[] equipSlots;
	InventorySlot[] bagSlots;
	InventorySlot destroySlot;

	public InvListVariable invItemEquip;
	public InvListVariable invItemBag;


	// Use this for initialization
	void Start () {

		//Slot initialization
		equipSlots = equipItemsParent.GetComponentsInChildren<InventorySlot>();
		for (int i = 0; i < equipSlots.Length; i++) {
			equipSlots[i].SetID(SlotType.EQUIP,i);
		}
		equipSlots[0].slotID.equipType = EquipType.WEAPON;
		equipSlots[1].slotID.equipType = EquipType.HEAD;
		equipSlots[2].slotID.equipType = EquipType.BODY;
		equipSlots[3].slotID.equipType = EquipType.VISION;
		
		bagSlots = bagItemsParent.GetComponentsInChildren<InventorySlot>();
		for (int i = 0; i < bagSlots.Length; i++) {
			bagSlots[i].SetID(SlotType.BAG,i);
		}

		destroySlot = destroyTransform.GetComponent<InventorySlot>();
		destroySlot.SetID(SlotType.DESTROY,-999);

		Debug.Log("Initiated the slot ids");
		UpdateUI();
	}

	/// <summary>
	/// Update function for the UI. Uses the inventory to update the images of all the inventory slots.
	/// </summary>
	public void UpdateUI() {

		//Update the equipment
		for (int i = 0; i < equipSlots.Length; i++) {
			if (invItemEquip.values[i] != null) {
				equipSlots[i].AddItem(invItemEquip.values[i]);
			}
			else {
				equipSlots[i].ClearSlot();
			}
		}

		for (int i = 0; i < bagSlots.Length; i++) {
			if (invItemBag.values[i] != null) {
				bagSlots[i].AddItem(invItemBag.values[i]);
			}
			else {
				bagSlots[i].ClearSlot();
			}
		}
	}

}
