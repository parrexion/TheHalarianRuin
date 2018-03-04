using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handler for the inventory slots which handles when items are placed into the inventory slot.
/// </summary>
public class SlotHandler : MonoBehaviour, IDropHandler {

	/// <summary>
	/// Get the child object representing the item in the slot.
	/// </summary>
	public GameObject item {
		get {
			if (transform.childCount > 0) {
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}

	public ItemEntryReference selectedItem;
	public InventoryHandler invContainer;

	private DragHandler dragHandler;
	private InventorySlot slot;


	void Start() {
		slot = GetComponent<InventorySlot>();
	}

	#region IDropHandler implementation

	/// <summary>
	/// Updates the position of the item being dropped into this inventory slot.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrop(PointerEventData eventData) {
		if (DragHandler.itemBeingDragged != null) {
			SlotID start_id = DragHandler.itemBeingDragged.GetComponent<DragHandler>().start_id;
			if (slot.slotID.type == SlotID.SlotType.DESTROY)
				selectedItem.reference = null;

			invContainer.Swap(start_id,slot.slotID);
		}
	}

	#endregion
}
