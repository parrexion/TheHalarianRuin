using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class OWTrigger : MonoBehaviour {

	[Header("Is trigger always active?")]
	public bool alwaysActive = true;
	[Header("Is trigger visible?")]
	public bool visible = false;
	
	[Header("Deactivate/Activate triggers")]
	public List<OWTrigger> deactivateTriggers = new List<OWTrigger>();
	public List<OWTrigger> activateTriggers = new List<OWTrigger>();

	[Header("References - don't touch")]
	public bool active;
	public UUID uuid;
	public SpriteRenderer sprite;
	public SpriteRenderer areaSprite;
	public BoolVariable paused;
	public IntVariable currentArea;

	public UnityEvent startEvent;


	void OnEnable() {
		StartCoroutine(CheckTrigger());
	}

	/// <summary>
	/// Waits for the triggercontroller to initialize before adding this trigger 
	/// to the trigger list.
	/// </summary>
	/// <returns></returns>
	IEnumerator CheckTrigger() {
		while(TriggerController.instance == null)
			yield return null;

		active = TriggerController.instance.CheckActive(uuid.uuid, alwaysActive);
		if (areaSprite != null)
			areaSprite.enabled = false;
		Startup();
	}
	
	/// <summary>
	/// When colliding with a BattleTrigger, start the battle.
	/// </summary>
	/// <param name="otherCollider"></param>
	void OnTriggerEnter2D(Collider2D otherCollider){
		if (!active)
			return;

		if (otherCollider.gameObject.tag != "Player") {
			Debug.Log("That was not a player");
			return;
		}

		Trigger();
	}

	/// <summary>
	/// Triggered when called from another trigger in-game.
	/// </summary>
	public virtual void IngameTrigger() {

	}

	/// <summary>
	/// Activates the trigger.
	/// </summary>
	public void Activate() {
		TriggerController.instance.SetActive(uuid.uuid, true);
		active = true;
		IngameTrigger();
	}

	/// <summary>
	/// Deactivates the trigger.
	/// </summary>
	public void Deactivate() {
		TriggerController.instance.SetActive(uuid.uuid, false);
		active = false;
	}

	/// <summary>
	/// Triggers the action depending on the trigger type.
	/// </summary>
	public abstract void Trigger();

	/// <summary>
	/// Run when the trigger is initialized.
	/// </summary>
	protected virtual void Startup(){
		sprite.enabled = active && visible && sprite.sprite != null;
	}

	/// <summary>
	/// Deactivates and activates the other triggers in the lists.
	/// </summary>
	protected void TriggerOtherTriggers() {
		for (int i = 0; i < deactivateTriggers.Count; i++) {
			if (activateTriggers[i] == null){
				Debug.LogError("Trigger is null in deactivation triggers");
				continue;
			}
			deactivateTriggers[i].Deactivate();
		}
		for (int i = 0; i < activateTriggers.Count; i++) {
			if (activateTriggers[i] == null){
				Debug.LogError("Trigger is null in activation triggers");
				continue;
			}
			activateTriggers[i].Activate();
		}
	}
}
