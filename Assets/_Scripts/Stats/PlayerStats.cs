using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which keeps a reference to the player's stats throughout the game.
/// Can also recalculate the stats when needed.
/// </summary>
public class PlayerStats : MonoBehaviour {

	#region Singleton
	private static PlayerStats instance;

	protected void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	#endregion

	[Header("Player Stats")]
	public IntVariable playerMaxHealth;
	public IntVariable playerAttack;
	public IntVariable playerDefense;
	public IntVariable playerSAttack;
	public IntVariable playerSDefense;
	// Internal representation
	private float _playerMaxHealth;
	private float _playerAttack;
	private float _playerDefense;
	private float _playerSAttack;
	private float _playerSDefense;

	[Header("Battle Values")]
	public FloatVariable androidDamageTaken;
	public FloatVariable soldierDamageTaken;

	[Header("Overworld values")]
	public IntVariable currentChapterIndex;
	public IntVariable currentArea;
	public IntVariable currentRoomNumber;
	public IntVariable playerArea;
	public FloatVariable playerPosX;
	public FloatVariable playerPosY;

	[Header("Follower values")]
	public BoolVariable playingAsAndroid;
	public BoolVariable useFollower;

	[Header("Level")]
	public IntVariable playerLevel;
	public IntVariable totalExp;

	[Header("Inventory")]
	public IntVariable totalMoney;
	public InvListVariable invItemEquip;
	public InvListVariable invItemBag;
	public InvListVariable invModuleEquip;
	public InvListVariable invModuleBag;

	[Header("Time")]
	public IntVariable ingameDay;
	public StringVariable playTime;
	public IntVariable playedSeconds;
	private int _seconds;
	private int _minutes;
	private int _hours;

	[Header("Libraries")]
	public ScrObjLibraryVariable itemLibrary;
	public ScrObjLibraryVariable moduleLibrary;

	[Header("Saving and Loading")]
	public IntVariable currentSaveFileIndex;
	public UnityEvent saveCheckEvent;
	public UnityEvent loadCheckEvent;


	void Start() {
		StartCoroutine(CountPlayTime());
		CalculateExp();
		RecalculateStats();
	}

	/// <summary>
	/// Continuosly counts up the current play time.
	/// </summary>
	/// <returns></returns>
	IEnumerator CountPlayTime() {
        while (true) {
            yield return new WaitForSeconds(1);
            playedSeconds.value++;
			playTime.value = Constants.PlayTimeFromInt(playedSeconds.value, true);
        }
	}

	/// <summary>
	/// Resets the player's stats.
	/// </summary>
	void ResetPlayerStats() {
		CalculateBaseHealth();
		_playerAttack = 0;
		_playerDefense = 0;
		_playerSAttack = 0;
		_playerSDefense = 0;
	}

	void CalculateBaseHealth() {
		_playerMaxHealth = Constants.PLAYER_HEALTH_BASE + playerLevel.value * Constants.PLAYER_HEALTH_SCALE;
	}

	/// <summary>
	/// Recalculates the player's stats using the current equipment.
	/// </summary>
	public void RecalculateStats() {
		ResetPlayerStats();

		// Start by adding upp all base stats
		for (int i = 0; i < invItemEquip.values.Length; i++) {
			ItemEquip item = (ItemEquip)invItemEquip.values[i];
			if (item == null)
				continue;

			_playerMaxHealth += item.healthModifier;
			_playerAttack += item.attackModifier;
			_playerDefense += item.defenseModifier;
			_playerSAttack += item.sAttackModifier;
			_playerSDefense += item.sDefenseModifier;
		}

		// Add percent modifiers
		for (int i = 0; i < invItemEquip.values.Length; i++) {
			ItemEquip item = (ItemEquip)invItemEquip.values[i];
			if (item == null)
				continue;

			for (int mod = 0; mod < item.percentModifiers.Count; mod++) {
				AddPercentValue(item.percentModifiers[mod].affectedStat, item.percentModifiers[mod].percentValue);
			}
		}

		// Apply changes
		playerMaxHealth.value = (int)(_playerMaxHealth + 0.5f);
		playerAttack.value = (int)(_playerAttack + 0.5f);
		playerDefense.value = (int)(_playerDefense + 0.5f);
		playerSAttack.value = (int)(_playerSAttack + 0.5f);
		playerSDefense.value = (int)(_playerSDefense + 0.5f);
	}

	/// <summary>
	/// Adds the percentage changes to the stats.
	/// </summary>
	/// <param name="stat"></param>
	/// <param name="multiplier"></param>
	void AddPercentValue(StatsPercentModifier.Stat stat, float multiplier) {
		switch(stat)
		{
			case StatsPercentModifier.Stat.ATTACK:
				_playerAttack *= multiplier;
				break;
			case StatsPercentModifier.Stat.DEFENSE:
				_playerDefense *= multiplier;
				break;
			case StatsPercentModifier.Stat.MAXHEALTH:
				_playerMaxHealth *= multiplier;
				break;


			case StatsPercentModifier.Stat.SATTACK:
				_playerSAttack *= multiplier;
				break;
			case StatsPercentModifier.Stat.SDEFENSE:
				_playerSDefense *= multiplier;
				break;
		}
	}

