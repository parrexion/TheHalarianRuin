using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AreaInfoValues : ScriptableObject {

	public AreaValue[] areas;


	public AreaValue GetArea(int id, int roomID) {
		for (int i = 0; i < areas.Length; i++) {
			if (areas[i].id == id && (areas[i].roomID == roomID || areas[i].roomID == -1))
				return areas[i];
		}
		Debug.LogWarning("Could not find an area with the id:  " + id);
		return null;
	}
}


[System.Serializable]
public class AreaValue {
	public int id;
	public string path;
	public string locationName;
	public int sceneID;
	public int roomID = -1;
	public Sprite minimap;
}