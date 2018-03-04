using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGUIController : MonoBehaviour {

	public List<DamageNumberDisplay> damages = new List<DamageNumberDisplay>();
	public List<Effect> effectList = new List<Effect>();

	
	// Update is called once per frame
	void Update () {
		damages.RemoveAll(item => (item == null));
		effectList.RemoveAll(item => (item == null));
	}

}
