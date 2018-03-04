using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicTrigger : OWTrigger {

	public AudioClip musicClip;
	public AudioVariable backgroundMusic;


	/// <summary>
	/// Sets up all blocks and hides them from sight.
	/// </summary>
	protected override void Startup(){
		if (!active)
			return;
			
		backgroundMusic.value = musicClip;
		Debug.Log("Music is now: " + musicClip.name);
		startEvent.Invoke();
		Deactivate();
	}

	/// <summary>
	/// Triggered when called from another trigger in-game.
	/// </summary>
	public override void IngameTrigger() {
		Startup();
	}

    public override void Trigger() {}
}
