using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueAdvanceFrame : MonoBehaviour, IPointerClickHandler {

	public ScrObjLibraryVariable dialogueLibrary;
	public StringVariable dialogueUuid;
	private DialogueEntry dialogueEntry;

    public IntVariable currentFrame;
    public BoolVariable skippableDialogue;
	public UnityEvent dialogueClickEvent;


    private void OnEnable() {
        dialogueEntry = (DialogueEntry)dialogueLibrary.GetEntry(dialogueUuid.value);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S) && skippableDialogue.value){
            currentFrame.value = dialogueEntry.actions.Count-1;
            dialogueClickEvent.Invoke();
            dialogueClickEvent.Invoke();
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        dialogueClickEvent.Invoke();
    }
}
