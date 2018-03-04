using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour {

	[Tooltip("Event to register with.")]
	public GameEvent evnt;

	[Tooltip("")]
	public UnityEvent response;


	void OnEnable() {
		evnt.RegisterListener(this);
	}

	void OnDisable() {
		evnt.UnregisterListener(this);
	}

	public void OnEventRaised() {
		response.Invoke();
	}
}
