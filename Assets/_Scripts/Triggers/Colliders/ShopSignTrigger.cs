using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSignTrigger : MonoBehaviour {

	public ShopTrigger interact;
	public bool active = false;
	
	void OnMouseDown() {
        if (!active)
			return;

		interact.EnterShop();
    }
}
