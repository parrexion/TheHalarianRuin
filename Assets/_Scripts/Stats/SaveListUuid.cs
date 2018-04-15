using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save class which contains a list of uuids
/// </summary>
[System.Serializable]
public class SaveListUuid {

	public int size;
	public string[] uuids;


	public SaveListUuid() {
		size = 4;
		uuids = new string[size];
	}

	public SaveListUuid(int size) {
		this.size = size;
		uuids = new string[size];
		for (int i = 0; i < size; i++) {
			uuids[i] = "";
		}
	}
}
