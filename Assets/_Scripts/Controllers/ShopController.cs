using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopController : MonoBehaviour {

	public BoolVariable menuLock;

	[Header("References")]
	public AreaIntVariable playerArea;
	public AreaIntVariable currentArea;
	public FloatVariable fadeSpeed;

	public UnityEvent buttonClickedEvent;
	public UnityEvent changeMapEvent;


	private void Start() {
		StartCoroutine(WaitForFadeIn());
	}

	/// <summary>
	/// Returns the player to the game again.
	/// </summary>
	public void ReturnToGame() {
		if (menuLock.value)
			return;
		
		menuLock.value = true;
		currentArea.value = playerArea.value;
		buttonClickedEvent.Invoke();
		changeMapEvent.Invoke();
	}

	/// <summary>
	/// Locks the menu until it has faded in.
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForFadeIn() {
		menuLock.value = true;
		yield return new WaitForSeconds(fadeSpeed.value);
		menuLock.value = false;
		yield break;
	}
}
