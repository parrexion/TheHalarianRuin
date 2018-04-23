using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UuidContainer : MonoBehaviour {
#region Singleton

	private static UuidContainer instance = null;

	void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
#endregion

	public StringVariable battleUuid;
	public StringVariable dialogueUuid;
}
