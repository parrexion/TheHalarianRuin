using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimpleDialogueTextBox : MonoBehaviour, IPointerDownHandler {

	public BoolVariable paused;
    public UnityEvent dialogueBoxClicked;


    private void Start() {
		gameObject.SetActive(false);
	}

    public void OnPointerDown(PointerEventData eventData) {
		dialogueBoxClicked.Invoke();
    }

    public void BoxClicked() {
		paused.value = false;
        gameObject.SetActive(false);
    }
}
