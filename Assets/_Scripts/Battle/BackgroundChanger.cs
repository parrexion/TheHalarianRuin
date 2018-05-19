using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour {
	public Button escapeBattleButton;

	public WeaponSlot weapons;
	public SoldierGridController gridController;
	public SoldierGrid soldierGrid;

	public SpriteRenderer tutorialAndroid;
	public SpriteRenderer tutorialSoldier;

	public SpriteRenderer backgroundAndroid;
	public SpriteRenderer backgroundSoldier;

	public Camera cameraAndroid;
	public Camera cameraSoldier;
}
