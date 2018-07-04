using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler {

	public static GameObject itemBeingDragged;

	public InventorySlot slot;
	public SlotID start_id;
	public ItemEntryReference selectedItem;
	public Transform invParent;

	private Image image;

	void Start() {
		image = GetComponent<Image>();
	}

	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData) {
		if (!slot.moveable) {
			return;
		}
		itemBeingDragged = gameObject;
		selectedItem.reference = slot.item;
		start_id = slot.slotID;
		image.raycastTarget = false;
		transform.parent.transform.SetAsLastSibling();
		invParent.SetAsLastSibling();
	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag(PointerEventData eventData) {
		if (!slot.moveable) {
			return;
		}
		transform.position = Input.mousePosition;
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag(PointerEventData eventData) {
		if (!slot.moveable) {
			return;
		}
		itemBeingDragged = null;
		image.raycastTarget = true;
		transform.localPosition = Vector3.zero;
	}

	#endregion

	#region IPointerDownHandler implementation

	public void OnPointerDown(PointerEventData eventData) {
		if (slot.slotID.type != SlotType.DESTROY && slot.slotID.type != SlotType.SELL)
			selectedItem.reference = slot.item;
	}

	#endregion
}