	/// <summary>
	/// Calculates the current level where 100 exp more is required per level.
	/// </summary>
	public void CalculateExp() {
		ExpLevel expLevel = new ExpLevel(totalExp.value);
		playerLevel.value = expLevel.level;
	}


	// SAVING AND LOADING

	/// <summary>
	/// Loads the player stats from the save class data.
	/// </summary>
	/// <param name="saveData"></param>
	public void NewgameStats() {
		//Overworld
		currentChapterIndex.value = 0;
		currentRoomNumber.value = 0;
		currentArea.value = playerArea.value = (int)Constants.SCENE_INDEXES.MAINMENU;
		playerPosX.value = 0;
		playerPosY.value = 0;

		//Follower
		playingAsAndroid.value = true;
		useFollower.value = false;

		//Exp
		playerLevel.value = 1;
		totalExp.value = 0;
		totalMoney.value = 0;

		//Inventory
		invItemBag.Reset();
		invItemEquip.Reset();
		invModuleBag.Reset();
		invModuleEquip.Reset();

		//Time
		ingameDay.value = 1;
		playedSeconds.value = 0;

		CalculateExp();
		RecalculateStats();

		Debug.Log("NEWGAME");
	}

	/// <summary>
	/// Puts the data into a save class for saving.
	/// </summary>
	/// <returns></returns>
	public void SaveStats() {
		PlayerStatsSaveClass playerData = new PlayerStatsSaveClass();

		//Overworld
		playerData.currentChapterIndex = currentChapterIndex.value;
		playerData.currentRoomNumber = currentRoomNumber.value;
		playerData.playerArea = playerArea.value;
		playerData.playerPosX = playerPosX.value;
		playerData.playerPosY = playerPosY.value;

		//Follower
		playerData.playingAsAndroid = playingAsAndroid.value;
		playerData.useFollower = useFollower.value;

		//Exp
		playerData.expTotal = totalExp.value;
		playerData.money = totalMoney.value;

		//Inventory
		playerData.invItemBag = invItemBag.GenerateSaveData();
		playerData.invItemEquip = invItemEquip.GenerateSaveData();
		playerData.invModuleBag = invModuleBag.GenerateSaveData();
		playerData.invModuleEquip = invModuleEquip.GenerateSaveData();

		//Time
		playerData.ingameDay = ingameDay.value;
		playerData.playedSeconds = playedSeconds.value;

		SaveController.instance.saveFiles.playerSave[currentSaveFileIndex.value] = playerData;
		saveCheckEvent.Invoke();
		Debug.Log("SAVED");
	}

	/// <summary>
	/// Loads the player stats from the save class data.
	/// </summary>
	/// <param name="saveData"></param>
	public void LoadStats() {
		PlayerStatsSaveClass saveData = SaveController.instance.saveFiles.playerSave[currentSaveFileIndex.value];

		//Overworld
		currentChapterIndex.value = saveData.currentChapterIndex;
		currentRoomNumber.value = saveData.currentRoomNumber;
		currentArea.value = playerArea.value = saveData.playerArea;
		playerPosX.value = saveData.playerPosX;
		playerPosY.value = saveData.playerPosY;

		//Follower
		playingAsAndroid.value = saveData.playingAsAndroid;
		useFollower.value = saveData.useFollower;

		//Exp
		totalExp.value = saveData.expTotal;
		CalculateExp();

		//Inventory
		totalMoney.value = saveData.money;
		itemLibrary.GenerateDictionary();
		moduleLibrary.GenerateDictionary();
		invItemBag.LoadItemData(saveData.invItemBag, itemLibrary);
		invItemEquip.LoadItemData(saveData.invItemEquip, itemLibrary);
		invModuleBag.LoadItemData(saveData.invModuleBag, moduleLibrary);
		invModuleEquip.LoadItemData(saveData.invModuleEquip, moduleLibrary);

		//Time
		ingameDay.value = saveData.ingameDay;
		playedSeconds.value = saveData.playedSeconds;

		RecalculateStats();

		loadCheckEvent.Invoke();
		Debug.Log("LOADED");
	}
}

/// <summary>
/// Save class for the player stats which extracts the ScrObj variables.
/// </summary>
[System.Serializable]
public class PlayerStatsSaveClass {

	[Header("Overworld values")]
	public string currentChapterString;
	public int currentChapterIndex;
	public int currentRoomNumber;
	public int playerArea;
	public float playerPosX;
	public float playerPosY;

	[Header("Follower values")]
	public bool playingAsAndroid;
	public bool useFollower;

	[Header("Level")]
	public int expTotal;

	[Header("Inventory")]
	public int money;
	public SaveListUuid invItemEquip;
	public SaveListUuid invItemBag;
	public SaveListUuid invModuleEquip;
	public SaveListUuid invModuleBag;

	[Header("Time")]
	public int ingameDay;
	public int playedSeconds;


	public PlayerStatsSaveClass() {
		invItemEquip = new SaveListUuid(Constants.GEAR_EQUIP_SPACE);
		invItemBag = new SaveListUuid(Constants.GEAR_BAG_SPACE);
		invModuleEquip = new SaveListUuid(Constants.MODULE_EQUIP_SPACE);
		invModuleBag = new SaveListUuid(Constants.MODULE_BAG_SPACE);
	}
}
