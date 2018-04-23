using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public BoolVariable paused;
	public GameObject pauseMenu;

	public void TriggerPaused() {
		pauseMenu.SetActive(paused.value);
	}
}
