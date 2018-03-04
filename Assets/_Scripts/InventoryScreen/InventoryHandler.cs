using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which contains help functions for manipulating the inventory lists.
/// </summary>
public class InventoryHandler : MonoBehaviour {

	public UnityEvent itemsChanged;

	public InvListVariable equippedItems;
	public InvListVariable bagItems;


	/// <summary>
	/// Adds a new item to a bag slot if there is room.
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool AddBag(ItemEntry item) {
		for (int i = 0; i < bagItems.values.Length; i++) {
			if (bagItems.values[i] == null) {
				Debug.Log("Placed Item at pos " + i);
				bagItems.values[i] = item;
				return true;
			}
		}

		Debug.Log("Not enough room.");
		return false;
	}

	/// <summary>
	/// Adds a new item to one of the equip slots if there is room.
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool AddEquip(ItemEntry item) {

		for (int i = 0; i < equippedItems.values.Length; i++) {
			if (equippedItems.values[i] == null) {
				equippedItems.values[i] = item;
				return true;
			}
		}

		Debug.Log("Not enough room.");
		return false;
	}

	/// <summary>
	/// Swaps the content of two inventory slots with the given ids.
	/// </summary>
	/// <param name="slotA"></param>
	/// <param name="slotB"></param>
	public void Swap(SlotID slotA, SlotID slotB) {

		int posA = slotA.id;
		int posB = slotB.id;

		if (slotA.type == SlotID.SlotType.DESTROY)
			Remove(posB);
		else if (slotB.type == SlotID.SlotType.DESTROY)
			Remove(posA);

		// Debug.Log(string.Format("Swappy: {0} <> {1}", posA, posB));
		ItemEntry temp = GetItem(posA);
		SetItem(posA,GetItem(posB));
		SetItem(posB,temp);

		itemsChanged.Invoke();
	}

	/// <summary>
	/// Removes the item at index position.
	/// Equipped items use negative indexing starting at -1
	/// </summary>
	/// <param name="index"></param>
	private void Remove(int index){
		if (index < 0) {
			index = -(index+1);
			equippedItems.values[index] = null;
		}
		else {
			bagItems.values[index] = null;
		}
	}

	/// <summary>
	/// Retrieves the item at the current index.
	/// Equipped items use negative indexing starting at -1
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	private ItemEntry GetItem(int index) {
		if (index < 0) {
			index = -(index+1);
			return (index < equippedItems.values.Length) ? equippedItems.values[index] : null;
		}
		else {
			return (index < bagItems.values.Length) ? bagItems.values[index] : null;
		}
	}

	/// <summary>
	/// Sets the given item in the slot at the index.
	/// Equipped items use negative indexing starting at -1
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	private void SetItem(int index, ItemEntry item) {

		if (index < 0) {
			index = -(index+1);
			if (index < equippedItems.values.Length) {
				equippedItems.values[index] = item;
			}
		}
		else {
			if (index < bagItems.values.Length) {
				bagItems.values[index] = item;
			}
		}
	}
		
}
