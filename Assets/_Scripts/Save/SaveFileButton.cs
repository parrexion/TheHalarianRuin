using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour, IPointerClickHandler {

	public SaveFileController controller;
	public int index;

	public GameObject emptyFile;
	public GameObject saveStats;

	public UnityEvent itemSelectedEvent;
	
	[Header("Save file data")]
	public Text currentChapter;
	public Text currentArea;
	public Text level;
	public Text playTime;
	public Image[] modules;
	public Image[] equipments;
	
	private Image buttonImage;


	public void SetImage(Sprite img) {
		if (buttonImage == null)
			buttonImage = GetComponent<Image>();
		buttonImage.sprite = img;
	}
	
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
		if (controller != null) 
        	controller.SelectSaveFile(index);
		itemSelectedEvent.Invoke();
    }
}
