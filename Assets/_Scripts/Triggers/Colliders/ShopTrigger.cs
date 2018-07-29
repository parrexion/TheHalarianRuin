using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopTrigger : OWTrigger {

	[Header("Shop Lists")]
	public InvListVariable currentShopList;
	public InvListVariable shopContentList;

	[Header("Shop Sign")]
	public SpriteRenderer signSprite;
	public PopupTrigger shopSignTrigger;


	private void Start() {
		signSprite.enabled = false;
	}

	public override void Trigger() {
		shopSignTrigger.active = true;
		signSprite.enabled = true;
	}

	private void OnTriggerExit2D(Collider2D otherCollider){
		if (!active)
			return;

		shopSignTrigger.active = false;
		signSprite.enabled = false;
	}

	public override void IngameTrigger() {
		paused.value = true;
		currentArea.value = (int)Constants.SCENE_INDEXES.SHOP;
		currentShopList.values = new ItemEntry[shopContentList.values.Length];
		for (int i = 0; i < currentShopList.values.Length; i++) {
			currentShopList.values[i] = shopContentList.values[i];
		}

		startEvent.Invoke();
	}
}
