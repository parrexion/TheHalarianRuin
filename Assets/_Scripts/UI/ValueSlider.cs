using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ValueSlider : MonoBehaviour {

	public FloatVariable sliderValue;
	public UnityEvent valueUpdatedValue;
	public Slider slider;
	public Text sliderText;


	// Use this for initialization
	void Start () {
		slider.value = sliderValue.value;
		UpdateText();
	}

	public void UpdateValue(float value) {
		sliderValue.value = value;
		valueUpdatedValue.Invoke();
		UpdateText();
	}

	void UpdateText(){
		sliderText.text = ((int)(sliderValue.value * 100)).ToString();
	}
}
