using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour {

	public List<Sprite> tutorialBackgrounds;
	public List<Sprite> androidBackgrounds;
	public List<Sprite> soldierBackgrounds;

	public Button escapeBattleButton;

	public WeaponSlot weapons;
	public SoldierGridController gridController;
	public SoldierGrid soldierGrid;

	public SpriteRenderer tutorialAndroid;
	public SpriteRenderer tutorialSoldier;

	public SpriteRenderer transformAndroid;
	public SpriteRenderer transformSoldier;

	public Camera cameraAndroid;
	public Camera cameraSoldier;
}
