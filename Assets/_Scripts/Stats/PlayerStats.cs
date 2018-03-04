using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public StringVariable currentChapter;
	public IntVariable currentArea;
	public IntVariable playerArea;
	public FloatVariable playerPosX;
	public FloatVariable playerPosY;

	[Header("Level")]
	public IntVariable playerLevel;
	public IntVariable totalExp;
	// public IntVariable expToNext;

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


	void Start() {
		StartCoroutine(CountPlayTime());
		CalculateExp();
		RecalculateStats();
	}

	IEnumerator CountPlayTime() {
        while (true)
        {
            yield return new WaitForSeconds(1);
            playedSeconds.value ++;
            _seconds = (playedSeconds.value % 60);
            _minutes = (playedSeconds.value / 60) % 60;
            _hours = (playedSeconds.value / 3600);
			playTime.value = string.Format("{0} : {1:D2} : {2:D2}",_hours, _minutes, _seconds);
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
		Debug.Log("Recalculating stats!");
		
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
	/// Puts the data into a save class for saving.
	/// </summary>
	/// <returns></returns>
	public PlayerStatsSaveClass SaveStats() {
		PlayerStatsSaveClass saveData = new PlayerStatsSaveClass();

		//Overworld
		saveData.currentChapter = currentChapter.value;
		saveData.currentArea = currentArea.value;
		saveData.playerArea = playerArea.value;
		saveData.playerPosX = playerPosX.value;
		saveData.playerPosY = playerPosY.value;

		//Exp
		saveData.expTotal = totalExp.value;

		//Inventory
		saveData.money = totalMoney.value;
		saveData.invItemBag = invItemBag.GenerateSaveData();
		saveData.invItemEquip = invItemEquip.GenerateSaveData();
		saveData.invModuleBag = invModuleBag.GenerateSaveData();
		saveData.invModuleEquip = invModuleEquip.GenerateSaveData();

		//Time
		saveData.ingameDay = ingameDay.value;
		saveData.playedSeconds = playedSeconds.value;

		return saveData;
	}

	/// <summary>
	/// Loads the player stats from the save class data.
	/// </summary>
	/// <param name="saveData"></param>
	public void LoadStats(PlayerStatsSaveClass saveData) {

		//Overworld
		currentArea.value = saveData.currentArea;
		playerArea.value = saveData.playerArea;
		playerPosX.value = saveData.playerPosX;
		playerPosY.value = saveData.playerPosY;

		//Exp
		totalExp.value = saveData.expTotal;

		//Inventory
		totalMoney.value = saveData.money;
		itemLibrary.GenerateDictionary();
		invItemBag.LoadItemData(saveData.invItemBag, itemLibrary);
		invItemEquip.LoadItemData(saveData.invItemEquip, itemLibrary);
		invModuleBag.LoadItemData(saveData.invModuleBag, itemLibrary);
		invModuleEquip.LoadItemData(saveData.invModuleEquip, itemLibrary);

		//Time
		ingameDay.value = saveData.ingameDay;
		playedSeconds.value = saveData.playedSeconds;
	}
}

/// <summary>
/// Save class for the player stats which extracts the ScrObj variables.
/// </summary>
public class PlayerStatsSaveClass {

	[Header("Overworld values")]
	public string currentChapter;
	public int currentArea;
	public int playerArea;
	public float playerPosX;
	public float playerPosY;

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
}
