using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour {

	public IntVariable removeBattleSide;

	[Header("Game speed")]
	public BoolVariable paused;
	public BoolVariable canBeSlowed;
	public BoolVariable slowLeftSide;
	public FloatVariable slowAmount;

	[Header("Libraries")]
	public ScrObjLibraryVariable moduleLibrary;
	public ScrObjLibraryVariable battleLibrary;
	public StringVariable battleUuid;
	public InvListVariable invModuleEquip;

	[Header("Player")]
	public IntVariable playerAttack;

	[Header("Module")]
	public ContainerModule[] containerModules;
	public UIModule[] uiModules;
	private float size;
	private float moduleWidth;
	private float moduleHeight;
	private Texture2D emptySprite;
	private Texture2D filledSprite;
	private Texture2D chargingSprite;

	private float shootCooldown;


	void Start () {
		shootCooldown = 0f;
		
		CalculateHeightDifference();
		SetupTextures();
		SetEquippedModule();
		SetUIModules();
	}

	/// <summary>
	/// Generates the colors used to render the current states of the module.
	/// </summary>
	void SetupTextures(){
		emptySprite = new Texture2D(1,1);
		emptySprite.SetPixel(0,0,Color.grey);
		emptySprite.Apply();
		filledSprite = new Texture2D(1,1);
		filledSprite.SetPixel(0,0,Color.white);
		filledSprite.Apply();
		chargingSprite = new Texture2D(1,1);
		chargingSprite.SetPixel(0,0,Color.yellow);
		chargingSprite.Apply();
	}

	/// <summary>
	/// Updates the size of the UI to fit the current screen resolution.
	/// </summary>
	void CalculateHeightDifference() {
		float p2 = (float)Constants.SCREEN_HEIGHT * (float)Screen.width/(float)Constants.SCREEN_WIDTH;
		float borderAdd = ((float)Screen.height - p2) * 0.5f / Screen.height;
		float resize = (1 - 2*borderAdd);
		float widthDiff = (float)Screen.width / (float)Constants.SCREEN_WIDTH;

		size = Constants.MODULE_SPRITE_SIZE*widthDiff;
		moduleWidth = Constants.MODULE_GUI_XPOS;
		moduleHeight = borderAdd + Constants.MODULE_GUI_YPOS * resize;
	}

	/// <summary>
	/// Reduces the shotcooldown and checks if the module should be activated.
	/// </summary>
	void Update() {
		if (paused.value || removeBattleSide.value == 2)
			return;

		float time = (canBeSlowed.value && !slowLeftSide.value) ? (Time.deltaTime * slowAmount.value) : Time.deltaTime;

		if (shootCooldown > 0) {
			shootCooldown -= time;
		}

		for (int i = 0; i < Constants.MODULE_EQUIP_SPACE; i++) {
			containerModules[i].LowerCooldown(time);
		}

		SetUIModuleCharge();
	}

	/// <summary>
	/// Retrieves the equipped module indexes to be used in the battle and assigns a module to each 
	/// containerModule in the WeaponSlot.
	/// </summary>
	/// <returns></returns>
	void SetEquippedModule(){
		BattleEntry be = (BattleEntry)battleLibrary.GetEntry(battleUuid.value);

		for (int i = 0; i < Constants.MODULE_EQUIP_SPACE; i++) {
			if (be.useSpecificModule) {
				containerModules[i].module = be.equippedModule[i];
			}
			else {
				containerModules[i].module = (Module)invModuleEquip.values[i];
			}
			
			containerModules[i].Initialize(i);

			containerModules[i].slotPos = new Rect(Screen.width*moduleWidth+i*size*1.25f,Screen.height*moduleHeight,size,size);
			containerModules[i].slotFilled = new Rect(Screen.width*moduleWidth+i*size*1.25f,Screen.height*moduleHeight,size,size);
		}
	}
	
	/// <summary>
	/// Sets the icons for the module's UI.
	/// </summary>
	void SetUIModules() {
		for (int i = 0; i < Constants.MODULE_EQUIP_SPACE; i++) {
			Module module = containerModules[i].module;
			if (module == null)
				uiModules[i].SetIcons(null,null);
			else
				uiModules[i].SetIcons(module.icon, module.chargingIcon);
		}
	}

	/// <summary>
	/// Sets the charges for the UI modules.
	/// </summary>
	void SetUIModuleCharge() {
		for (int i = 0; i < Constants.MODULE_EQUIP_SPACE; i++) {
			if (containerModules[i].module == null)
				continue;
			
			float charge = containerModules[i].GetCharge();
			bool active = containerModules[i].active;
			uiModules[i].SetValue(charge, active);
		}
	}

	// /// <summary>
	// /// Shows the UI modules.
	// /// </summary>
	// public void ShowUIModules() {
	// 	for (int i = 0; i < Constants.MODULE_EQUIP_SPACE; i++) {
	// 		if (containerModules[i].module == null)
	// 			continue;
			
	// 		uiModules[i].SetVisible(!paused.value);
	// 	}
	// }

	/// <summary>
	/// Returns if the player is currently attacking.
	/// </summary>
	/// <returns></returns>
	public bool IsAttacking(){
		return (shootCooldown > 0);
	}

	/// <summary>
	/// Checks each module equipped if they can be activated.
	/// </summary>
	/// <param name="mouseInfo"></param>
	/// <returns></returns>
	public bool Activate(MouseInformation mouseInfo) {
		if (shootCooldown > 0f){
			return false;
		}

		for (int i = 0; i < 4; i++) {
			if (containerModules[i].CanActivate(mouseInfo)) {
				shootCooldown = containerModules[i].GetValues().delay;
				containerModules[i].reduceCharge();
				containerModules[i].CreateEffect(mouseInfo, playerAttack.value);
				return true;
			}
		}

		return false;
	}
}
