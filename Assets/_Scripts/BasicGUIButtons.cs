using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Basic class which can be used to transition between scenes. 
/// Can be overridden to add additional features.
/// </summary>
public class BasicGUIButtons : MonoBehaviour {

	public bool useFadeOut = true;
	public AreaInfoValues areaInfo;
	public FloatVariable fadeOutTime;
	public IntVariable currentScene;
	public IntVariable currentRoom;

	public UnityEvent buttonClickEvent;
	public UnityEvent fadeOutEvent;

	public bool fading = false;


	/// <summary>
	/// Moves to the next scene when the button is pressed.
	/// </summary>
	/// <param name="scene">Scene.</param>
	public void MoveToSceneButton(int sceneIndex) {
		if (fading)
			return;
		fading = true;

		buttonClickEvent.Invoke();
		currentScene.value = sceneIndex;
		currentRoom.value = 0;
		StartCoroutine(WaitForFadeOut());
	}

	/// <summary>
	/// Moves to the indicated scene when the event fires.
	/// </summary>
	public void MoveToSceneTrigger() {
		if (fading)
			return;
		fading = true;

		StartCoroutine(WaitForFadeOut());
	}

	/// <summary>
	/// Waits for the screen to fade out before returning to the game.
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForFadeOut() {
		if (useFadeOut) {
			fadeOutEvent.Invoke();
			yield return new WaitForSeconds(fadeOutTime.value);
		}

		fading = false;

		AreaValue currentValue = areaInfo.GetArea(currentScene.value, currentRoom.value);
		SceneManager.LoadScene(currentValue.sceneID);

		yield break;
	}
}
