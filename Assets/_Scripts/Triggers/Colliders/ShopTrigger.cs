using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopTrigger : OWTrigger {

	public string shopName;


	public override void Trigger() {
		Debug.Log("Visiting shop: "+ shopName);
		startEvent.Invoke();
	}
}
