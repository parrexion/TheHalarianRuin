using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStandalone : MonoBehaviour {

	
// 	void OnAwake() {
// #if !UNITY_EDITOR
// 		Debug.Log("Destroying object " + gameObject.name)
// 		Destroy(gameObject);
// #endif
// 	}

	void OnEnable() {
#if !UNITY_EDITOR
		Debug.Log("Destroying object " + name);
		gameObject.SetActive(false);
#endif
	}
}
