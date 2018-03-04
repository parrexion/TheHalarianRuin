using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueAdvanceFrame : MonoBehaviour, IPointerClickHandler {

    public IntVariable currentFrame;
    public BoolVariable skippableDialogue;
	public UnityEvent dialogueClickEvent;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        dialogueClickEvent.Invoke();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S) && skippableDialogue.value){
            currentFrame.value += 1000;
            dialogueClickEvent.Invoke();
        }
    }
}
