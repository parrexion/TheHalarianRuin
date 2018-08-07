using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/OneLinerDatabase")]
public class OneLinerDatabase : ScriptableObject {


	public List<OneLiner> database = new List<OneLiner>();


	public bool ContainsID(string uuid) {
		for (int i = 0; i < database.Count; i++) {
			if (database[i].name == uuid)
				return true;
		}
		return false;
	}
}
