using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ID used to identify which inventory the inventory slot belongs to.
/// </summary>
public class SlotID {
	public enum SlotType { MODULE, EQUIP, DESTROY }

	public SlotType type;
	public int id;
}

/// <summary>
/// Class containing the information for the inventory slots.
/// </summary>
public class InventorySlot : MonoBehaviour {

	public bool moveable = true;
	public SlotID slotID;
	public Image icon;
	public ItemEntry item;


	/// <summary>
	/// Sets the SlotID for the inventory slot.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="id"></param>
	public void SetID(SlotID.SlotType type, int id) {
		slotID = new SlotID();
		slotID.type = type;
		slotID.id = id;
	}

	/// <summary>
	/// Adds an item to the slot, overwriting the previous item.
	/// </summary>
	/// <param name="newItem"></param>
	public void AddItem(ItemEntry newItem) {
		item = newItem;

		icon.sprite = item.icon;
		icon.color = item.tintColor;
		icon.enabled = true;
	}
	
	/// <summary>
	/// Empties the inventory slot.
	/// </summary>
	public void ClearSlot() {

		item = null;
		icon.sprite = null;
		icon.enabled = false;
	}

}
