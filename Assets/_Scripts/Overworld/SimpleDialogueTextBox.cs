using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleDialogueTextBox : MonoBehaviour, IPointerDownHandler {

	public BoolVariable paused;


    private void Start () {
		gameObject.SetActive(false);
	}

    public void OnPointerDown(PointerEventData eventData) {
		paused.value = false;
        gameObject.SetActive(false);
    }
}
