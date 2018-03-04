using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

	public BoolVariable paused;
	public GameObject pauseMenu;

	public void TriggerPaused() {
		pauseMenu.SetActive(paused.value);
	}
}
