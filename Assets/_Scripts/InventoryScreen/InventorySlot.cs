using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ID used to identify which inventory the inventory slot belongs to.
/// </summary>
public enum SlotType { EQUIP, BAG, DESTROY, SHOP, SELL }

/// <summary>
/// ID class for identifying each slot in the inventories.
/// </summary>
public class SlotID {
	public SlotType type;
	public int id;
	public EquipType equipType;

	public bool CanUse(ItemEntry item) {
		return (!item || equipType == EquipType.WILD || item.equipType == EquipType.WILD || equipType == item.equipType);
	}
	public static bool SameType(EquipType item, EquipType item2) {
		return (item == EquipType.WILD || item2 == EquipType.WILD || item == item2);
	}
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
	public void SetID(SlotType type, int id) {
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
