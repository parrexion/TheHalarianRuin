using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleSfxAudioController : MonoBehaviour {

	[Header("References")]
	public AudioQueueVariable currentSfx;
	public UnityEvent playSfx;


	/// <summary>
	/// Plays the buy item sfx when triggered.
	/// </summary>
	public void PlaySingleSfx(SfxEntry entry) {
		currentSfx.value.Enqueue(entry.clip);
		playSfx.Invoke();
	}

}
