using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main controller class which contains a reference to most of the global modules 
/// used in the game. 
/// </summary>
public class MainControllerScript : MonoBehaviour {

#region Singleton
	public static MainControllerScript instance;

	/// <summary>
	/// Loads all the modules.
	/// </summary>
	void Awake () {
		if (instance != null) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(transform.gameObject);
		instance = this;
		battleGUI = GetComponent<BattleGUIController>();
		StartCoroutine("WaitForInitiate");
	}
#endregion

	public bool initiated;
	public BattleGUIController battleGUI;
	

	/// <summary>
	/// Waits to make sure that the necessary modules are loaded before the last modules are initiated.
	/// </summary>
	/// <returns></returns>
	void WaitForInitiate() {
		initiated = true;
		Debug.Log("Maincontroller is ready");
	}

}




public static class AppHelper {

	public static void Quit() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}