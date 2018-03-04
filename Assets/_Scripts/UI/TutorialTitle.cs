using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTitle : MonoBehaviour {

	public Text tutorialTitle;
	public StringVariable currentTitle;

	// Use this for initialization
	void Start () {
		tutorialTitle.text = currentTitle.value;
	}
	
}
