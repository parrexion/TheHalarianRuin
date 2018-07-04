using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The UI component of the inventory screen containing all the images of the inventory.
/// </summary>
public class ShopUI : MonoBehaviour {

	public GameObject equipItemsParent;
	public GameObject bagItemsParent;
	public GameObject shopItemsParent;
	public Transform sellTransform;

	public InvListVariable invItemEquip;
	public InvListVariable invItemBag;
	public InvListVariable invItemShop;

	InventorySlot[] equipSlots;
	InventorySlot[] bagSlots;
	InventorySlot[] shopSlots;
	InventorySlot sellSlot;

	EquipType currentSellType;

	[Header("Money")]
	public IntVariable totalMoney;
	public Text moneyText;


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

		shopSlots = shopItemsParent.GetComponentsInChildren<InventorySlot>();
		for (int i = 0; i < shopSlots.Length; i++) {
			shopSlots[i].SetID(SlotType.SHOP,i);
			shopSlots[i].GetComponentInChildren<Text>().text = (invItemShop.values[i]) ? invItemShop.values[i].cost.ToString() : "";
		}

		sellSlot = sellTransform.GetComponent<InventorySlot>();
		sellSlot.SetID(SlotType.SELL,-999);

		Debug.Log("Initiated the slot ids");
		UpdateUI();
	}

	/// <summary>
	/// Update function for the UI. Uses the inventory to update the images of all the inventory slots.
	/// </summary>
	public void UpdateUI() {

		//Update money
		moneyText.text = "Money:  " + totalMoney.value.ToString();

		//Update the inventories
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

		int pos = 0;
		for (int i = 0; i < invItemShop.values.Length; i++) {
			if (invItemShop.values[i] != null && SlotID.SameType(currentSellType, invItemShop.values[i].equipType)) {
				shopSlots[pos].AddItem(invItemShop.values[i]);
				shopSlots[pos].SetID(SlotType.SHOP,i);
				shopSlots[pos].GetComponentInChildren<Text>().text = invItemShop.values[i].cost.ToString();
				pos++;
			}
		}
		for (int i = pos; i < shopSlots.Length; i++) {
			shopSlots[i].SetID(SlotType.SHOP,-1);
			shopSlots[i].GetComponentInChildren<Text>().text = "";
			shopSlots[i].ClearSlot();
		}
	}

	public void ShowShopCategory(int category) {
		currentSellType = (EquipType)category;
		UpdateUI();
	}
}
