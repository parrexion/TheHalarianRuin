using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which contains help functions for manipulating the inventory lists.
/// </summary>
public class InventoryHandler : MonoBehaviour {

	public IntVariable inventoryOffset;
	public InvListVariable equippedItems;
	public InvListVariable bagItems;
	public InvListVariable shopItems;
	public IntVariable currentMoney;

	public int bagInvSize;
	public int rowLength;

	public UnityEvent itemsChanged;


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
	/// <param name="start"></param>
	/// <param name="drop"></param>
	public void Swap(SlotID start, SlotID drop) {
		if (start.type != SlotType.EQUIP && start.type != SlotType.BAG)
			return;


		// Debug.Log(string.Format("Swappy: {0} <> {1}", posA, posB));
		ItemEntry temp = GetItem(start);
		if (!drop.CanUse(temp))
			return;

		SetItem(start,GetItem(drop));
		SetItem(drop,temp);

		itemsChanged.Invoke();
	}

	/// <summary>
	/// Destroys the dropped item if applicable.
	/// </summary>
	/// <param name="start"></param>//
	public void DestroyItem(SlotID start) {
		if (start.type != SlotType.EQUIP && start.type != SlotType.BAG)
			return;

		Remove(start);
		Debug.Log("Destroyed the item");
		itemsChanged.Invoke();
	}

	/// <summary>
	/// Buys the dropped item if applicable.
	/// </summary>
	/// <param name="start"></param>
	public void BuyItem(SlotID start, SlotID dropped) {
		if (dropped.type != SlotType.EQUIP && dropped.type != SlotType.BAG)
			return;
		
		ItemEntry item = GetItem(start);
		if (currentMoney.value >= item.cost && !GetItem(dropped) && dropped.CanUse(item)) {
			currentMoney.value -= item.cost;
			Debug.Log("Bought the item");
			SetItem(dropped,item);
			// AddBag(item);
			itemsChanged.Invoke();
		}
	}

	/// <summary>
	/// Sells the dropped item if applicable.
	/// </summary>
	/// <param name="start"></param>
	public void SellItem(SlotID start) {
		if (start.type != SlotType.EQUIP && start.type != SlotType.BAG)
			return;
		
		ItemEntry item = GetItem(start);
		currentMoney.value += item.cost / 2;
		Remove(start);
		Debug.Log("Sold the item");
		itemsChanged.Invoke();
	}

	/// <summary>
	/// Removes the item at index position.
	/// Equipped items use negative indexing starting at -1
	/// </summary>
	/// <param name="index"></param>
	private void Remove(SlotID index){
		if (index.type == SlotType.EQUIP) {
			equippedItems.values[index.id] = null;
		}
		else if (index.type == SlotType.SHOP) {
			shopItems.values[index.id] = null;
		}
		else {
			bagItems.values[index.id + inventoryOffset.value] = null;
		}
	}

	/// <summary>
	/// Retrieves the item at the current index.
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	private ItemEntry GetItem(SlotID index) {
		if (index.type == SlotType.EQUIP) {
			return (index.id < equippedItems.values.Length) ? equippedItems.values[index.id] : null;
		}
		else if (index.type == SlotType.SHOP) {
			return (index.id < shopItems.values.Length) ? shopItems.values[index.id] : null;
		}
		else if (index.type == SlotType.BAG) {
			return (index.id + inventoryOffset.value < bagItems.values.Length) ? bagItems.values[index.id + inventoryOffset.value] : null;
		}

		return null;
	}

	/// <summary>
	/// Sets the given item in the slot at the index.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="item"></param>
	/// <returns></returns>
	private void SetItem(SlotID index, ItemEntry item) {

		if (index.type == SlotType.EQUIP) {
			if (index.id < equippedItems.values.Length) {
				equippedItems.values[index.id] = item;
			}
		}
		else if (index.type == SlotType.SHOP) {
			if (index.id < shopItems.values.Length) {
				shopItems.values[index.id] = item;
			}
		}
		else if (index.type == SlotType.BAG) {
			if (index.id + inventoryOffset.value < bagItems.values.Length) {
				bagItems.values[index.id + inventoryOffset.value] = item;
			}
		}
	}
		
	public void UpdateInvPos(int direction) {
		inventoryOffset.value += direction * rowLength;
		inventoryOffset.value = Mathf.Clamp(inventoryOffset.value, 0, Constants.GEAR_BAG_SPACE - bagInvSize);
		itemsChanged.Invoke();
	}
}
