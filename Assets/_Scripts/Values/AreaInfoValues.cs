using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AreaInfoValues : ScriptableObject {

	public AreaValue[] areas;


	public AreaValue GetArea(int id) {
		for (int i = 0; i < areas.Length; i++) {
			if (areas[i].id == id)
				return areas[i];
		}
		Debug.LogWarning("Could not find an area with the id:  " + id);
		return null;
	}
}


[System.Serializable]
public class AreaValue {
	public int id;
	public int sceneID;
	public string locationName;
	public Sprite minimap;
}