using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour, IPointerClickHandler {

	public SaveFileController controller;
	public int index;
	
	private Image buttonImage;


	public void SetImage(Sprite img) {
		if (buttonImage == null)
			buttonImage = GetComponent<Image>();
		buttonImage.sprite = img;
	}
	
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        controller.SelectSaveFile(index);
    }
}
